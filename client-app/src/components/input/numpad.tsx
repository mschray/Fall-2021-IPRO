import React, { useEffect } from "react";

import styles from "./numpad.module.scss";
import FiberManualRecordIcon from '@mui/icons-material/FiberManualRecord';
import CheckIcon from '@mui/icons-material/Check';
import ClearIcon from '@mui/icons-material/Clear';
import DeleteSweepIcon from '@mui/icons-material/DeleteSweep';

//Link to grid layout grid.layoutit.com?id=GINfCeW

/*
Handles the input cases from each of the buttons on the number pad.
Non-number buttons are represented using negatives.
-5 = backspace
-4 = negative
-3 = clear
-2 = submit
-1 = decimal point
*/
function handleNumpadButtonClick(num: number, onSubmitFunction: Function){
    //console.log("Numpad " + num + " was pressed!")
    var display = document.getElementsByClassName("numpad_display_text")[0];
    var lastChar = display.innerHTML.charAt(display.innerHTML.length -1);
    switch(num){      
        case -1:
            if(!(lastChar===".")){
                display.innerHTML=display.innerHTML+".";
            }         
            break;
        case -2:
            console.log("Submit");
            //Handle submit
            onSubmitFunction(display.innerHTML.substring(4));
            display.innerHTML="&gt;";
            break;
        case -3:
            display.innerHTML="&gt;";
            break;
        case -4:
            //console.log(display.innerHTML.charAt(display.innerHTML.length -1));           
            //console.log(lastChar + "|" + !(lastChar === '.'));
            if(isNaN(parseInt(lastChar)) && !(lastChar === '.')){ //Only allow - if the last number was not another number or a decimal point. Example of what this prevents: "324-3" and "3.4-4"
                display.innerHTML=display.innerHTML+"-";
            }
            break;
        case -5:
            display.innerHTML=display.innerHTML.substring(0, display.innerHTML.length -1);
            break;
        default: 
            display.innerHTML=display.innerHTML+num;
            break;
    }
}

interface NumpadProps {
    onSubmit(arg: string): void; //Ran when the enter key is pressed.
}

/*
Props: onSubmit = function(string)
    -Called when enter button is pressed with the collected input as the string parameter.
 */
const NumPad: React.FC<NumpadProps> = (props) => {
    const onSubmit = props.onSubmit;
    useEffect(() => {
        // Will run once since the second parameter [] is empty. Otherwise would run every time those parameters change.
        
        /*
        This code is used to capture keyboard input and push it to the numpad display.
        */
        //Code partially from https://javascript.plainenglish.io/how-to-detect-a-sequence-of-keystrokes-in-javascript-83ec6ffd8e93
        document.addEventListener('DOMContentLoaded', () => {
            let buffer: string[] = [];
            let lastKeyTime = Date.now();

            document.addEventListener('keydown', event => {
                const currentTime = Date.now();

                if (currentTime - lastKeyTime > 100) {
                    buffer = [];
                }

                buffer.push(event.key);
                lastKeyTime = currentTime;

                if(!(isNaN(parseInt(event.key)))){ //If number is entered
                    handleNumpadButtonClick(parseInt(event.key), onSubmit);
                }

                switch(event.key)
                {
                    case 'Enter':
                        handleNumpadButtonClick(-2, onSubmit);
                        break;
                    case '-':
                        handleNumpadButtonClick(-4, onSubmit);
                        break;
                    case '.':
                        handleNumpadButtonClick(-1, onSubmit);
                        break;
                    case 'Backspace':
                        handleNumpadButtonClick(-3, onSubmit);
                        break;
                    case 'Delete':
                        handleNumpadButtonClick(-5, onSubmit);
                        break;
                    case ' ':
                        handleNumpadButtonClick(-5, onSubmit); //Space is same as delete
                        break;
                    default:
                        break;
                }
                //console.log(buffer);
                //console.log(event.key === 'Delete');
            }); 
        });
    }, []);
    
    return (
        <div>
            <div className={styles.numpad_container}>
                <span className={styles.numpad_display}><h3 className="numpad_display_text" onClick={() =>handleNumpadButtonClick(-5, onSubmit)}>&gt;</h3></span>            
                <button onClick={() =>handleNumpadButtonClick(7, onSubmit)} className={[styles.num7, styles.numpad_button].join(" ")}>7</button>
                <button onClick={() =>handleNumpadButtonClick(8, onSubmit)} className={[styles.num8, styles.numpad_button].join(" ")}>8</button>
                <button onClick={() =>handleNumpadButtonClick(9, onSubmit)} className={[styles.num9, styles.numpad_button].join(" ")}>9</button>
                <button onClick={() =>handleNumpadButtonClick(4, onSubmit)} className={[styles.num4, styles.numpad_button].join(" ")}>4</button>
                <button onClick={() =>handleNumpadButtonClick(5, onSubmit)} className={[styles.num5, styles.numpad_button].join(" ")}>5</button>
                <button onClick={() =>handleNumpadButtonClick(6, onSubmit)} className={[styles.num6, styles.numpad_button].join(" ")}>6</button>
                <button onClick={() =>handleNumpadButtonClick(1, onSubmit)} className={[styles.num1, styles.numpad_button].join(" ")}>1</button>
                <button onClick={() =>handleNumpadButtonClick(2, onSubmit)} className={[styles.num2, styles.numpad_button].join(" ")}>2</button>
                <button onClick={() =>handleNumpadButtonClick(3, onSubmit)} className={[styles.num3, styles.numpad_button].join(" ")}>3</button>
                <button onClick={() =>handleNumpadButtonClick(0, onSubmit)} className={[styles.num0, styles.numpad_button].join(" ")}>0</button>
                <button onClick={() =>handleNumpadButtonClick(-1, onSubmit)} className={[styles.numdot, styles.numpad_button].join(" ")}><FiberManualRecordIcon sx={{ fontSize: "75%" }} /></button>
                <button onClick={() =>handleNumpadButtonClick(-2, onSubmit)} className={[styles.numenter, styles.numpad_button].join(" ")}><span><CheckIcon sx={{ fontSize: "140%"}} /></span></button>
                <button onClick={() =>handleNumpadButtonClick(-3, onSubmit)} className={[styles.numclear, styles.numpad_button].join(" ")}>C</button>
                <button onClick={() =>handleNumpadButtonClick(-4, onSubmit)} className={[styles.numneg, styles.numpad_button].join(" ")}>-</button>
            </div>
        </div>
    );
};

export default NumPad;
