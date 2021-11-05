import React from "react";
import { Link } from "react-router-dom";
import styles from "./Page.module.scss";
import homeStyles from "./Home.module.scss";

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
                <ul className={styles.container}>
                    <li><Link to="/studentlogin">Student login</Link></li>
                    <li><Link to="/login">Instructor login</Link></li>
                </ul>
            </div>
        </div>
    );
};

export default Home;