import React from "react";

import useFetch, { FetchStatus } from "hooks/useFetch";
import useForm from "hooks/useForm";

import { isStudentLoginModel } from "models/StudentLoginModel";

// Properties for the Question React component
interface StudentLoginProperties {
    code: string
}

// Interface for answer form
interface AnswerFormState {
    answer: string
}

const Student: React.FC<StudentLoginProperties> = (props) => {
    // Fetch the question data using the GetQuestion function
    const url = new URL("http://localhost:7071/api/UserSignIn");
    url.searchParams.append("code", props.code.toString());
    const fetchResult = useFetch(
        url.toString(),
        (data) => {
            // The Azure function should return the data as an array with one QuestionModel object inside it
            if (Array.isArray(data) && isStudentLoginModel(data[0])) {
                return data[0];
            }
            return undefined;
        },
        [props.code]
    );

    // Callback to be fired when the Submit button is entered
    async function submitAnswerCallback(state: AnswerFormState) {
        if (fetchResult.status === FetchStatus.Success) {
            if (state.answer === fetchResult.payload.code) {
                alert("Correct!");
            } else {
                alert("Incorrect!");
            }
        } else {
            alert("Question isn't determined yet");
        }
    }
    
    // Default state for the Answer form
    const initialState: AnswerFormState = {
        answer: ""
    }
    
    // Create a Form state using the answer submission callback and the default state above
    const [formState, onFormChange, onFormSubmit] = useForm(
        submitAnswerCallback,
        initialState
    );

    if (fetchResult.status === FetchStatus.Success) {
        // Display question if fetching was successful
        const payload = fetchResult.payload;
        return (
            <div>
                <p>Solve: {payload.code}</p>
                <form onSubmit={onFormSubmit}>
                    <label>
                        Enter answer: 
                        <input name="answer" value={formState.answer} type="text" placeholder="Answer" onChange={onFormChange} />
                        <input type="submit" />
                    </label>
                </form>
            </div>
        )
    } else if (fetchResult.status === FetchStatus.Failure) {
        // Notify user that the question couldn't be fetched
        return (
            <p>Could not fetch question! Reason: {fetchResult.reason}</p>
        )
    } else {
        // Notify user that the question is currently being fetched
        return (
            <p>Fetching question data...</p>
        )
    }
};

export default Student;