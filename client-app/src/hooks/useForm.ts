import React, { useState } from "react";

type FormChangeCallback = (event: React.ChangeEvent<{ name: string, value: unknown }>) => void;
type FormSubmitCallback = (event: React.FormEvent<HTMLFormElement>) => Promise<void>;

const useForm = function<StateType>(callback: (state: StateType) => Promise<void>, initialState: StateType): [StateType, FormChangeCallback, FormSubmitCallback, React.Dispatch<React.SetStateAction<StateType>>] {
    const [formState, setFormState] = useState(initialState);

    // onChange
    const onChange = (event: React.ChangeEvent<{ name: string, value: unknown }>) => {
        setFormState(formState => ({ ...formState, [event.target.name]: event.target.value }));
    };

    // onSubmit
    const onSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        await callback(formState); // triggering the callback
    };

    // return values
    return [
        formState,
        onChange,
        onSubmit,
        setFormState
    ];
}

// useForm hook
export default useForm;