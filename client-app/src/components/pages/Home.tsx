import React from "react";
import { Link } from "react-router-dom";
import styles from "./Page.module.scss";
import homeStyles from "./Home.module.scss";
import Button from "@mui/material/Button";
import AddIcon from "@mui/icons-material/Add";
import CreateIcon from "@mui/icons-material/Create";

const Home: React.FC = () => {
    return (
        <div className={styles.content}>
            <h3>Home</h3>
            <div className={homeStyles.overallContainer}>
                <div className={styles.container}>
                    <h1>Your Stats</h1>
                </div>
                <div className={styles.container}>
                    <h1>Available Subjects</h1>
                </div>
                <div className={homeStyles.buttonContainer}>
                    <Button className={homeStyles.createButton} href="/login" variant="contained" color="primary" startIcon={<CreateIcon />}>Create a game</Button>
                    <Button className={homeStyles.joinButton} href="/studentlogin" variant="contained" color="success" startIcon={<AddIcon />}>Join a game</Button>
                </div>
            </div>
        </div>
    );
};

export default Home;