// Interface for Stage End model sent from the backend
export default interface StageEndModel {
    end_of_stage: true,
    new_stage_id: number,
    new_max_hp: number,
    new_stage_name: string,
    new_subject_id: number,
    new_subject_name: string
}

// Type guard for validating that data returned from the backend contains the expected fields
export function isStageEndModel(data: any): data is StageEndModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const stageEventData = data as StageEndModel;
    return typeof stageEventData.end_of_stage === "boolean"
        && stageEventData.end_of_stage === true
        && typeof stageEventData.new_stage_id === "number"
        && typeof stageEventData.new_max_hp === "number"
        && typeof stageEventData.new_stage_name === "string"
        && typeof stageEventData.new_subject_id === "number"
        && typeof stageEventData.new_subject_name === "string";
}