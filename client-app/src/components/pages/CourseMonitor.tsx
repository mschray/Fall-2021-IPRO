import React, { useState } from "react";

import styles from "./Page.module.scss"

import CourseCreationForm from "components/CourseCreationForm";

import InstructorModel from "models/InstructorModel";
import NewGameResponseModel from "models/NewGameResponseModel";

interface CourseMonitorProps {
    instructorData: InstructorModel
}

const CourseMonitor: React.FC<CourseMonitorProps> = props => {
    const [gameData, setGameData] = useState<NewGameResponseModel | undefined>(undefined);

    let contents: JSX.Element;

    if (gameData === undefined) {
        contents = (
            <CourseCreationForm instructorData={props.instructorData} onCourseCreated={setGameData}/>
        );
    } else {
        contents = (
            <p>Made game!</p>
        );
    }

    return (
        <div className={styles.content}>
            <h3>Course Monitor - {props.instructorData.fname} {props.instructorData.lname}</h3>
            {contents}
        </div>
    );
}

export default CourseMonitor;