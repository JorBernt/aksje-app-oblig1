import React from 'react';

type Props = {
    link: string;
    text: string;
}

const NavbarLink : React.FC<Props> = (props) => {
    return (
        <>
            <button
                className="p-5 mx-5 hover:bg-gradient-to-tl hover:from-gradient-start hover:to-gradient-end hover:scale-105 hover:text-white hover:rounded-xl rounded-xl hover:shadow-xl hover:shadow-green-300 w-40 transition duration-300 ease-in-out">
                <a href={props.link}>
                    <p className="text-xl text-center ">{props.text}</p>
                </a>
            </button>
        </>
    )
}

export default NavbarLink;