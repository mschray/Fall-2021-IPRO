// Interface for Stage Event model sent from the backend
export default interface StageEventModel {
    id: number,
    inflicted_hp: number,
    was_correct: boolean,
    event_time: string
}

// Type guard for validating that data returned from the backend contains the expected fields
export function isStageEventModel(data: any): data is StageEventModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const stageEventData = data as StageEventModel;
    return typeof stageEventData.id === "number"
        && typeof stageEventData.inflicted_hp === "number"
        && typeof stageEventData.was_correct === "boolean"
        && typeof stageEventData.event_time === "string";
}