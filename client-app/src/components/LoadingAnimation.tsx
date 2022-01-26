import React from "react";

import monsterLoadGif from "assets/monster_load.gif";
import styles from "./LoadingAnimation.module.scss";

const LoadingAnimation: React.FC = () => {
    return (
        <img
            className={styles.gif}
            src={monsterLoadGif}
            alt="Loading..."
        />
    )
};

export default LoadingAnimation;