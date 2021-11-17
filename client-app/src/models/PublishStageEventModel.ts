import getAzureFunctions from "getAzureFunctions";

// Interface for Publish Stage Event model sent to the backend
export default interface PublishStageEventModel {
    stage_id: number,
    course_id: number,
    origin_user_id: number,
    question_id: number,
    inflicted_hp: number,
    was_correct: number,
    event_time: string
}

export function publishStageEvent(data: PublishStageEventModel) {
    const url = new URL(getAzureFunctions().PublishStageEvent);

    const requestInfo: RequestInit = {
        method: "PUT",
        body: JSON.stringify(data)
    };

    return fetch(url.toString(), requestInfo);
}