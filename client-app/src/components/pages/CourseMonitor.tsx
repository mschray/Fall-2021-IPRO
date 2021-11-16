import React from "react";

import styles from "./Page.module.scss"

import CourseCreationForm from "components/CourseCreationForm";

import InstructorModel from "models/InstructorModel";

interface CourseMonitorProps {
    instructorData: InstructorModel
}

const CourseMonitor: React.FC<CourseMonitorProps> = props => {
    return (
        <div className={styles.content}>
            <h3>Course Monitor - {props.instructorData.fname} {props.instructorData.lname}</h3>
            <CourseCreationForm instructorData={props.instructorData} />
        </div>
    );
}

export default CourseMonitor;