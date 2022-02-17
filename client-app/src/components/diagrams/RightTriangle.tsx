import React from "react";

import styles from "./RightTriangle.module.scss";

interface RightTriangleProps {
    a: number,
    b: number,
    c: number,
    angle: "A" | "B",
    hideA?: boolean,    // For inverse trig questions, we may want to hide unneeded side lengths
    hideB?: boolean
}

interface Vertex {
    x: number,
    y: number,
    label: string,
    textAnchor: "start" | "middle" | "end",
    alignmentBaseline: "baseline" | "middle" | "hanging"
}

const TRIANGLE_WIDTH = 400; // Not actual pixels, the SVG viewport can be scaled responsively
const PADDING = 32;         // Padding around the triangle so that the text doesn't get clipped

// Offset for text labels depending on alignment
const TEXT_OFFSET_X = {
    end: -8,
    middle: 0,
    start: 8
}
const TEXT_OFFSET_Y = {
    baseline: -8,
    middle: 0,
    hanging: 8
}

const RightTriangle: React.FC<RightTriangleProps> = props => {
    /*
    Note that a < b < c in length. We want the triangle to span
    the full width of its bounding box, so we let the side along
    the horizontal axis be side b. Thus, side a, which is shorter
    than side b, will lie along the vertical axis, minimizing the
    height of the diagram.
    */

    const pixelsPerUnit = TRIANGLE_WIDTH / props.b;
    const height = pixelsPerUnit * props.a;

    // Array of vertices
    let vertices: Vertex[] = [
        {
            x: 0,
            y: 0,
            label: "B",
            textAnchor: "end",
            alignmentBaseline: "baseline"
        },
        {
            x: 0,
            y: height,
            label: "C",
            textAnchor: "end",
            alignmentBaseline: "hanging"
        },
        {
            x: TRIANGLE_WIDTH,
            y: height,
            label: "A",
            textAnchor: "start",
            alignmentBaseline: "hanging"
        }
    ];
    // Array of midpoints
    let midpoints: Vertex[] = [
        {
            x: TRIANGLE_WIDTH/2,
            y: height/2,
            label: props.c.toString(),
            textAnchor: "start",
            alignmentBaseline: "baseline"
        }
    ];
    if (!props.hideA) {
        midpoints.push({
            x: 0,
            y: height/2,
            label: props.a.toString(),
            textAnchor: "end",
            alignmentBaseline: "middle"
        });
    }
    if (!props.hideB) {
        midpoints.push({
            x: TRIANGLE_WIDTH/2,
            y: height,
            label: props.b.toString(),
            textAnchor: "middle",
            alignmentBaseline: "hanging"
        });
    }

    // Add padding
    const addPadding = (vertex: Vertex) => ({
        ...vertex,
        x: vertex.x + PADDING,
        y: vertex.y + PADDING
    });
    vertices = vertices.map(addPadding);
    midpoints = midpoints.map(addPadding);

    // Convert vertex array to string of points
    const points = vertices
        .map(({x, y}) => `${x},${y}`)
        .join(" ");

    // Create text labels for angles and side lengths
    const labels = vertices
        .concat(midpoints)
        .map(vertex => (
            <text
                x={vertex.x + TEXT_OFFSET_X[vertex.textAnchor]}
                y={vertex.y + TEXT_OFFSET_Y[vertex.alignmentBaseline]}
                textAnchor={vertex.textAnchor}
                alignmentBaseline={vertex.alignmentBaseline}
            >
                {vertex.label}
            </text>
        ));

    return (
        <svg width={TRIANGLE_WIDTH + 2*PADDING} height={height + 2*PADDING}>
            <polygon
                className={styles.triangle}
                points={points}
            />
            {labels}
        </svg>
    )
}

export default RightTriangle;