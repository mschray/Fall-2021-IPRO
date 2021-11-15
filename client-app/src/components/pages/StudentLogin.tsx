import React from "react";

import styles from "./Page.module.scss"
import useForm from "hooks/useForm";
import TextField from "@mui/material/TextField";

import getAzureFunctions from "getAzureFunctions";

interface LoginFormState {
    userEmail: string,
    courseCode: string,
};

const StudentLogin: React.FC = () => {
    const initialState: LoginFormState = {
        userEmail: "",
        courseCode: ""
    };

    // a submit function that will execute upon form submission
    async function loginUserCallback(formState: LoginFormState) {
        const url = new URL(getAzureFunctions().UserSignIn);
        url.searchParams.append("courseCode", formState.courseCode);
        url.searchParams.append("userEmail", formState.userEmail);

        const requestInfo: RequestInit = { method: "PUT" };

        fetch(url.toString(), requestInfo)
            .then(response => response.text())
            .then(text => {console.log(text)})
            .catch(err => {console.error(err)});
        
        
    }

    // getting the event handlers from our custom hook
    const [formState, onFormChange, onFormSubmit] = useForm(
        loginUserCallback,
        initialState
    );

    return (
        <div className={styles.content}>
            <h3>Student Log In</h3>
            <form onSubmit={onFormSubmit}>
                <div>
                    <TextField name="userEmail" sx={{ m: 1, width: '25ch' }} required label="Email" value={formState.userEmail} placeholder="Email" type="email" onChange={onFormChange} />
                </div>
                <br/>
                <div>                
                    <TextField name="courseCode" required sx={{ m: 1, width: '25ch' }} label="Course Code" value={formState.courseCode} placeholder="Course Code" type="text" onChange={onFormChange} />
                </div>
                <br/>
                <button name="Join class">Join Class</button>
            </form>
        </div>
    );
};

export default StudentLogin;