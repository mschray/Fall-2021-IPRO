import getAzureFunctions from 'getAzureFunctions';
import React, { useCallback, useEffect, useState } from 'react'

interface PlayerCountProps {
    courseId: number
}

export const PlayerCount: React.FC<PlayerCountProps> = (props) => {
    const [numberOfPlayers, setNumberOfPlayers] = useState(0);
    const EVENTS_INTERVAL = 2500;
    const getNumberOfPlayers = useCallback(
        () => {
            const url = new URL(getAzureFunctions().GetNumberOfPlayers);
            url.searchParams.append("course_id", props.courseId.toString());
            fetch(url.toString())
                .then(response => response.json())
                .then(json => {
                    setNumberOfPlayers(json.NumberOfPlayers);
                })
                .catch(err => {
                    console.log(err);
                });
        },
        [setNumberOfPlayers, props.courseId]
    );

    useEffect(
        () => {
            // Immediately fetch number of players
            getNumberOfPlayers();
            // Continue to fetch stage events + number of players every EVENTS_INTERVAL milliseconds
            console.log("starting ping interval for number of players!");
            const intervalNumberOfPlayers = setInterval(getNumberOfPlayers, EVENTS_INTERVAL);
            return () => {
                console.log("clearing ping interval for number of players!");
                clearInterval(intervalNumberOfPlayers);//Runs automatically as cleanup
            }
        },
        [getNumberOfPlayers]
    );
    
    return (
        <div>
            <h3>Player Count</h3>
            <h3>{numberOfPlayers}</h3>
        </div>
    )
}
