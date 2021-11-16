// Interface for Subject Name JSON data returned from backend
export default interface SubjectNameModel {
    subject_name: string,
    max_hp: number
}

// Type guard for validating that data returned from the backend contains the expected fields
export function isSubjectNameModel(data: any): data is SubjectNameModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const subjectData = data as SubjectNameModel;
    return typeof subjectData.subject_name === "string"
        && typeof subjectData.max_hp === "number";
}