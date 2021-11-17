// Interface for Subject JSON data returned from backend
export default interface SubjectModel {
    id: number,
    subject_name: string
}

// Type guard for validating that data returned from the backend contains the expected fields
export function isSubjectModel(data: any): data is SubjectModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const subjectData = data as SubjectModel;
    return typeof subjectData.id === "number"
        && typeof subjectData.subject_name === "string"
}