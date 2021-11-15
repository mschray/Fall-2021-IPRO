import React, { useState } from "react";

import styles from "./Page.module.scss"
import useForm from "hooks/useForm";

import IconButton from "@mui/material/IconButton";
import InputAdornment from "@mui/material/InputAdornment";
import TextField from "@mui/material/TextField";
import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";
import InsDashboard from "./InsDashboard";

interface LoginFormState {
    email: string,
    password: string,
    signedIn: boolean
};

const Login: React.FC = () => {
    const initialState: LoginFormState = {
        email: "",
        password: "",
        signedIn: false
    };
    const [signedIn, setSignedIn] = useState(false);
    const [visible, setVisible] = useState(false);

    // a submit function that will execute upon form submission
    async function loginUserCallback() {
        // send "values" to database
        setSignedIn(!signedIn)
    }

    const toggleVisibility = () => {
        setVisible(!visible);
    };
    const handleSignOut = () => {
        setSignedIn(!signedIn)
    }
    // getting the event handlers from our custom hook
    const [formState, onFormChange, onFormSubmit] = useForm(
        loginUserCallback,
        initialState
    );

    if (signedIn === false) {
        return (
            <div className={styles.content}>
                <h3>Log in</h3>
                <form onSubmit={onFormSubmit}>
                    <div>
                        <TextField name="email" sx={{ m: 1, width: '25ch' }} required label="Email" value={formState.email} placeholder="Email" type="email" onChange={onFormChange} />
                    </div>
                    <br/>
                    <div>                
                        <TextField name="password" required label="Password" value={formState.password} placeholder="Password" onChange={onFormChange}
                            type={visible ? 'text' : 'password'} 
                            sx={{ m: 1, width: '25ch' }}
                            InputProps={{
                                endAdornment: (
                                    <InputAdornment position="end">
                                        <IconButton onClick={toggleVisibility}>
                                            {visible ? <VisibilityOff /> : <Visibility />}
                                        </IconButton>
                                    </InputAdornment>
                                )
                            }}
                        />
                    </div>
                    <br/>
                    <input type="submit" value="Submit" />
                </form>
                <p> New to Jebra? <a href="/Signup">Create an account</a></p>
            </div>
        );}
    else {
        return (
            <div className={styles.content}>
                <InsDashboard email={formState.email} courses={[]}/>
                <button onClick ={handleSignOut}>Logout</button>
            </div>
        );
    }
    };

export default Login;