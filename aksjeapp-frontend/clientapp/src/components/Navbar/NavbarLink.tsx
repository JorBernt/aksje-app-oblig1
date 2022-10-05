import React from 'react';

type Props = {
    link: string;
    text: string;
}

const NavbarLink : React.FC<Props> = (props) => {
    return (
        <>
            <button
                className="p-5 border border-gray-600 rounded mx-5 hover:bg-amber-500 hover:scale-105 w-40 transition duration-300 ease-in-out">
                <a href={props.link}>
                    <p className="text-xl text-center">{props.text}</p>
                </a>
            </button>
        </>
    )
}

export default NavbarLink;