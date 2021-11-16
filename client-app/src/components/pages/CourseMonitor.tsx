import React, { useCallback, useState } from "react";

import styles from "./Page.module.scss"

import CourseCreationForm from "components/CourseCreationForm";
import Stage from "components/Stage";

import InstructorModel from "models/InstructorModel";
import NewGameResponseModel from "models/NewGameResponseModel";
import StageEndModel from "models/StageEndModel";

interface CourseMonitorProps {
    instructorData: InstructorModel
}

const CourseMonitor: React.FC<CourseMonitorProps> = props => {
    const [gameData, setGameData] = useState<NewGameResponseModel | undefined>(undefined);

    const onStageFinish = useCallback(
        (data: StageEndModel) => {
            setGameData(
                oldGameData => ({
                    course_id: oldGameData!.course_id,
                    cname: oldGameData!.cname,
                    code: oldGameData!.code,
                    max_hp: data.new_max_hp,
                    name: data.new_stage_name,
                    stage_id: data.new_stage_id,
                    subject_id: data.new_subject_id,
                    subject_name: data.new_subject_name
                })
            );
        },
        [setGameData]
    )

    let contents: JSX.Element;

    if (gameData === undefined) {
        contents = (
            <CourseCreationForm instructorData={props.instructorData} onCourseCreated={setGameData}/>
        );
    } else {
        contents = (
            <Stage
                max_hp={gameData.max_hp}
                stageId={gameData.stage_id}
                stageName={gameData.name}
                courseCode={gameData.code.toString()}
                winMessage={"Your brilliant students have defeated the monster! Moving to next stage..."}
                onStageFinish={onStageFinish}
            />
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