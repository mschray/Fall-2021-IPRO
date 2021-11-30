import React, { useState } from "react";

import InstructorLogin from "components/pages/InstructorLogin";
import InstructorModel from "models/InstructorModel"; 
import CourseMonitor from "./CourseMonitor";

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
                <CourseMonitor instructorData={loginResult} />
        );
    }
};

export default InstructorPortal;