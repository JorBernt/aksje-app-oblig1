import React, {FC} from 'react';

type Props = {
    color: string;
    customCss?: string;
    children: React.ReactNode;
}

const Card : FC<Props> = (props ) => {
    const className = props.customCss + " drop-shadow-md  border-2 border-black rounded-2xl p-5 m-5 " + (props.color === "default" ? "bg-card" : props.color);
    return (
        <>
            <div className={className}>
                {props.children}
            </div>
            <div className=""></div>
        </>
    )
}

export default Card;


