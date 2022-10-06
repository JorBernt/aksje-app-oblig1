import React, {FC} from 'react';

type Props = {
    color: string;
    children : React.ReactNode;
}

const Card : FC<Props> = (props ) => {
    const className = "drop-shadow-md rounded-2xl p-5 m-5 " + (props.color === "default" ? "bg-gray-500" : props.color);
    return (
        <>
            <div className={className}>
                {props.children}
            </div>
        </>
    )
}

export default Card;


