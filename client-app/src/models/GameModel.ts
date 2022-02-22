// Interface for Game JSON data returned from backend
export default interface GameModel {
    subject_name: string,
    max_hp: number,
    stage_name: string
}

// Type guard for validating that data returned from the backend contains the expected fields
export function isGameModel(data: any): data is GameModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const gameData = data as GameModel;
    return typeof gameData.subject_name === "string"
        && typeof gameData.max_hp === "number"
        && typeof gameData.stage_name === "string";
}