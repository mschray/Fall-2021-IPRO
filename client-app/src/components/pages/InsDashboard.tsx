import Button from "@mui/material/Button";
import React from "react";
import styles from "./Page.module.scss";
import Login from "./Login";

interface InstructorState {
    email: string,
    courses: Array<String>
}

const InsDashboard: React.FC = () => {
    const initialState: InstructorState ={
        email: "",
        courses: []
    }

    initialState.courses = ["algebra1", "algebra2"]
    const menuButtons = initialState.courses.map(course => (
        <ul>
            <li>
            <Button>
                {course}
            </Button>
            </li>
        </ul>
    ));
    return (
        <div className={styles.content}>
            <h3>Hello instructor</h3>
            <p>
                Courses
                {menuButtons}
            </p>
        </div>
    );
};

export default InsDashboard;