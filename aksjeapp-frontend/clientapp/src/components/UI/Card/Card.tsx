import React, {FC} from 'react';

type Props = {
    color?: string;
    customCss?: string;
    children: React.ReactNode;
}

const Card : FC<Props> = (props ) => {
    return (
        <>
            <div
                className={props.customCss + " shadow-2xl rounded-2xl " + (props.color == null ? " bg-card " : props.color + " ") + (props.customCss == null ? "p-5 m-5" : props.customCss)}>
                {props.children}
            </div>
            <div className=""></div>
        </>
    )
}

export default Card;


