import React, { useCallback, useState } from "react";

import styles from "./Game.module.scss";

import monsterGif from "assets/monster_havoc.gif"

import getAzureFunctions from "getAzureFunctions";
import useFetch, { FetchStatus } from "hooks/useFetch";

import GameModel from "models/GameModel";
import { publishStageEvent } from "models/PublishStageEventModel";
import QuestionModel, { isQuestionModel } from "models/QuestionModel";
import UserSignInResponseModel from "models/UserSignInResponseModel";

import Question from "components/Question";
import ProgressBar from "components/ProgressBar";

interface GameProps {
    game: GameModel,
    userData: UserSignInResponseModel
}

// Amount of damage to deal per correct answer
const INFLICTED_HP = 10;

const Game: React.FC<GameProps> = (props) => {
    // Fetch the questions
    const url = new URL(getAzureFunctions().GetQuestionsBySubject);
    url.searchParams.append("subject", props.game.subject_name);
    const fetchResult = useFetch(
        url.toString(),
        (data) => {
            // The Azure function should return the data as an array of QuestionModels
            if (Array.isArray(data) && data.every(isQuestionModel)) {
                return data;
            }
            return undefined;
        },
        [props.game.subject_name]
    );

    const onQuestionSolve = useCallback(
        (questionData: QuestionModel) => {
            publishStageEvent({
                stage_id: props.userData.stageId,
                course_id: props.userData.courseId,
                origin_user_id: props.userData.userId,
                question_id: questionData.id,
                inflicted_hp: INFLICTED_HP,
                was_correct: 1,
                event_time: new Date().toISOString().slice(0, 19).replace('T', ' ')
            });
            setQuestionIndex(questionIndex => questionIndex + 1);
        },
        [props.userData.stageId, props.userData.courseId, props.userData.userId]
    );

    // Current question number as a state variable
    const [questionIndex, setQuestionIndex] = useState(0);

    if (fetchResult.status === FetchStatus.Success) {
        if (fetchResult.payload.length === 0) {
            return (
                <p>No questions for this subject.</p>
            );
        } else if (questionIndex < fetchResult.payload.length) {
            return (
                <>
                    <img
                        className={styles.gif}
                        src={monsterGif}
                        alt="Evil monster is destroying Jebraville! Solve math questions to kill the monster."
                    />
                    <ProgressBar alpha={1 - questionIndex / fetchResult.payload.length}/>
                    <p>Question #{questionIndex + 1}:</p>
                    <Question
                        questionData={fetchResult.payload[questionIndex]}
                        onSolve={onQuestionSolve}
                    />
                </>
            );
        } else {
            return (
                <>
                    <img
                        className={styles.gif}
                        src={monsterGif}
                        alt="Evil monster is destroying Jebraville! Solve math questions to kill the monster."
                    />
                    <ProgressBar alpha={0}/>
                    <p>Congratulations, you won!</p>
                </>
            );
        }
    } else if (fetchResult.status === FetchStatus.Failure) {
        // Notify user that the questions couldn't be fetched
        return (
            <p>Could not fetch questions! Reason: {fetchResult.reason}</p>
        )
    } else {
        // Notify user that the questions are currently being fetched
        return (
            <p>Fetching questions for subject...</p>
        )
    }
}

export default Game;