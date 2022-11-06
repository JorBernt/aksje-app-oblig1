import React from "react";

type Props = {
    text: string,
    onClick: any
}

const Button: React.FC<Props> = (props) => {
    return (
        <>
            <button
                className="bg-transparent rounded-2xl w-fit bg-gray-300 hover:bg-gradient-to-tl hover:from-gradient-start hover:to-gradient-end text-black font-semibold py-2 px-4 hover:text-white transition duration-300 ease-in-out"
                onClick={props.onClick}>
                {props.text}
            </button>
        </>
    )
}

export default Button;