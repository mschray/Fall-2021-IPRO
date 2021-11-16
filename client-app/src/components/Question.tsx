import React from "react";

import useForm from "hooks/useForm";
import QuestionModel from "models/QuestionModel";

// Properties for the Question React component
interface QuestionProperties {
    questionData: QuestionModel,
    onSolve: (questionData: QuestionModel) => void
}

// Interface for answer form
interface AnswerFormState {
    answer: string
}

const Question: React.FC<QuestionProperties> = (props) => {
    // Callback to be fired when the Submit button is entered
    async function submitAnswerCallback(state: AnswerFormState) {
        if (state.answer === props.questionData.answer_a) {
            alert("Correct!");
            props.onSolve(props.questionData);
        } else {
            alert("Incorrect!");
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

    return (
        <div>
            <p>Solve: {props.questionData.question}</p>
            <p>Subject: {props.questionData.subject_name}</p>
            <form onSubmit={onFormSubmit}>
                <label>
                    Enter answer: 
                    <input name="answer" value={formState.answer} type="text" placeholder="Answer" onChange={onFormChange} />
                    <input type="submit" />
                </label>
            </form>
        </div>
    )
};

export default Question;