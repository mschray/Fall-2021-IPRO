import React, { useState } from "react";
import functionPlot from "function-plot";

interface FunctionProperties{
    function: string
}

const FunctionPlot: React.FC = () => {
    const [functionValue, setFunctionValue] = useState<string>("");

    let contentsBounds = document.body.getBoundingClientRect();
    let width = 800;
    let height = 500;
    let ratio = contentsBounds.width / width;
    width *= ratio;
    height *= ratio;

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setFunctionValue(event.target.value)
    }
    const handleSubmit = () => {
        functionPlot({
            target: "#root",
            width,
            height,
            yAxis: { domain: [-1, 9] },
            grid: true,
            data: [
              {
                fn: functionValue,
          
              }
            ]
          });
    }

    return(
        <div>
            <form onSubmit={handleSubmit}>
                <label>
                Input function:
                    <input type="text" />
                </label>
                <input type="submit" value="Submit" onChange ={handleChange}/>
            </form>
        </div>
    );
};

export default FunctionPlot;