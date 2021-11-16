import React, { useState } from "react";

import StudentLogin from "components/pages/StudentLogin";
import Play from "components/pages/Play";

import UserSignInResponseModel from "models/UserSignInResponseModel";

const StudentPortal: React.FC = () => {
    const [userData, setUserData] = useState<UserSignInResponseModel | undefined>(undefined);

    if (userData === undefined) {
        return (
            <StudentLogin onLogin={setUserData}/>
        );
    } else {
        return (
            <Play />
        );
    }
};

export default StudentPortal;