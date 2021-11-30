import React, { useState } from "react";

import styles from "./Question.module.scss";

import useForm from "hooks/useForm";
import QuestionModel from "models/QuestionModel";

import Latex from "react-latex";

// Properties for the Question React component
interface QuestionProperties {
    questionData: QuestionModel,
    onSolve: (questionData: QuestionModel) => void
}

// Interface for answer form
interface AnswerFormState {
    answer: string
}

enum AnswerResult {
    Correct,
    Incorrect,
    NoAnswersYet
}

const Question: React.FC<QuestionProperties> = (props) => {
    // Result of last question answered as a state variable
    const [lastAnswerResult, setLastAnswerResult] = useState(AnswerResult.NoAnswersYet);

    // Callback to be fired when the Submit button is entered
    async function submitAnswerCallback(state: AnswerFormState) {
        if (state.answer === props.questionData.answer_a || state.answer === props.questionData.answer_b) {
            setLastAnswerResult(AnswerResult.Correct);
            props.onSolve(props.questionData);
            // Clear input field
            setFormState({ answer: "" });
        } else {
            setLastAnswerResult(AnswerResult.Incorrect);
        }
    }
    
    // Default state for the Answer form
    const initialState: AnswerFormState = {
        answer: ""
    }
    
    // Create a Form state using the answer submission callback and the default state above
    const [formState, onFormChange, onFormSubmit, setFormState] = useForm(
        submitAnswerCallback,
        initialState
    );

    return (
        <div>
            <Latex>{`Solve: $${props.questionData.question}$`}</Latex>
            <p>Subject: {props.questionData.subject_name}</p>
            <form onSubmit={onFormSubmit}>
                <label>
                    Enter answer: 
                    <input name="answer" value={formState.answer} type="text" placeholder="Answer" autoComplete="off" onChange={onFormChange} />
                    <input type="submit" />
                </label>
            </form>
            {
                (lastAnswerResult === AnswerResult.Correct)
                    ? <p className={styles.correct}>That was correct!</p>
                    : (lastAnswerResult === AnswerResult.Incorrect)
                        ? <p className={styles.incorrect}>That was incorrect. Try again.</p>
                        : null
            }
        </div>
    )
};

export default Question;