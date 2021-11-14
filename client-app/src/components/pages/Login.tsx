import React, { useState } from "react";

import styles from "./Page.module.scss"
import useForm from "hooks/useForm";

import IconButton from "@mui/material/IconButton";
import InputAdornment from "@mui/material/InputAdornment";
import TextField from "@mui/material/TextField";
import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";
import { Link } from "react-router-dom";

interface LoginFormState {
    email: string,
    password: string,
};

const Login: React.FC = () => {
    const initialState: LoginFormState = {
        email: "",
        password: ""
    };

    // a submit function that will execute upon form submission
    async function loginUserCallback() {
        // send "values" to database
        fetch(window.location.href)
            .then(resp => {
                window.location.href = "/InsDashboard";
            })
    }

    const [visible, setVisible] = useState(false);
    const toggleVisibility = () => {
        setVisible(!visible);
    };
    // getting the event handlers from our custom hook
    const [formState, onFormChange, onFormSubmit] = useForm(
        loginUserCallback,
        initialState
    );

    <Link
to={{
    pathname: "/InsDashboard",
    state: {email: formState.email} // your data array of objects
  }}
></Link>

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
    );
};

export default Login;