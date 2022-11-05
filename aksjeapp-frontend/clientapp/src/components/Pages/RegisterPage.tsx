import React from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import InputField from "../UI/Input/InputField";
import Button from "../UI/Input/Button";

interface UserData {
    name: string;
    email: string;
    phone: string;
    ssn: string;
    password: string;
}


const LoginPage = () => {
    const nameRef = React.createRef<HTMLInputElement>()
    const emailRef = React.createRef<HTMLInputElement>()
    const phoneRef = React.createRef<HTMLInputElement>()
    const ssnRef = React.createRef<HTMLInputElement>()
    const passwordRef = React.createRef<HTMLInputElement>()
    const retypedPasswordRef = React.createRef<HTMLInputElement>()
    const handleOnClick = () => {
        const userData: UserData = {
            name: String(nameRef.current?.value),
            email: String(emailRef.current?.value),
            phone: String(phoneRef.current?.value),
            ssn: String(ssnRef.current?.value),
            password: String(passwordRef.current?.value)
        }
        console.log(userData)
    }
    return (
        <>
            <Navbar/>
            <div className="flex justify-center ">
                <Card>
                    <div className="flex flex-col h-fit items-center">
                        <h1 className={"text-center text-2xl mb-4"}>Register</h1>
                        <InputField type={"text"} label={"Name"} ref={nameRef}/>
                        <InputField type={"text"} label={"Email"} ref={emailRef}/>
                        <InputField type={"text"} label={"Phone"} ref={phoneRef}/>
                        <InputField type={"text"} label={"SSN"} ref={ssnRef}/>
                        <InputField type={"password"} label={"Password"} ref={passwordRef}/>
                        <InputField type={"password"} label={"Retype password"} ref={retypedPasswordRef}/>
                        <div className="mt-4">
                            <Button text={"Create account"} onClick={handleOnClick}/>
                        </div>
                    </div>
                </Card>
            </div>
        </>
    )
}

export default LoginPage;