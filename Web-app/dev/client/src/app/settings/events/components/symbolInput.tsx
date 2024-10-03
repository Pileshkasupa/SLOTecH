import React, { ChangeEvent, useState } from "react";

const SymbolInput = (props: { onChange: (e: ChangeEvent<HTMLInputElement>) => void }) => {
    const [value, setValue] = useState("");

    const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
        const newValue = event.target.value;
        const allowedChars = ["<", ">", "="];

        // Check if the new input is one of the allowed characters and if it's a single character
        if (allowedChars.includes(newValue) && newValue.length === 1) {
            setValue(newValue);
        } else if (newValue.length === 0) {
            // Allow clearing the input
            setValue("");
        }
        props.onChange(event);
    };

    return (
        <input
            type="text"
            value={value}
            onChange={(e) => {
                handleChange(e);
            }}
            placeholder="<"
            className="border border-gray-300 rounded text-center text-base w-10 ml-3 mr-3"
        />
    );
};

export default SymbolInput;
