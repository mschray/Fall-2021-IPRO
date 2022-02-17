import React, { useEffect, useState } from "react";
import functionPlot from "function-plot";

interface FunctionProperties{
    function: string
    solution: string
}

const FunctionPlot: React.FC<FunctionProperties> = (props) => {

    const [y, setY] = useState(1);
    let contentsBounds = document.body.getBoundingClientRect();
    

    let width = contentsBounds.width * 0.3
    let height = width

    functionPlot({
        target: "#root",
        width ,
        height,
        yAxis: { domain: [-10, 10] },
        xAxis: { domain: [-10, 10] },
        grid: true,
        tip: {
            xLine: true,    // dashed line parallel to y = 0
            yLine: true,    // dashed line parallel to x = 0
            renderer: function (x, y, index) {
                if (props.solution === ""){
                  const y1 = functionPlot.$eval.builtIn({fn: props.function}, 'fn', {x: x})
                  setY(y1)
                }
              return ''
            }
          },
        data: [
        {
            fn: props.function

        }]
        
    })
    if (props.solution !== ""){
      var y1 = functionPlot.$eval.builtIn({fn: props.function}, 'fn', {x: +props.solution})
      if (y1 === 0){
        alert('correct solution')
        y1 = 1
      }
    }

    function useSolutionChecker(y: number) {
      useEffect(() => {
    
          function handleClick(event: { target: any; }) {
            if (-0.1 < y && y < 0.1){
              alert('correct '+ y)
              setY(1)
            }
          }
    
          // Bind the event listener
          document.addEventListener("click", handleClick);
          return () => {
              document.removeEventListener("click", handleClick);
          };
      });
    }    
    
    useSolutionChecker(y)

    
    return(
        <div>
        </div>
    );
};

export default FunctionPlot;
