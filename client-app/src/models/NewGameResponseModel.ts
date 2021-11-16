// Interface for New Game JSON data returned from backend
export default interface NewGameResponseModel {
    course_id: number,
    cname: string,
    code: number,
    max_hp: number,
    name: string,
    stage_id: number,
    subject_id: number,
    subject_name: string
}

// Type guard for validating that data returned from the backend contains the expected fields
export function isNewGameResponseModel(data: any): data is NewGameResponseModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const newGameData = data as NewGameResponseModel;
    return typeof newGameData.course_id === "number"
        && typeof newGameData.cname === "string"
        && typeof newGameData.code === "number"
        && typeof newGameData.max_hp === "number"
        && typeof newGameData.name === "string"
        && typeof newGameData.stage_id === "number"
        && typeof newGameData.subject_id === "number"
        && typeof newGameData.subject_name === "string";
}