import React from 'react';
import NavbarLink from "./NavbarLink";
import SearchBar from "../UI/SearchBar/SearchBar";
import Divider from "./Divider";

function Navbar() {
    return (
        <>
            <nav className="flex items-center justify-between bg-gray-500 p-6">
                <a href="/" className="text-5xl">
                    Aksjehandel
                </a>
                <SearchBar/>
                <div className="flex items-center justify-between">
                    <NavbarLink link={"/"} text={"Hjem"}/>
                    <Divider/>
                    <NavbarLink link={"/stocks"} text={"Aksjer"}/>
                    <Divider/>
                    <NavbarLink link={"/profile"} text={"Profil"}/>
                </div>
            </nav>
        </>
    );
}

export default Navbar;