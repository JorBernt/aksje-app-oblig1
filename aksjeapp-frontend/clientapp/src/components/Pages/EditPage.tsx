import React from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import InputField from "../UI/Input/InputField";
import Button from "../UI/Input/Button";

interface UserData {
    name: string;
    email: string;
    phone: string;

    password: string;
}

const EditPage = () => {
    const nameRef = React.createRef<HTMLInputElement>()
    const emailRef = React.createRef<HTMLInputElement>()
    const phoneRef = React.createRef<HTMLInputElement>()
    const passwordRef = React.createRef<HTMLInputElement>()
    const handleOnClick = () => {
        const userData: UserData = {
            name: String(nameRef.current?.value),
            email: String(emailRef.current?.value),
            phone: String(phoneRef.current?.value),
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
                        <h1 className={"text-center text-2xl mb-4"}>Edit</h1>
                        <InputField type={"text"} label={"Name"} ref={nameRef}/>
                        <InputField type={"text"} label={"Email"} ref={emailRef}/>
                        <InputField type={"text"} label={"Phone"} ref={phoneRef}/>
                        <InputField type={"password"} label={"Password"} ref={passwordRef}/>

                        <div className="mt-4">
                            <Button text={"Save"} onClick={handleOnClick}/>
                        </div>
                    </div>
                </Card>
            </div>
        </>
    )
}
export default EditPage;