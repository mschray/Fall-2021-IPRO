import React, { useState } from "react";

import styles from "./Page.module.scss"
import useForm from "hooks/useForm";
import TextField from "@mui/material/TextField";
import Fade from '@mui/material/Fade';
import getAzureFunctions from "getAzureFunctions";
import UserSignInResponseModel, { isUserSignInResponseModel } from "models/UserSignInResponseModel";
import LoadingAnimation from "components/LoadingAnimation";

interface StudentLoginProps {
    onLogin: (data: UserSignInResponseModel, courseCode: string) => void
}

interface StudentLoginFormState {
    userEmail: string,
    courseCode: string,
};

const StudentLogin: React.FC<StudentLoginProps> = props => {
    // login request pending state
    const [isLoginPending, setIsLoginPending] = useState(false);

    // login error message state
    const [loginErrorState, setLoginErrorState] = useState<string | undefined>(undefined);

    const initialFormState: StudentLoginFormState = {
        userEmail: "",
        courseCode: ""
    };

    // a submit function that will execute upon form submission
    async function loginUserCallback(formState: StudentLoginFormState) {
        if (isLoginPending)
            return;

        setIsLoginPending(true);

        const url = new URL(getAzureFunctions().UserSignIn);
        // Store course code in a variable since we use it later (and it may change since it's after the PUT request)
        const courseCode = formState.courseCode;
        url.searchParams.append("courseCode", courseCode);
        url.searchParams.append("userEmail", formState.userEmail);

        const requestInfo: RequestInit = { method: "PUT" };

        fetch(url.toString(), requestInfo)
            .then(response => {
                setIsLoginPending(false);
                return response.json();
            })
            .then(json => {
                console.log(json);
                if (isUserSignInResponseModel(json)) {
                    // Fire onLogin callback with the UserSignInResponse model data
                    props.onLogin(json, courseCode);
                } else {
                    // Data doesn't conform to UserSignInResponse model
                    setLoginErrorState("Unexpected data returned from backend. Check the console.");
                }
            })
            .catch(err => {
                // Error occurred furing PUT request
                setIsLoginPending(false);
                console.error(err);
                setLoginErrorState("Incorrect course code was inputted. Please try again.");
            });
    }

    // getting the event handlers from our custom hook
    const [formState, onFormChange, onFormSubmit] = useForm(
        loginUserCallback,
        initialFormState
    );

    const form = (
        <form onSubmit={onFormSubmit}>
            <div>
                <TextField name="userEmail" sx={{ m: 1, width: '25ch' }} required label="Email" value={formState.userEmail} placeholder="Email" type="email" onChange={onFormChange} />
            </div>
            <br/>
            <div>                
                <TextField name="courseCode" required sx={{ m: 1, width: '25ch' }} label="Course Code" value={formState.courseCode} placeholder="Course Code" type="text" onChange={onFormChange} />
            </div>
            {
                loginErrorState !== undefined
                    ? <p className={styles.error}>{loginErrorState}</p>
                    : <br />
            }
            <button name="Join class">Join Class</button>
        </form>
    );

    const loading = (
        <>
            <p>Joining game...</p>
            <LoadingAnimation />
        </>
    )

    return (
        <Fade in={true} timeout={500}>
            <div className={styles.content}>
                <h3>Student Log In</h3>
                {
                    (!isLoginPending)
                        ? form
                        : loading
                }
            </div>
        </Fade>
    );
};

export default StudentLogin;