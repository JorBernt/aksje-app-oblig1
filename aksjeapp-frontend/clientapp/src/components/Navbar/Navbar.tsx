import React from 'react';
import NavbarLink from "./NavbarLink";
import SearchBar from "../UI/SearchBar/SearchBar";

function Navbar() {
    return (
        <>
            <nav className="flex items-center justify-between bg-gray-500 p-6">
                <a href="/" className="text-5xl">
                    Aksjehandel
                </a>
                <SearchBar/>
                <div className="flex items-center justify-between">
                    <NavbarLink link={"/aksjer"}>
                        <p>Aksjer</p>
                    </NavbarLink>
                    <NavbarLink link={"/profile"}>
                        <p>Profil</p>
                    </NavbarLink>
                </div>
            </nav>
        </>
    );
}

export default Navbar;