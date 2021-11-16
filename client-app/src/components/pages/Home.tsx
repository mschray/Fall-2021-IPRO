import React from "react";
import { Link } from "react-router-dom";
import styles from "./Page.module.scss";

const Home: React.FC = () => {
    return (
        <div className={styles.content}>
            <h3>Home</h3>
            <ul className={styles.container}>
                <li><Link to="/student">Student Portal</Link></li>
                <li><Link to="/instructor">Instructor Portal</Link></li>
            </ul>
        </div>
    );
};

export default Home;