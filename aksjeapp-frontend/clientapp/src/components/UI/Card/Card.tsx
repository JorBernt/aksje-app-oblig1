import React, {FC} from 'react';

type Props = {
    color: string;
    children : React.ReactNode;
}

const Card : FC<Props> = (props ) => {
    const className = "box-shadow-md border-2 border-black rounded-2xl p-5 m-5 " + (props.color === "default" ? "bg-card" : props.color);
    return (
        <>
            <div className={className}>
                {props.children}
            </div>
        </>
    )
}

export default Card;


