import React, {FC} from 'react';

type Props = {
    color: string;
    customCss?: string;
    children: React.ReactNode;
}

const Card : FC<Props> = (props ) => {
    //const className = props.customCss + " drop-shadow-2xl rounded-2xl p-5 m-5 bg-card";
    return (
        <>
            <div className="shadow-2xl rounded-2xl p-5 m-5 bg-card">
                {props.children}
            </div>
            <div className=""></div>
        </>
    )
}

export default Card;


