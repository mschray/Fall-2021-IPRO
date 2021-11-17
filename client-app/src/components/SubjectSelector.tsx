import React from "react";

import getAzureFunctions from "getAzureFunctions";
import useFetch, { FetchStatus } from "hooks/useFetch";

import SubjectModel, { isSubjectModel } from "models/SubjectModel";

interface SubjectSelectorProps {
    onSelection: (subject: SubjectModel) => void
}

const SubjectSelector: React.FC<SubjectSelectorProps> = (props) => {
    // Fetch the subjects
    const fetchResult = useFetch(
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

    if (fetchResult.status === FetchStatus.Success) {
        const subjectListItems = fetchResult.payload.map((subject) => (
            <li key={subject.id}>
                <button onClick={() => {props.onSelection(subject)}}>{subject.subject_name}</button>
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
            <p>Could not fetch subjects! Reason: {fetchResult.reason}</p>
        )
    } else {
        // Notify user that the subjects list is currently being fetched
        return (
            <p>Fetching subjects list...</p>
        )
    }
}

export default SubjectSelector;