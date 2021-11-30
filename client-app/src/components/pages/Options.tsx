import React from "react";

import styles from "./Page.module.scss"
import Fade from '@mui/material/Fade';

const Options: React.FC = () => {
    return (
        <Fade in={true} timeout={500}>
            <div className={styles.content}>
                <h3>Options</h3>
            </div>
        </Fade>
    );
};

export default Options;