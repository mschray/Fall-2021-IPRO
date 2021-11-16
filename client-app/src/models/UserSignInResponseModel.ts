// Interface for UserSignIn JSON data returned from backend
export default interface UserSignInResponseModel {
    courseId: number,
    userId: number,
    instructorId: number,
    stageId: number
}

// Type guard for validating that data returned from the backend contains the expected fields
export function isUserSignInResponseModel(data: any): data is UserSignInResponseModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const usirData = data as UserSignInResponseModel;
    return typeof usirData.courseId === "number"
        && typeof usirData.userId === "number"
        && typeof usirData.instructorId == "number"
        && typeof usirData.stageId === "number";
}