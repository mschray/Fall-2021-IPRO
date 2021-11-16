import React, { useCallback, useState } from "react";

import StudentLogin from "components/pages/StudentLogin";
import Play from "components/pages/Play";

import StageEndModel from "models/StageEndModel";
import UserSignInResponseModel from "models/UserSignInResponseModel";

const StudentPortal: React.FC = () => {
    const [loginResult, setLoginResult] = useState<{userData: UserSignInResponseModel, courseCode: string} | undefined>(undefined);

    const onStageFinish = useCallback(
        (data: StageEndModel) => {
            setLoginResult(
                oldLoginResult => ({
                    userData: {
                        courseId: oldLoginResult!.userData.courseId,
                        userId: oldLoginResult!.userData.userId,
                        instructorId: oldLoginResult!.userData.instructorId,
                        stageId: data.new_stage_id
                    },
                    courseCode: oldLoginResult!.courseCode
                })
            );
        },
        [setLoginResult]
    )

    if (loginResult === undefined) {
        return (
            <StudentLogin onLogin={
                (newUserData, newCourseCode) => {
                    setLoginResult({
                        userData: newUserData,
                        courseCode: newCourseCode
                    });
                }
            }/>
        );
    } else {
        return (
            <Play
                userData={loginResult.userData}
                courseCode={loginResult.courseCode}
                onStageFinish={onStageFinish}
            />
        );
    }
};

export default StudentPortal;