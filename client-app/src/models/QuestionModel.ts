// Interface for Question JSON data returned from backend
export default interface QuestionModel {
    id: number,
    answer_a: string,
    answer_b: string | null,
    question: string,
    subject_name: string,
    is_json: boolean
}

// Type guard for validating that data returned from the backend contains the expected fields
export function isQuestionModel(data: any): data is QuestionModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const questionData = data as QuestionModel;
    return typeof questionData.id === "number"
        && typeof questionData.answer_a === "string"
        && (typeof questionData.answer_b === "string" || questionData.answer_b === null)
        && typeof questionData.question === "string"
        && typeof questionData.subject_name === "string"
        && typeof questionData.is_json === "boolean";
}