import React, { useState } from "react";
import functionPlot from "function-plot";

interface FunctionProperties{
    function: string
}

const FunctionPlot: React.FC<FunctionProperties> = (props) => {


    let contentsBounds = document.body.getBoundingClientRect();
    
    let width = contentsBounds.width * 0.3
    let height = width

    functionPlot({
        target: "#root",
        width ,
        height,
        yAxis: { domain: [-5, 5] },
        xAxis: { domain: [-5, 5] },
        grid: true,
        data: [
        {
            fn: props.function,
        }
        ]
    })

    return(
        <div>
        </div>
    );
};

export default FunctionPlot;