import React, {useState} from "react";

import { useForm } from "./useForm";

const Signup: React.FC = () => {
    const initialState = {
        fname: "",
        lname: "",
        email: "",
        username: "",
        password: "",
    };

    // getting the event handlers from our custom hook
    const { onSubmit, values } = useForm(
        loginUserCallback,
        initialState
    );

    // a submit function that will execute upon form submission
    async function loginUserCallback() {
        // send "values" to database
        alert('You have submitted the form.')
    }

    return (
        <div>
            <h3>Sign up</h3>
            <form onSubmit = {onSubmit}>
                <div>
                {/* <fieldset> */}
                    <label>
                        <p>First Name</p>
                        <input name="fname" placeholder="First name" type="text"/>
                    </label>

                    <label>
                        <p>Last Name</p>
                        <input name="lname" placeholder="Last name" type = "text"/>
                    </label>

                    <label>
                        <p>Email</p>
                        <input name="email" placeholder="Email" type = "email"/>
                    </label>

                    <label>
                        <p>Username</p>
                        <input name="username" placeholder="Username"/>
                    </label>

                    <label>
                        <p>Password</p>
                        <input name="password" placeholder="Password" type = "password"/>
                    </label>
        {/*    </fieldset> */}
                    <div>
                        <button type="submit">Submit</button>
                    </div>
                </div>
            </form>
        </div>
    );
};

export default Signup;