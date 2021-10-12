import React from "react";
import { setConstantValue } from "typescript";

const Home: React.FC = () => {
    const [data, setData] = React.useState("");
    let questionID = 84;
    React.useEffect(() => {      
        const fetchData = async () => {
            var request = "https://jebrafunctions.azurewebsites.net/api/GetQuestion?code=KoWO/aWjlpHbXBep0eePd39TA1uwcTXjYQxMIOaHDgrTwTKkBT3S2g==&id="+questionID;
            const response = await fetch(request);
            const json = await response.json();
            console.log(json);
            setData(json);
        }
        fetchData();
    }, [])

    return (
        <div>
            <h3>Home</h3>
            <p>Example API request: {JSON.stringify(data)}</p>
        </div>
    );
};

export default Home;