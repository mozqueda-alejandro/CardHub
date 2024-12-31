import { defineStore } from 'pinia';
import { HttpTransportType, type HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { GameType } from "~/types/enums";
import { getUrl } from "~/utils/game";
import { useCookie } from "#app";

const runtimeConfig = useRuntimeConfig();


export const useBaseStore = defineStore('base', () => {
    const { $api } = useNuxtApp();

    const gameConnection = ref<HubConnection | null>(null);
    const jwtToken = useCookie<string>("jwt-token");
    // if (!jwtToken.value) {
    $api("/room", {
        method: "POST",
        body: JSON.stringify(GameType.Une)
    }).then((res) => {
        jwtToken.value = res;
    })
        .catch((err: Error) => {
            console.log("JWT not acquired: ", err)
        });
    // }

    const result = $api("/test", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${ jwtToken.value }`
        }
    });

    async function tryJoinRoom(gameType: GameType) {
        const webSocketUrl = `${ runtimeConfig.public.baseURL }/${ getUrl(gameType) }hub`;

        // HubConnection configuration
        // https://learn.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-8.0&tabs=dotnet#configure-client-options
        gameConnection.value = new HubConnectionBuilder()
            .withUrl(webSocketUrl,
                {
                    accessTokenFactory: () => {
                        return jwtToken.value;
                    },
                    // skipNegotiation: true,
                    // transport: HttpTransportType.WebSockets
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

    return { tryJoinRoom };
})