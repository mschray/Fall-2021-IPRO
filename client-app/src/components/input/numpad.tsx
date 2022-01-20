import React from "react";

import styles from "./numpad.module.scss";
import FiberManualRecordIcon from '@mui/icons-material/FiberManualRecord';
import CheckIcon from '@mui/icons-material/Check';

const NumPad: React.FC = (props) => {
    return (
        <div className={styles.numpad_container}>
            <button className={[styles.num7, styles.numpad_button].join(" ")}>7</button>
            <button className={[styles.num8, styles.numpad_button].join(" ")}>8</button>
            <button className={[styles.num9, styles.numpad_button].join(" ")}>9</button>
            <button className={[styles.num4, styles.numpad_button].join(" ")}>4</button>
            <button className={[styles.num5, styles.numpad_button].join(" ")}>5</button>
            <button className={[styles.num6, styles.numpad_button].join(" ")}>6</button>
            <button className={[styles.num1, styles.numpad_button].join(" ")}>1</button>
            <button className={[styles.num2, styles.numpad_button].join(" ")}>2</button>
            <button className={[styles.num3, styles.numpad_button].join(" ")}>3</button>
            <button className={[styles.num0, styles.numpad_button].join(" ")}>0</button>
            <button className={[styles.numdot, styles.numpad_button].join(" ")}><FiberManualRecordIcon sx={{ fontSize: "75%" }} /></button>
            <button className={[styles.numenter, styles.numpad_button].join(" ")}><span><CheckIcon sx={{ fontSize: "200%", boxSizing:"content-box" }} /></span></button>
        </div>
    );
};

export default NumPad;