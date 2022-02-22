// Interface for Instructor JSON data returned from backend
export default interface InstructorModel {
    id: number,
    fname: string,
    lname: string,
    username: string,
    pass: string,
    email: string
}

// Type guard for validating that data returned from the backend contains the expected fields
export function isInstructorModel(data: any): data is InstructorModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const instructorData = data as InstructorModel;
    return typeof instructorData.id === "number"
        && typeof instructorData.fname === "string"
        && typeof instructorData.lname === "string"
        && typeof instructorData.username === "string"
        && typeof instructorData.pass === "string"
        && typeof instructorData.email === "string";
}