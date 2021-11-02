import React from "react";

import styles from "./Page.module.scss"
import useForm from "hooks/useForm";
import {TextField } from "@mui/material";

interface LoginFormState {
    email: string,
    course_id: string,
};

const StudentLogin: React.FC = () => {
    const initialState: LoginFormState = {
        email: "",
        course_id: ""
    };

    // a submit function that will execute upon form submission
    async function loginUserCallback() {
        // send "values" to database
        alert('You have submitted the form.')
    }

    // getting the event handlers from our custom hook
    const [formState, onFormChange, onFormSubmit] = useForm(
        loginUserCallback,
        initialState
    );

    return (
        <div className={styles.content}>
            <h3>Log in</h3>
            <form onSubmit={onFormSubmit}>
                <div>
                    <TextField name="email" sx={{ m: 1, width: '25ch' }} required label="Email" value={formState.email} placeholder="Email" type="email" onChange={onFormChange} />
                </div>
                <br/>
                <div>                
                    <TextField name="course_id" required sx={{ m: 1, width: '25ch' }} label="course ID" value={formState.course_id} placeholder="Course ID" type="text" onChange={onFormChange} />
                </div>
                <br/>
                <button name="Join class">Join Class</button>
            </form>
        </div>
    );
};

export default StudentLogin;