import React from "react";

import styles from "./numpad.module.scss";
import FiberManualRecordIcon from '@mui/icons-material/FiberManualRecord';
import CheckIcon from '@mui/icons-material/Check';
import ClearIcon from '@mui/icons-material/Clear';

//Link to grid layout https://grid.layoutit.com/?id=ATBfViZ

/*
Handles the input cases from each of the buttons on the number pad.
Non-number buttons are represented using negatives.
-3 = clear
-2 = submit
-1 = period
*/
function handleNumpadButtonClick(num: number){
    //console.log("Numpad " + num + " was pressed!")
    var display = document.getElementsByClassName("numpad_display_text")[0];
    switch(num){      
        case -1:
            display.innerHTML=display.innerHTML+".";
            break;
        case -2:
            display.innerHTML="&gt;";
            //Handle submit
            break;
        case -3:
            display.innerHTML="&gt;";
            break;
        default: 
            display.innerHTML=display.innerHTML+num;
            break;
    }
}

const NumPad: React.FC = (props) => {
    return (
        <div>
            <span className={styles.numpad_display}><h3 className="numpad_display_text">&gt;</h3></span>
            <div className={styles.numpad_container}>
            <button onClick={() =>handleNumpadButtonClick(7)} className={[styles.num7, styles.numpad_button].join(" ")}>7</button>
            <button onClick={() =>handleNumpadButtonClick(8)} className={[styles.num8, styles.numpad_button].join(" ")}>8</button>
            <button onClick={() =>handleNumpadButtonClick(9)} className={[styles.num9, styles.numpad_button].join(" ")}>9</button>
            <button onClick={() =>handleNumpadButtonClick(4)} className={[styles.num4, styles.numpad_button].join(" ")}>4</button>
            <button onClick={() =>handleNumpadButtonClick(5)} className={[styles.num5, styles.numpad_button].join(" ")}>5</button>
            <button onClick={() =>handleNumpadButtonClick(6)} className={[styles.num6, styles.numpad_button].join(" ")}>6</button>
            <button onClick={() =>handleNumpadButtonClick(1)} className={[styles.num1, styles.numpad_button].join(" ")}>1</button>
            <button onClick={() =>handleNumpadButtonClick(2)} className={[styles.num2, styles.numpad_button].join(" ")}>2</button>
            <button onClick={() =>handleNumpadButtonClick(3)} className={[styles.num3, styles.numpad_button].join(" ")}>3</button>
            <button onClick={() =>handleNumpadButtonClick(0)} className={[styles.num0, styles.numpad_button].join(" ")}>0</button>
            <button onClick={() =>handleNumpadButtonClick(-1)} className={[styles.numdot, styles.numpad_button].join(" ")}><FiberManualRecordIcon sx={{ fontSize: "75%" }} /></button>
            <button onClick={() =>handleNumpadButtonClick(-2)} className={[styles.numenter, styles.numpad_button].join(" ")}><span><CheckIcon sx={{ fontSize: "100%"}} /></span></button>
            <button onClick={() =>handleNumpadButtonClick(-3)} className={[styles.numclear, styles.numpad_button].join(" ")}><span><ClearIcon sx={{ fontSize: "150%", lineHeight: "100%"}} /></span></button>
        </div>
        </div>
    );
};

export default NumPad;