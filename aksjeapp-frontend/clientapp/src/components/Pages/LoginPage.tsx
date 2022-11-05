import React from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import InputField from "../UI/Input/InputField";

const LoginPage = () => {
    return (
        <>
            <Navbar/>
            <div className="flex justify-center mt-32">
                <Card>
                    <div className="flex flex-col h-fit items-center">
                        <h1 className={"text-center text-2xl mb-4"}>Login</h1>
                        <InputField type={"text"} label={"Email"}/>
                        <InputField type={"password"} label={"Password"}/>
                        <button
                            className="mt-4 bg-transparent rounded-2xl w-fit bg-gray-300 hover:bg-gradient-to-tl hover:from-gradient-start hover:to-gradient-end text-black font-semibold py-2 px-4 hover:text-white transition duration-300 ease-in-out">
                            Log in
                        </button>
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