import React, { useState } from "react";

import styles from "./Question.module.scss";

import QuestionModel from "models/QuestionModel";
import determineQuestionFormatting from "determineQuestionFormatting";
import NumPad from "components/input/numpad";

// Properties for the Question React component
interface QuestionProperties {
    questionData: QuestionModel,
    onSolve: (questionData: QuestionModel, correct: number) => void,
    questionNumber?: number
}

enum AnswerResult {
    Correct,
    Incorrect,
    NoAnswersYet
}

const Question: React.FC<QuestionProperties> = (props) => {
    // Result of last question answered as a state variable
    const [lastAnswerResult, setLastAnswerResult] = useState(AnswerResult.NoAnswersYet);

    async function submitAnswer(answer: string) {
        if (answer === props.questionData.answer_a || answer === props.questionData.answer_b) { //If correct answer
            setLastAnswerResult(AnswerResult.Correct);
            props.onSolve(props.questionData, 1);
        } else { //If not correct answer
            props.onSolve(props.questionData, 0);
            setLastAnswerResult(AnswerResult.Incorrect);
        }
    }

    const questionNumberLabel = (props.questionNumber !== undefined) ? `(${props.questionNumber})` : null;

    const [questionStatement, _] = determineQuestionFormatting(props.questionData);

    return (
        <>
            <div style={{gridArea: "question"}}>
                <p>{questionNumberLabel} {questionStatement}</p>
                {
                    (lastAnswerResult === AnswerResult.Correct)
                        ? <p className={styles.correct}>That was correct!</p>
                        : (lastAnswerResult === AnswerResult.Incorrect)
                            ? <p className={styles.incorrect}>That was incorrect. Try again.</p>
                            : null
                }
            </div>
            <NumPad
                onSubmit={submitAnswer}
            />
        </>
    )
};

export default Question;