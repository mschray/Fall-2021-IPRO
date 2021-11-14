import React from "react";

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
        <div>
            Hello instructor
        </div>
    );
};

export default InsDashboard;