import React from "react";
import styles from "./Page.module.scss";

interface InstructorState {
    email: string,
    courses: Array<String>
}
const InsDashboard: React.FC = () => {
    const initialState: InstructorState ={
        email: "",
        courses: []
    }

    return (
        <div className={styles.content}>
            Hello instructor
        </div>
    );
};

export default InsDashboard;