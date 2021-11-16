import React from "react";

import styles from "./Page.module.scss";

import getAzureFunctions from "getAzureFunctions";
import useFetch, { FetchStatus } from "hooks/useFetch";

import { isSubjectNameModel } from "models/SubjectNameModel";
import UserSignInResponseModel from "models/UserSignInResponseModel";

import Game from "components/Game";

interface PlayProps {
    userData: UserSignInResponseModel;
}

const Play: React.FC<PlayProps> = props => {
    const url = new URL(getAzureFunctions().GetSubjectNameFromStageId);
    url.searchParams.append("stage_id", props.userData.stageId.toString());
    const subjectNameFetchResult = useFetch(
        url.toString(),
        (data) => {
            if (isSubjectNameModel(data)) {
                return data.subject_name;
            }
            return undefined;
        },
        [props.userData.stageId]
    );

    let contents: JSX.Element;

    if (subjectNameFetchResult.status === FetchStatus.Success) {
        contents = (
            <Game subjectName={subjectNameFetchResult.payload} />
        );
    } else if (subjectNameFetchResult.status === FetchStatus.InProgress) {
        contents = (
            <p>Fetching subject name from stage ID...</p>
        );
    } else {
        contents = (
            <p>Failed to fetch subject name from stage ID! Reason: {subjectNameFetchResult.reason}</p>
        );
    }

    return (
        <div className={styles.content}>
            <h3>Play</h3>
            {contents}
        </div>
    );
};

export default Play;