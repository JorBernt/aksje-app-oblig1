import React from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import InputField from "../UI/Input/InputField";
import Button from "../UI/Input/Button";

const LoginPage = () => {
    const handleOnClick = () => {

    }
    return (
        <>
            <Navbar/>
            <div className="flex justify-center mt-32">
                <Card>
                    <div className="flex flex-col h-fit items-center">
                        <h1 className={"text-center text-2xl mb-4"}>Login</h1>
                        <InputField type={"text"} label={"Email"}/>
                        <InputField type={"password"} label={"Password"}/>
                        <div className="mt-4">
                            <Button text={"Log in"} onClick={handleOnClick}/>
                        </div>
                        <div className="mt-4 flex flex-col justify-center items-center">
                            <p>Don't have an account?</p>
                            <a href={"/"} className={"underline decoration-inherit text-blue-500"}>Register here</a>
                        </div>
                    </div>
                </Card>
            </div>
        </>
    )
}

export default LoginPage;