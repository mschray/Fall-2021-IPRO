import React from "react";

import styles from "./Page.module.scss"
import Fade from '@mui/material/Fade';
import PieCharts from 'components/diagrams/PieCharts'
import { Grid, MenuItem, Select, Typography } from "@mui/material";

const stats = [
    {
        subject: "exponent",
        values: [{ name: "Correct", score: 6 },
            { name: "Incorrect", score: 4 }]
    },
    {
        subject: "geometry",
        values: [{name: "Correct", score: 5 },
                { name: "Incorrect", score: 5 },]
    },
    {
        subject: "geometry",
        values: [{name: "Correct", score: 5 },
                { name: "Incorrect", score: 5 },]
    },
];

const students = [
    "Student 1",
    "Student 2",
    "Student 3"
]
const subjects = [
    "exponent",
    "geometry",
    "geometry"
]

const Options: React.FC = () => {
    return (
        <Fade in={true} timeout={500}>
            <div className={styles.content}>
                <h3>Options</h3>
                <p>Options will be added soon</p>
        
                <span>Student</span>
                <Select
                    label="Subject"
                    style={{width: 80}}
                    >
                    <MenuItem value="">
                        <em>None</em>
                    </MenuItem>
                    {students.map((student) =>(
                        <MenuItem value={10}>{student}</MenuItem>
                    ))}
                </Select>
                <span>Subject</span>
                <Select
                    label="Subject"
                    style={{width: 80}}
                    >
                    <MenuItem value="">
                        <em>None</em>
                    </MenuItem>
                    {subjects.map((subject) =>(
                        <MenuItem value={10}>{subject}</MenuItem>
                    ))}
                </Select>

                {students.map((value, index)=>(
                    <Grid container>
                    <Grid item xs={3}>
                        <p>{value}</p>
                    </Grid>
                    <Grid item xs={9}>
                        <PieCharts data={stats} name={"name"} value={"score"}/>
                    </Grid>

                    </Grid>
                ))}

            </div>
        </Fade>
    );
};

export default Options;