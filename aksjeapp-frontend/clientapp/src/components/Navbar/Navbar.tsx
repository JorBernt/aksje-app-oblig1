import React, {useState} from 'react';
import NavbarLink from "./NavbarLink";
import SearchBar from "../UI/SearchBar/SearchBar";
import Divider from "./Divider";
import {API} from "../../Constants";

function Navbar() {
    const [loggedIn, setLoggedIn] = useState(false)
    API.CLIENT.AUTHENTICATE_USER().then(response => {
        if (response)
            setLoggedIn(true)
    })
    return (
        <>
            <nav className="flex items-center justify-between bg-navbar border-b-2 border-black p-1">
                <a href="/" className="text-2xl ml-10">
                    Stock Trading
                </a>
                <div className="ml-[29rem]">
                    <SearchBar/>
                </div>
                <div className="flex items-center justify-between">
                    <NavbarLink link={"/"} text={"Home"}/>
                    <Divider/>
                    <NavbarLink link={"/stocks"} text={"Stocks"}/>
                    <Divider/>
                    {
                        loggedIn ?
                            <NavbarLink link={"/profile"} text={"Profile"}/>
                            :
                            <NavbarLink link={"/login"} text={"Log in"}/>
                    }
                </div>
            </nav>
        </>
    );
}

export default Navbar;