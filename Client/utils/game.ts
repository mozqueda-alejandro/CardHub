import { GameType } from "~/types/enums";

interface GameInfo {
    name: string;
    url: string;
}

export const gamesSet: Record<GameType, GameInfo> = {
    [GameType.Tens]: { name: "Tens", url: "tens" },
    [GameType.Une]: { name: "Une", url: "une" }
};

export function getName(type: GameType) {
    return gamesSet[type].name;
}

export function getUrl(type: GameType) {
    return gamesSet[type].url;
}
