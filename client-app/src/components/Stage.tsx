import React, { useCallback, useEffect, useState } from "react";

import styles from "./Game.module.scss";

import monsterGif from "assets/monster_havoc.gif"

import getAzureFunctions from "getAzureFunctions";

import { isCourseEndModel } from "models/CourseEndModel";
import StageEndModel, { isStageEndModel } from "models/StageEndModel";
import { isStageEventModel } from "models/StageEventModel";

import ProgressBar from "components/ProgressBar";

interface StageProps {
    max_hp: number,
    stageName: string,
    stageId: number,
    courseCode: string,
    winMessage: string,
    onStageFinish: (data: StageEndModel) => void,
    onCourseFinish: () => void,
    forceFetchStageEvents?: number                  // Used to force Stage to re-fetch stage events when set to a new number (hacky!)
}

// Time (in milliseconds) between each GetEvents request
const EVENTS_INTERVAL = 2500;

const Stage: React.FC<StageProps> = (props) => {
    // Current HP of monster as a state variable
    const [monsterHP, setMonsterHP] = useState(props.max_hp);
    // Error message string
    const [eventsErrorMessage, setEventsErrorMessage] = useState<string | undefined>(undefined);

    // need to destructure function props since React doesn't like having them in dependency arrays
    const onStageFinish = props.onStageFinish;
    const onCourseFinish = props.onCourseFinish;

    // Callback to fetch stage events
    const fetchStageEvents = useCallback(
        () => {
            if (props.forceFetchStageEvents !== undefined)
                console.log(`Re-fetch force number: ${props.forceFetchStageEvents}`);

            const url = new URL(getAzureFunctions().GetEvents);
            url.searchParams.append("stage", props.stageId.toString());
            url.searchParams.append("course_code", props.courseCode);
            fetch(url.toString())
                .then(response => response.json())
                .then(json => {
                    console.log(json)
                    if (Array.isArray(json) && json.every(isStageEventModel)) {
                        if (json.length > 0) {
                            const newHP = props.max_hp - json.map(stageEvent => stageEvent.inflicted_hp).reduce((hp1, hp2) => hp1 + hp2);
                            setMonsterHP(newHP);
                        } else {
                            setMonsterHP(props.max_hp);
                        }
                        setEventsErrorMessage(undefined);
                    } else if (isStageEndModel(json)) {
                        onStageFinish(json);
                    } else if (isCourseEndModel(json)) {
                        onCourseFinish();
                    } else {
                        setEventsErrorMessage("Received unexpected data from the backend. Check console.");
                    }
                })
                .catch(err => {
                    console.log(err);
                    setEventsErrorMessage("Unexpected error occured while fetching stage events. Check console.");
                });
        },
        [setEventsErrorMessage, setMonsterHP, props.stageId, props.courseCode, props.max_hp, onStageFinish, onCourseFinish, props.forceFetchStageEvents]
    );

    // Periodically update HP based on stage events
    useEffect(
        () => {
            // Immediately fetch stage events
            fetchStageEvents();
            // Continue to fetch stage events every EVENTS_INTERVAL milliseconds
            console.log("starting ping interval!");
            const interval = setInterval(fetchStageEvents, EVENTS_INTERVAL);
            return () => {
                console.log("clearing ping interval!");
                clearInterval(interval);
            }
        },
        [fetchStageEvents]
    );

    const hp = Math.max(monsterHP, 0);

    const contents = (
        <>
            <h4>Stage: {props.stageName}</h4>
            <p>Course code: {props.courseCode}</p>
            <img
                className={styles.gif}
                src={monsterGif}
                alt="This evil monster is destroying Jebraville! Solve math questions to defeat the monster."
            />
            <p>This evil monster is destroying Jebraville! Solve math questions to defeat the monster.</p>
            <ProgressBar alpha={hp / props.max_hp}/>
        </>
    );

    if (eventsErrorMessage !== undefined) {
        return (
            <p>{eventsErrorMessage}</p>
        );
    } else if (hp > 0) {
        return (
            <>
                {contents}
                {props.children} 
            </>
        );
    } else {
        return (
            <>
                {contents}
                <p>{props.winMessage}</p>
            </>
        );
    }
}

export default Stage;