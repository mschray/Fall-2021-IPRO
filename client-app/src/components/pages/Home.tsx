import React, { useState } from "react";
import { Link } from "react-router-dom";
import styles from "./Page.module.scss";
import homeStyles from "./Home.module.scss";
import Button from "@mui/material/Button";
import AddIcon from "@mui/icons-material/Add";
import CreateIcon from "@mui/icons-material/Create";

import getAzureFunctions from "getAzureFunctions";
import useFetch, { FetchStatus } from "hooks/useFetch";
import { isSubjectModel } from "models/SubjectModel";

const SubjectSelector: React.FC<{ callback: (name: string) => void }> = (props) => {
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
            <ul key={subject.id} className={homeStyles.columnContainer}>
                <h2>{subject.subject_name}</h2>
            </ul>
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

const Home: React.FC = () => {
    const [subjectName, setSubjectName] = useState<string | undefined>(undefined);

    return (
        <div className={styles.content}>
            <h3>Home</h3>
            <div className={homeStyles.columnContainer}>
                <div>
                    <SubjectSelector callback={setSubjectName}/>
                </div>
                <div className={homeStyles.buttonContainer}>
                    <Button className={homeStyles.createButton} href="/login" variant="contained" color="primary" startIcon={<CreateIcon />}>Create a game</Button>
                    <Button className={homeStyles.joinButton} href="/studentlogin" variant="contained" color="success" startIcon={<AddIcon />}>Join a game</Button>
                </div>
            </div>
        </div>
    );
};

export default Home;