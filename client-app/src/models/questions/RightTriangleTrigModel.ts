// Interface for Right Triangle JSON data returned from backend
export default interface RightTriangleTrigModel {
    a: number,
    b: number,
    c: number,
    angle: "A" | "B",
    function: "sine" | "cosine" | "tangent" | "arcsine" | "arccosine" | "arctangent"
}

// Type guard for validating that data returned from the backend contains the expected fields
export function isRightTriangleTrigModel(data: any): data is RightTriangleTrigModel {
    if (typeof data !== "object" || Array.isArray(data) || data === null) {
        return false;
    }

    const shapedData = data as RightTriangleTrigModel;
    return typeof shapedData.a === "number"
        && typeof shapedData.b === "number"
        && typeof shapedData.c === "number"
        && (shapedData.angle === "A" || shapedData.angle === "B")
        && (shapedData.function === "sine" || shapedData.function === "arcsine"
            || shapedData.function === "cosine" || shapedData.function === "arccosine"
            || shapedData.function === "tangent" || shapedData.function === "arctangent");
}
