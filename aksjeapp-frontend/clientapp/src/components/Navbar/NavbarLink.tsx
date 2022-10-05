import React from 'react';

type Props = {
    link : string;
    children : React.ReactNode;
}

const NavbarLink : React.FC<Props> = (props) => {
    return (
        <>
            <div className="p-5">
                <a href={props.link}>
                    {props.children}
                </a>
            </div>
        </>
    )
}

export default NavbarLink;