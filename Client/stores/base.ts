import { defineStore } from 'pinia';
import {
    type HubConnection,
    HubConnectionBuilder,
    HubConnectionState,
    LogLevel
} from "@microsoft/signalr";
import { GameType } from "~/types/enums";
import { getUrl } from "~/utils/game";

const runtimeConfig = useRuntimeConfig();

export const useBaseStore = defineStore('base', () => {
    const gameConnection = ref<HubConnection | null>(null);

    async function tryJoinRoom(gameType: GameType) {
        const webSocketUrl = `${ runtimeConfig.public.baseURL }/${ getUrl(gameType) }hub`;
        console.log("attempting WS with URL: ", webSocketUrl);

        // HubConnection configuration
        // https://learn.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-8.0&tabs=dotnet#configure-client-options
        gameConnection.value = new HubConnectionBuilder()
            .withUrl(webSocketUrl)
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

        await gameConnection.value.start();
        await gameConnection.value.invoke("JoinGame", {roomId:"123"});
        // await gameConnection.value.invoke("BasePing");
    }

    return { tryJoinRoom };
})