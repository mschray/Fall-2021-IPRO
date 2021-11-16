import React, { useCallback } from "react";

import InstructorModel from "models/InstructorModel";
import getAzureFunctions from "getAzureFunctions";
import { isSubjectModel } from "models/SubjectModel";

import useFetch, { FetchStatus } from "hooks/useFetch";
import useForm from "hooks/useForm";

import TextField from "@mui/material/TextField";
import Select from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";

interface CourseCreationFormProps {
    instructorData: InstructorModel
}

interface NewGameFormState {
    subjectIndex: number,
    courseName: string,
    stageHp: number,
    stageName: string
}

const CourseCreationForm: React.FC<CourseCreationFormProps> = props => {
    // Fetch the subjects
    const subjectFetchResult = useFetch(
        getAzureFunctions().GetSubjects,
        (data) => {
            // The Azure function should return the data as an array of SubjectModels
            if (Array.isArray(data) && data.every(isSubjectModel)) {
                return data;
            }
            return undefined;
        },
        []
    );

    const initialFormState: NewGameFormState = {
        subjectIndex: 0,
        courseName: "",
        stageHp: 100,
        stageName: ""
    };

    // Need to memo-ize the callback since it relies on subjectFetchResult
    const newGameRequest = useCallback(
        (formState: NewGameFormState) => {
            if (subjectFetchResult.status !== FetchStatus.Success)
                return;
            
            const url = new URL(getAzureFunctions().NewGame);
            url.searchParams.append("subjectName", subjectFetchResult.payload[formState.subjectIndex].subject_name);
            url.searchParams.append("courseName", formState.courseName);
            url.searchParams.append("instructorEmail", props.instructorData.email);
            url.searchParams.append("stageHp", formState.stageHp.toString());
            url.searchParams.append("stageName", formState.stageName);

            const requestInfo: RequestInit = { method: "PUT" };

            fetch(url.toString(), requestInfo)
                .then(response => response.json())
                .then(json => {
                    console.log(json);
                })
                .catch(err => {
                    console.error(err);
                });
        },
        [subjectFetchResult, props.instructorData.email]
    );

    // async callback simply calls memo-ized callback
    async function newGameFormCallback(formState: NewGameFormState) {
        newGameRequest(formState);
    }

    // getting the event handlers from our custom hook
    const [formState, onFormChange, onFormSubmit] = useForm(
        newGameFormCallback,
        initialFormState
    );

    if (subjectFetchResult.status === FetchStatus.Success) {
        return (
            <form onSubmit={onFormSubmit}>
                <div>
                    <Select
                        required
                        name="subjectIndex"
                        id="subjectIndex"
                        sx={{ m: 1, width: '24ch' }}
                        label="Subject"
                        value={formState.subjectIndex}
                        onChange={(event) => {onFormChange(event as React.ChangeEvent<{ name: string, value: string }>);}}
                    >
                        {
                            subjectFetchResult.payload.map((subject, index) => (
                                <MenuItem value={index} key={subject.id}>{subject.subject_name}</MenuItem>
                            ))
                        }
                    </Select>
                </div>
                <br/>
                <div>
                    <TextField
                        required
                        name="courseName"
                        id="courseName"
                        sx={{ m: 1, width: '25ch' }}
                        label="Course Name"
                        value={formState.courseName}
                        placeholder="Jebra Course"
                        type="text"
                        onChange={onFormChange}
                    />
                </div>
                <br/>
                <div>                
                    <TextField
                        required
                        name="stageName"
                        id="stageName"
                        sx={{ m: 1, width: '25ch' }}
                        label="Stage Name"
                        value={formState.stageName}
                        placeholder="Jebra Stage"
                        type="text"
                        onChange={onFormChange}
                    />
                </div>
                <br/>
                <div>                
                    <TextField
                        required
                        name="stageHp"
                        id="stageHp"
                        sx={{ m: 1, width: '25ch' }}
                        label="Stage HP"
                        value={formState.stageHp}
                        placeholder="Stage HP"
                        type="number"
                        onChange={onFormChange}
                    />
                </div>
                <br/>
                <button name="CreateCourse">Create Course</button>
            </form>
        );
    } else if (subjectFetchResult.status === FetchStatus.InProgress) {
        return (
            <p>Fetching subject list...</p>
        );
    } else {
        return (
            <p>Failed to fetch subject list! Reason: {subjectFetchResult.reason}</p>
        );
    }
}

export default CourseCreationForm;