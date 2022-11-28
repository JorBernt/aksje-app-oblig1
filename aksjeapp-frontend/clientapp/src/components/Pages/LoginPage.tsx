import React, {useState} from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import InputField from "../UI/Input/InputField";
import Button from "../UI/Input/Button";
import {useNavigate} from "react-router-dom";
import {User} from "../models";
import {API} from "../../Constants";
import {useLoggedInContext} from "../../App";

const LoginPage = () => {
    const navigate = useNavigate();
    const usernameRef = React.createRef<HTMLInputElement>()
    const passwordRef = React.createRef<HTMLInputElement>()
    const loggedInContext = useLoggedInContext()
    const [wrongLogin, setWrongLogin] = useState(false)
    const handleOnClick = () => {
        if (!usernameRef.current || !passwordRef.current)
            return
        const user: User = {
            username: usernameRef.current.value,
            password: passwordRef.current.value
        };
        API.CLIENT.LOGIN(user).then(response => {
            if (response) {
                navigate("/profile")
                loggedInContext.setLoggedIn(true)
            } else {
                setWrongLogin(true)
            }
        })
    }
    const handleNavigateClick = () => {
        navigate("/register")
    }

    const handleOnKeyDown = (key: string) => {
        if (key === "Enter")
            handleOnClick()
    }

    return (
        <>
            <Navbar/>
            <div className="flex justify-center mt-32">
                <Card>
                    <div className="flex flex-col h-fit items-center w-72">
                        <h1 className={"text-center text-2xl mb-4"}>Login</h1>
                        <InputField type={"text"} label={"SSN"} ref={usernameRef} onKeyDown={handleOnKeyDown}/>
                        <InputField type={"password"} label={"Password"} ref={passwordRef} onKeyDown={handleOnKeyDown}
                                    validate={false}/>
                        {wrongLogin &&
                            <p className="text-red-500">Wrong username or password</p>
                        }
                        <div className="mt-4">
                            <Button text={"Log in"} onClick={handleOnClick}/>
                        </div>
                        <div className="mt-4 flex flex-col justify-center items-center">
                            <p>Don't have an account?</p>
                            <a href={"/register"} onClick={handleNavigateClick}
                               className={"underline decoration-inherit text-blue-500"}>Register here</a>
                        </div>
                    </div>
                </Card>
            </div>
        </>
    )
}

export default LoginPage;