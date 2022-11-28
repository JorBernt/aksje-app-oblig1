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
                    className=" z-0 p-5 mx-5 group-hover:bg-gradient-to-tl group-hover:from-gradient-start group-hover:to-gradient-end group-hover:scale-105 group-hover:text-white group-hover:rounded-xl rounded-xl  w-40 transition duration-300 ease-in-out "
                    onClick={() => handleOnClick(props.links[0])}
                >
                    <div className="flex w-fill justify-center gap-2">
                        <p className="text-xl text-center ">
                            {props.texts[0]}
                        </p>
                        <svg
                            className="svg-icon w-6 group-hover:rotate-180 transition-all easy-in-out hover:delay-200 duration-300 group-hover:fill-white"
                            viewBox="0 0 20 20">
                            <path
                                d="M13.962,8.885l-3.736,3.739c-0.086,0.086-0.201,0.13-0.314,0.13S9.686,12.71,9.6,12.624l-3.562-3.56C5.863,8.892,5.863,8.611,6.036,8.438c0.175-0.173,0.454-0.173,0.626,0l3.25,3.247l3.426-3.424c0.173-0.172,0.451-0.172,0.624,0C14.137,8.434,14.137,8.712,13.962,8.885 M18.406,10c0,4.644-3.763,8.406-8.406,8.406S1.594,14.644,1.594,10S5.356,1.594,10,1.594S18.406,5.356,18.406,10 M17.521,10c0-4.148-3.373-7.521-7.521-7.521c-4.148,0-7.521,3.374-7.521,7.521c0,4.147,3.374,7.521,7.521,7.521C14.148,17.521,17.521,14.147,17.521,10"></path>
                        </svg>
                    </div>
                </button>
                <div>
                    <button
                        className={`z-20 group-hover:delay-200 absolute opacity-0 group/2 group-hover:opacity-100 pt-0 group-hover:pt-6 z-0 transition-all  bg-gradient-to-t from-gray-${props.links.length > 2 ? '200' : '300'} hover:to-transparent to-transparent ease-in-out p-5 mx-5  hover:text-white hover:rounded-xl rounded-xl  w-40 transition duration-300 ease-in-out ` + (`hover:from-${props.color}-200 hover:shadow-${props.color}-200`)}
                        onClick={() => handleOnClick(props.links[1])}>
                        <p className="group-hover/2:text-green-400 opacity-0 group-hover:opacity-100 group-hover:delay-200 group-hover/2:scale-110 text-xl text-center text-black transition-all ease-in-out">
                            {props.texts[1]}
                        </p>
                    </button>
                </div>

                {props.texts.length > 2 &&
                    <div>
                        <button
                            className={"z-10 absolute group-hover:delay-200 opacity-0 group-hover:opacity-100 pt-0 group/2 group-hover:pt-[5.2rem] z-0 transition-all  bg-gradient-to-t from-gray-300 hover:to-transparent to-transparent ease-in-out p-5 mx-5  hover:text-white hover:rounded-xl rounded-xl shadow-xl w-40 transition duration-300 ease-in-out " + (`hover:from-${props.color}-200 hover:shadow-${props.color}-200`)}
                            onClick={() => handleOnClick(props.links[2])}>
                            <div className={"w-fill flex justify-center"}>
                                <div
                                    className={"opacity-0 group-hover:opacity-100 w-1 group-hover:w-24 h-[0.05rem] bg-black -mt-4 transition-all ease-in-out group-hover:delay-500 duration-500"}></div>
                            </div>
                            <p className="group-hover/2:text-red-500 opacity-0 group-hover:opacity-100 group-hover:delay-400 group-hover/2:scale-110 text-xl text-center text-black transition ease-in-out">
                                {props.texts[2]}
                            </p>
                        </button>
                    </div>
                }
            </div>
        </>
    )
}


export default NavbarLinkDropDown;