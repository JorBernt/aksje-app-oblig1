import React from 'react';
import {useNavigate} from 'react-router-dom'

type Props = {
    link: string;
    text: string;
}

const NavbarLink: React.FC<Props> = (props) => {
    let navigate = useNavigate();
    const handleOnClick = () => {
        navigate(props.link)
    }
    return (
        <>
            <button
                className="p-5 mx-5 hover:bg-gradient-to-tl hover:from-gradient-start hover:to-gradient-end hover:scale-105 hover:text-white hover:rounded-xl rounded-xl hover:shadow-xl hover:shadow-green-300 w-40 transition duration-300 ease-in-out"
                onClick={handleOnClick}>
                <p className="text-xl text-center ">{props.text}</p>
            </button>
        </>
    )
}

export default NavbarLink;