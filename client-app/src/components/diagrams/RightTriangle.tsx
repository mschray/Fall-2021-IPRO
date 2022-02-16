import React from "react";

import styles from "./RightTriangle.module.scss";

interface RightTriangleProps {
    a: number,
    b: number,
    c: number,
    angle: "A" | "B",
    hideA?: boolean,
    hideB?: boolean
}

const TRIANGLE_WIDTH = 400; // Not actual pixels, the SVG viewport can be scaled responsively
const PADDING = 8;          // Padding around the triangle so that the stroke doesn't get clipped

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

    const points = [
        [0, 0],
        [0, height],
        [TRIANGLE_WIDTH, height]
    ];

    const pointsString = points
        .map(([x, y]) => `${x + PADDING},${y + PADDING}`)
        .join(" ");

    return (
        <svg width={TRIANGLE_WIDTH + 2*PADDING} height={height + 2*PADDING}>
            <polygon
                className={styles.triangle}
                points={pointsString}
            />
        </svg>
    )
}

export default RightTriangle;