import React from 'react';
import NavbarLink from "./NavbarLink";
import SearchBar from "../UI/SearchBar/SearchBar";
import Divider from "./Divider";
import {LoggedInContextType, useLoggedInContext} from "../../App";
import NavbarLinkDropDown from "./NavbarLinkDropdown";

const Navbar = () => {
    const loggedInContext = useLoggedInContext() as LoggedInContextType
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
                        loggedInContext.loggedIn ?
                            <NavbarLinkDropDown links={["/profile", "/edit", "/logout"]}
                                                texts={["Profile", "Edit", "Log Out"]}
                                                color={"red"}/>
                            :
                            <NavbarLinkDropDown links={["/login", "/register"]} texts={["Log In", "Register"]}
                                                color={"blue"}/>
                    }
                </div>
            </nav>
        </>
    );
}

export default Navbar;