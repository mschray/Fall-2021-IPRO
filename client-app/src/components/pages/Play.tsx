import React, { useState } from "react";
import _ from "lodash";

import azureFunctions from "azureFunctions";
import useFetch, { FetchStatus } from "hooks/useFetch";

import { isQuestionModel } from "models/QuestionModel";
import { isSubjectModel } from "models/SubjectModel";
import Question from "components/Question";

const NQUESTIONS = 5;

const SubjectSelector: React.FC<{callback: (name: string) => void}> = (props) => {
    // Fetch the subjects
    const fetchResult = useFetch(
        azureFunctions.GetSubjects,
        (data) => {
            // The Azure function should return the data as an array of SubjectModels
            if (Array.isArray(data) && data.every(isSubjectModel)) {
                return data;
            }
            return undefined;
        },
        []
    );

    if (fetchResult.status === FetchStatus.Success) {
        const subjectListItems = fetchResult.payload.map((subject) => (
            <li key={subject.id}>
                <button onClick={() => {props.callback(subject.subject_name)}}>{subject.subject_name}</button>
            </li>
        ))
        return (
            <ul>
                {subjectListItems}
            </ul>
        );
    } else if (fetchResult.status === FetchStatus.Failure) {
        // Notify user that the subjects list couldn't be fetched
        return (
            <p>Could not fetch subjects!</p>
        )
    } else {
        // Notify user that the subjects list is currently being fetched
        return (
            <p>Fetching subjects list...</p>
        )
    }
}

const Game: React.FC<{subjectName: string}> = (props) => {
    // Fetch the questions
    const fetchResult = useFetch(
        azureFunctions.GetQuestionsBySubject + "&subject=" + encodeURIComponent(props.subjectName),
        (data) => {
            // The Azure function should return the data as an array of QuestionModels
            if (Array.isArray(data) && data.every(isQuestionModel)) {
                return _.sampleSize(data, NQUESTIONS);
            }
            return undefined;
        },
        [props.subjectName]
    );

    // Current question number as a state variable
    const [questionIndex, setQuestionIndex] = useState(0);

    if (fetchResult.status === FetchStatus.Success) {
        if (fetchResult.payload.length === 0) {
            return (
                <p>No questions for this subject.</p>
            )
        }
        return (
            <Question
                id={fetchResult.payload[questionIndex].id}
                onSolve={() => {
                    setQuestionIndex((questionIndex) => {
                        if (questionIndex < fetchResult.payload.length - 1) {
                            return questionIndex + 1;
                        } else {
                            alert("Congrats! You won!");
                            return questionIndex;
                        }
                    });
                }}
            />
        )
    } else if (fetchResult.status === FetchStatus.Failure) {
        // Notify user that the questions couldn't be fetched
        return (
            <p>Could not fetch questions!</p>
        )
    } else {
        // Notify user that the questions are currently being fetched
        return (
            <p>Fetching questions for subject...</p>
        )
    }
}

const Play: React.FC = () => {
    const [subjectName, setSubjectName] = useState<string | undefined>(undefined);

    return (
        <div>
            <h3>Play</h3>
            {
                (subjectName === undefined)
                    ? <SubjectSelector callback={setSubjectName} />
                    : <Game subjectName={subjectName} />
            }
        </div>
    );
};

export default Play;