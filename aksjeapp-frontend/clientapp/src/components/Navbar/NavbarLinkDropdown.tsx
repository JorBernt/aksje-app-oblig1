import React from 'react';
import {useNavigate} from 'react-router-dom'
import {API} from "../../Constants";
import {useLoggedInContext} from "../../App";

type Props = {
    links: string[];
    texts: string[];
    color: string
}

const NavbarLinkDropDown: React.FC<Props> = (props) => {

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
                        className={`z-10 absolute opacity-0 group/2 group-hover:opacity-100 pt-0 group-hover:pt-6 z-0 transition-all  bg-gradient-to-t from-gray-${props.links.length > 2 ? '100' : '300'} hover:to-transparent to-transparent ease-in-out p-5 mx-5  hover:text-white hover:rounded-xl rounded-xl hover:shadow-xl w-40 transition duration-300 ease-in-out ` + (`hover:from-${props.color}-200 hover:shadow-${props.color}-200`)}
                        onClick={() => handleOnClick(props.links[1])}>
                        <p className="group-hover/2:scale-110 text-xl text-center text-black transition ease-in-out">{props.texts[1]}</p>
                    </button>
                </div>
                {props.texts.length > 2 &&
                    <div>
                        <button
                            className={"absolute opacity-0 group-hover:opacity-100 pt-0 group/2 group-hover:pt-[5.2rem] z-0 transition-all  bg-gradient-to-t from-gray-300 hover:to-transparent to-transparent ease-in-out p-5 mx-5  hover:text-white hover:rounded-xl rounded-xl hover:shadow-xl w-40 transition duration-300 ease-in-out " + (`hover:from-${props.color}-200 hover:shadow-${props.color}-200`)}
                            onClick={() => handleOnClick(props.links[2])}>
                            <p className="group-hover/2:scale-110 text-xl text-center text-black transition ease-in-out">{props.texts[2]}</p>
                        </button>
                    </div>
                }
            </div>
        </>
    )
}


export default NavbarLinkDropDown;