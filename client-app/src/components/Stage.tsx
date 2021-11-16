import React, { useCallback, useEffect, useState } from "react";

import styles from "./Game.module.scss";

import monsterGif from "assets/monster_havoc.gif"

import getAzureFunctions from "getAzureFunctions";

import { isStageEndModel } from "models/StageEndModel";
import { isStageEventModel } from "models/StageEventModel";

import ProgressBar from "components/ProgressBar";

interface GameProps {
    max_hp: number,
    stageId: number,
    courseCode: string,
    winMessage: string,
    onStageFinish: (newStage: number) => void,
    onHPUpdate?: (newHP: number) => void,
    hpOverride?: number
}

// Time (in milliseconds) between each GetEvents request
const EVENTS_INTERVAL = 2500;

const Game: React.FC<GameProps> = (props) => {
    // Current HP of monster as a state variable
    const [monsterHP, setMonsterHP] = useState(props.max_hp);
    const [eventsErrorMessage, setEventsErrorMessage] = useState<string | undefined>(undefined);

    // need to destructure function props since React doesn't like having them in dependency arrays
    const onStageFinish = props.onStageFinish;
    const onHPUpdate = props.onHPUpdate;

    // Callback to fetch stage events
    const fetchStageEvents = useCallback(
        () => {
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
                            if (onHPUpdate !== undefined)
                                onHPUpdate(newHP);
                            setMonsterHP(newHP);
                        } else {
                            if (onHPUpdate !== undefined)
                                onHPUpdate(props.max_hp);
                            setMonsterHP(props.max_hp);
                        }
                        setEventsErrorMessage(undefined);
                    } else if (isStageEndModel(json)) {
                        onStageFinish(json.NewStage);
                    } else {
                        setEventsErrorMessage("Received unexpected data from the backend. Check console.");
                    }
                })
                .catch(err => {
                    console.log(err);
                    setEventsErrorMessage("Unexpected error occured while fetching stage events. Check console.");
                });
        },
        [setEventsErrorMessage, setMonsterHP, props.stageId, props.courseCode, props.max_hp, onStageFinish, onHPUpdate]
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

    const hp = (props.hpOverride !== undefined) ? props.hpOverride : monsterHP;

    if (eventsErrorMessage !== undefined) {
        return (
            <p>{eventsErrorMessage}</p>
        );
    } else if (hp > 0) {
        return (
            <>
                <img
                    className={styles.gif}
                    src={monsterGif}
                    alt="Evil monster is destroying Jebraville! Solve math questions to kill the monster."
                />
                <ProgressBar alpha={hp / props.max_hp}/>
                {props.children}
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
                <p>{props.winMessage}</p>
            </>
        );
    }
}

export default Game;