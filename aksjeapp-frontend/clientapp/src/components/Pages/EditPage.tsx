import React from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import InputField from "../UI/Input/InputField";
import Button from "../UI/Input/Button";
import {UserData} from "../models";

const EditPage = () => {
    const firstNameRef = React.createRef<HTMLInputElement>()
    const lastNameRef = React.createRef<HTMLInputElement>()
    const ssnRef = React.createRef<HTMLInputElement>()
    const passwordRef = React.createRef<HTMLInputElement>()
    const handleOnClick = () => {
        const userData: UserData = {
            firstname: String(firstNameRef.current?.value),
            lastname: String(lastNameRef.current?.value),
            ssn: String(ssnRef.current?.value),
            password: String(passwordRef.current?.value)
        }
        console.log(userData)
        console.log(userData)
    }
    return (
        <>
            <Navbar/>
            <div className="flex justify-center ">
                <Card>
                    <div className="flex flex-col h-fit items-center">
                        <h1 className={"text-center text-2xl mb-4"}>Edit</h1>
                        <InputField type={"text"} label={"First Name"} ref={firstNameRef}/>
                        <InputField type={"text"} label={"Last Name"} ref={lastNameRef}/>
                        <InputField type={"text"} label={"SSN"} ref={ssnRef}/>
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