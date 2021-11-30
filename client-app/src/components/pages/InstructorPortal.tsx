import React, { useState } from "react";

import InstructorLogin from "components/pages/InstructorLogin";
import InstructorModel from "models/InstructorModel"; 
import CourseMonitor from "./CourseMonitor";
import Fade from '@mui/material/Fade';

const InstructorPortal: React.FC = () => {
    const [loginResult, setLoginResult] = useState<InstructorModel | undefined>(undefined);

    if (loginResult === undefined) {
        return (
            <InstructorLogin
                onLogin={
                    (data) => {
                        setLoginResult(data);
                    }
                }
            />
        );
    } else {
        return (
            <Fade in={true} timeout={500}>
                <CourseMonitor instructorData={loginResult} />
            </Fade>
        );
    }
};

export default InstructorPortal;