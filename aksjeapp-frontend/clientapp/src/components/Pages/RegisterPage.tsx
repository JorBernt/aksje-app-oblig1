import React from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import InputField from "../UI/Input/InputField";
import Button from "../UI/Input/Button";
import {UserData} from "../models";


const LoginPage = () => {
    const firstNameRef = React.createRef<HTMLInputElement>()
    const lastNameRef = React.createRef<HTMLInputElement>()
    const ssnRef = React.createRef<HTMLInputElement>()
    const passwordRef = React.createRef<HTMLInputElement>()
    const retypedPasswordRef = React.createRef<HTMLInputElement>()
    const handleOnClick = () => {
        if (String(passwordRef.current?.value) !== String(retypedPasswordRef.current?.value)) {
            console.log("Passwords don't match!")
            return;
        }
        const userData: UserData = {
            firstname: String(firstNameRef.current?.value),
            lastname: String(lastNameRef.current?.value),
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
                        <InputField type={"text"} label={"First Name"} ref={firstNameRef}/>
                        <InputField type={"text"} label={"Last Name"} ref={lastNameRef}/>
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