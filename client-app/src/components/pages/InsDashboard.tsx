import React from "react";

interface InstructorState {
    email: string,
    courses: Array<String>
}

const InsDashboard: React.FC<InstructorState> = (props) => {
    const initialState: InstructorState ={
        email: "",
        courses: []
    }

    initialState.courses = ["algebra1", "algebra2"]

    const menuButtons = initialState.courses.map(course => (
        <ul>
            <li>
            <button>
                {course}
            </button>
            </li>
        </ul>
    ));

    return (
        <div>
            <h3>Hello {props.email}</h3>
            <p>
                Courses
                {menuButtons}
            </p>
        </div>
    );
};

export default InsDashboard;