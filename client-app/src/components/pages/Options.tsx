import React from "react";

import styles from "./Page.module.scss";

import Student from "components/Student";

const Options: React.FC = () => {
    return (
        <div className={styles.content}>
            <h3>Options</h3>
            <Student code="code=773202" />
        </div>
    );
};

export default Options;