import React, {useEffect, useState} from 'react';
import {useNavigate} from 'react-router-dom'
import {API} from "../../Constants";
import {useLoggedInContext} from "../../App";

type Props = {
    links: string[];
    texts: string[];
}

const NavbarLinkDropDown: React.FC<Props> = (props) => {

    const [titles, setTitles] = useState<string[]>([""])
    useEffect(() => setTitles(props.texts), [props.texts])
    const loggedInContext = useLoggedInContext()
    let navigate = useNavigate();
    const handleOnClick = (link: string) => {
        if (link === "/logout") {
            API.CLIENT.LOGOUT()
                .then(response => {
                    if (response)
                        loggedInContext.setLoggedIn(false)
                    navigate("/")
                })
        }
        navigate(link)
    }

    return (
        <>
            <div className={"group"}>
                <button
                    className=" z-20 p-5 mx-5 group-hover:bg-gradient-to-tl group-hover:from-gradient-start group-hover:to-gradient-end group-hover:scale-105 group-hover:text-white group-hover:rounded-xl rounded-xl  w-40 transition duration-300 ease-in-out "
                    onClick={() => handleOnClick(props.links[0])}
                >
                    <p className="text-xl text-center ">{props.texts[0]}</p>
                </button>
                <div>
                    <button
                        className={"absolute opacity-0 group-hover:opacity-100 pt-0 group-hover:pt-6 z-0 transition-all  bg-gradient-to-t hover:from-red-200 from-gray-300 hover:to-transparent to-transparent ease-in-out delay-150  p-5 mx-5  hover:text-white hover:rounded-xl rounded-xl hover:shadow-xl hover:shadow-red-200 w-40 transition duration-300 ease-in-out"}
                        onClick={() => handleOnClick(props.links[1])}>
                        <p className="text-xl text-center text-black">{props.texts[1]}</p>
                    </button>
                </div>
            </div>
        </>
    )
}


export default NavbarLinkDropDown;