// Interface for Question JSON data returned from backend
export default interface StudentLoginModel {
    id: number,
    cname: string,
    code: string,
    stage_id: number
}

// Type guard for validating that data returned from the backend contains the expected fields
// Calling this function with the returned JSON object will subsequently typecheck the JSON as a QuestionData, woo!
export function isStudentLoginModel(data: any): data is StudentLoginModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const studentLoginData = data as StudentLoginModel;
    return typeof studentLoginData.id === "number"
        && typeof studentLoginData.cname === "string"
        && typeof studentLoginData.code === "string"
        && typeof studentLoginData.stage_id === "number"
}