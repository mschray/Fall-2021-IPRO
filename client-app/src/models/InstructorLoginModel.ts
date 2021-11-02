// Interface for Question JSON data returned from backend
export default interface InstructorLoginModel {
    id: number,
    email: string,
    is_online: boolean
}

// Type guard for validating that data returned from the backend contains the expected fields
// Calling this function with the returned JSON object will subsequently typecheck the JSON as a QuestionData, woo!
export function isInstructorLoginModel(data: any): data is InstructorLoginModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const instructorData = data as InstructorLoginModel;
    return typeof instructorData.id === "number"
        && typeof instructorData.email === "string"
        && typeof instructorData.is_online === "boolean"
}