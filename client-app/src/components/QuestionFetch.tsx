import React from "react";

import getAzureFunctions from "getAzureFunctions";
import useFetch, { FetchStatus } from "hooks/useFetch";

import Question from "components/Question";
import { isQuestionModel } from "models/QuestionModel";

// Properties for the QuestionFetch React component
interface QuestionFetchProperties {
    id: number,
    onSolve: () => void
}

const QuestionFetch: React.FC<QuestionFetchProperties> = (props) => {
    // Fetch the question data using the GetQuestion function
    const url = new URL(getAzureFunctions().GetQuestion);
    url.searchParams.append("id", props.id.toString());
    const fetchResult = useFetch(
        url.toString(),
        (data) => {
            // The Azure function should return the data as an array with one QuestionModel object inside it
            if (Array.isArray(data) && isQuestionModel(data[0])) {
                return data[0];
            }
            return undefined;
        },
        [props.id]
    );

    if (fetchResult.status === FetchStatus.Success) {
        // Display question if fetching was successful
        return (
            <Question
                questionData={fetchResult.payload}
                onSolve={props.onSolve}
            />
        );
    } else if (fetchResult.status === FetchStatus.Failure) {
        // Notify user that the question couldn't be fetched
        return (
            <p>Could not fetch question! Reason: {fetchResult.reason}</p>
        )
    } else {
        // Notify user that the question is currently being fetched
        return (
            <p>Fetching question data...</p>
        )
    }
};

export default QuestionFetch;