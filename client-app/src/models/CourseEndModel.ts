// Interface for Course End model sent from the backend
export default interface CourseEndModel {
    end_of_course: true
}

// Type guard for validating that data returned from the backend contains the expected fields
export function isCourseEndModel(data: any): data is CourseEndModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const courseEndData = data as CourseEndModel;
    return typeof courseEndData.end_of_course === "boolean"
        && courseEndData.end_of_course === true;
}