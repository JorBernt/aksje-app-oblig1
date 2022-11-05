import React from "react";

type Props = {
    type: string,
    label: string
}

const InputField: React.FC<Props> = (props) => {
    return (
        <>
            <div className="flex flex-col justify-between my-2 text-l w-72">
                <p>{props.label}</p>
                <input type={props.type} className="bg-transparent border focus:border-pink-500 rounded-2xl  pl-4 py-2"
                       style={{outline: "none"}}/>
            </div>
        </>
    )
}

export default InputField;