import React, { useEffect, useState } from "react";
import PropTypes from "prop-types"


const ProgressBar= (props: { value: any; }) => {
    var value = 100;
    if (props.value == 1){
        value = 80;
    }
    if (props.value == 2){
        value = 60;
    }
    if (props.value == 3){
        value = 40;
    }
    if (props.value == 4){
        value = 20;
    }
    if (props.value == 5){
        value = 0;
    }

    return (
        <div>
            <progress value={value} max ="100" />
        </div>
    );
};

ProgressBar.propTypes = {
    value: PropTypes.number.isRequired,
}
ProgressBar.defaultProps ={
    max: 100
}
export default ProgressBar;