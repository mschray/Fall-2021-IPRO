// Interface for Stage End model sent from the backend
export default interface StageEndModel {
    EndOfStage: true,
    NewStage: number
}

// Type guard for validating that data returned from the backend contains the expected fields
export function isStageEndModel(data: any): data is StageEndModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const stageEventData = data as StageEndModel;
    return typeof stageEventData.EndOfStage === "boolean"
        && stageEventData.EndOfStage === true
        && typeof stageEventData.NewStage === "number";
}