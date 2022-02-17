import React, { useState } from "react";

import styles from "./Question.module.scss";

import useForm from "hooks/useForm";
import QuestionModel from "models/QuestionModel";
import { isRightTriangleTrigModel } from "models/questions/RightTriangleTrigModel";

import Latex from "react-latex";

import TextField from "@mui/material/TextField";
import RightTriangle from "components/diagrams/RightTriangle";

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
        return !isNaN(Number(question.answer_a)) && !isNaN(Number(question.answer_b));
    }
    return !isNaN(Number(question.answer_a));
}

function getDecimalPlaces(answerOrQuestion: string | QuestionModel): number {
    if (typeof answerOrQuestion === "string") {
        const parts = answerOrQuestion.split(".");

        if (parts.length < 2)
            return 0;

        return parts[1].length;
    } else {
        if (answerOrQuestion.answer_b !== null) {
            return Math.max(
                getDecimalPlaces(answerOrQuestion.answer_a),
                getDecimalPlaces(answerOrQuestion.answer_b)
            );
        } else {
            return getDecimalPlaces(answerOrQuestion.answer_a);
        }
    }
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

    let diagram: JSX.Element | null = null;
    let questionStatement = (
        <>Solve: <Latex>{props.questionData.question}</Latex></>
    );

    if (props.questionData.is_json) {
        const parsed = JSON.parse(props.questionData.question);
        if (isRightTriangleTrigModel(parsed)) {
            diagram = (
                <RightTriangle
                    a={parsed.a}
                    b={parsed.b}
                    c={parsed.c}
                    angle={parsed.angle}
                />
            );

            if (props.questionData.subject_name === "Trig Functions") {
                const latexFunc = (parsed.function === "sine")    ? "sin"
                                : (parsed.function === "cosine")  ? "cos"
                                                                  : "tan";
                const latexString = `$\\${latexFunc}{${parsed.angle}}$`;
                questionStatement = (
                    <>Solve: <Latex>{latexString}</Latex> as a reduced fraction</>
                );
            } else if (props.questionData.subject_name === "Inverse Trig Functions") {
                const latexString = `$m \\angle ${parsed.angle}$`;
                questionStatement = (
                    <>Solve: <Latex>{latexString}</Latex> rounded to the nearest hundredth of a degree</>
                );
            }
        }
    }

    return (
        <div>
            {diagram}
            <p>{questionNumberLabel} {questionStatement}</p>
            <form onSubmit={onFormSubmit}>
                <TextField
                    required
                    name="answer"
                    id="answer"
                    sx={{ m: 1, width: '25ch' }}
                    label="Answer"
                    value={formState.answer}
                    placeholder="Answer"
                    type={hasNumericAnswer(props.questionData) ? "number" : "text"}
                    inputProps={
                        hasNumericAnswer(props.questionData)
                            ? { step: Math.pow(10, -getDecimalPlaces(props.questionData)) }
                            : {}
                    }
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