import { defineStore } from 'pinia';
import { type HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { GameType } from "~/types/enums";
import { getUrl } from "~/utils/game";
import { useCookie } from "#app";

const runtimeConfig = useRuntimeConfig();


export const useBaseStore = defineStore('base', () => {
    const { $api } = useNuxtApp();

    const gameConnection = ref<HubConnection | null>(null);
    const jwtToken = useCookie<string>("ch_token");

    // const result = $api("/test", {
    //     method: "GET",
    //     headers: {
    //         "Content-Type": "application/json",
    //         "Authorization": `Bearer ${ jwtToken.value }`
    //     }
    // });

    async function tryCreateRoom(gameType: GameType) {


        // HubConnection configuration
        // https://learn.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-8.0&tabs=dotnet#configure-client-options
        const webSocketUrl = `${ runtimeConfig.public.baseURL }/${ getUrl(gameType) }hub`;
        gameConnection.value = new HubConnectionBuilder()
            .withUrl(webSocketUrl,
                {
                    accessTokenFactory: () => {
                        return jwtToken.value;
                    }
                }
            )
            .withStatefulReconnect()
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();

        gameConnection.value.onclose(() => {
            console.log("DISCONNECTED FROM SERVER");
        });

        gameConnection.value.onreconnecting(() => {
            console.log("RECONNECTING TO SERVER");
        });

        gameConnection.value.onreconnected(() => {
            console.log("RECONNECTED TO SERVER");
        });

        gameConnection.value.on("Pong", () => {
            console.log("Pong from Server");
        });

        gameConnection.value.on("BasePong", () => {
            console.log("Base Pong from Server");
        });

        await gameConnection.value.start();
        await gameConnection.value.invoke("BasePing");
        await gameConnection.value.invoke("Ping");
        // await gameConnection.value.invoke("JoinGame", {roomId:"123"});
    }

    async function tryJoinRoom(roomPin: string) {

    }

    async function createGBTokenRequest(gameType: GameType): Promise<string> {
        return $api<string>("/tokens/gameboard", {
            method: "POST",
            body: JSON.stringify(gameType)
        });
    }

    async function createPlayerTokenRequest(roomPin: number): Promise<string> {
        return $api("/tokens/player", {
            method: "POST",
            body: JSON.stringify(roomPin)
        });
    }

    async function updateTokenRequest(token: string): Promise<string> {
        return $api("/tokens/update", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${ token }`
            }
        });
    }

    // convert to interface
    // public record CreateGBTokenRequest(GameType GameType);
    // public record CreatePlayerTokenRequest(int RoomPin);
    // public record UpdateTokenRequest(IClient Client);

    interface CreateGBTokenRequest {
        GameType: GameType;
    }

    interface CreatePlayerTokenRequest {
        RoomPin: number;
    }

    interface UpdateTokenRequest {
        Client: IClient;
    }

    return { tryJoinRoom };
})