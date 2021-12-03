import React, { useState } from "react";

import styles from "./Question.module.scss";

import useForm from "hooks/useForm";
import QuestionModel from "models/QuestionModel";

import Latex from "react-latex";

import TextField from "@mui/material/TextField";

// Properties for the Question React component
interface QuestionProperties {
    questionData: QuestionModel,
    onSolve: (questionData: QuestionModel) => void,
    questionNumber?: number
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

function hasNumericAnswer(question: QuestionModel): boolean {
    if (question.answer_b !== null) {
        return (Number(question.answer_a) !== NaN) && (Number(question.answer_b) !== NaN);
    }
    return (Number(question.answer_a) !== NaN);
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

    const questionNumberLabel = (props.questionNumber !== undefined) ? `(${props.questionNumber})` : null;

    return (
        <div>
            <Latex>{`${questionNumberLabel} Solve: ${props.questionData.question}`}</Latex>
            <form onSubmit={onFormSubmit}>
                <TextField
                    required
                    name="answer"
                    id="answer"
                    sx={{ m: 1, width: '25ch' }}
                    label="Answer"
                    value={formState.answer}
                    placeholder="Answer"
                    type={(hasNumericAnswer(props.questionData)) ? "number" : "text"}
                    autoComplete="off"
                    onChange={onFormChange}
                />
                <br />
                <input type="submit" />
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