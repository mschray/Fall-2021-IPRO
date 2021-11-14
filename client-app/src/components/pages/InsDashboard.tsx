import React from "react";
import styles from "./Page.module.scss";
import Login from "./Login";

interface InstructorState {
    email: string,
    courses: Array<String>
}

interface InsDashBoardProperties {
    email: String
}

const InsDashboard: React.FC = () => {
    const initialState: InstructorState ={
        email: "",
        courses: []
    }

    return (
        <div className={styles.content}>
            <p>Email: </p> 
        </div>
    );
};

export default InsDashboard;