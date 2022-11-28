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
    const addressRef = React.createRef<HTMLInputElement>()
    const pCodeRef = React.createRef<HTMLInputElement>()
    const pAddressRef = React.createRef<HTMLInputElement>()
    const passwordRef = React.createRef<HTMLInputElement>()
    const handleOnClick = () => {
        const userData: UserData = {
            firstname: String(firstNameRef.current?.value),
            lastname: String(lastNameRef.current?.value),
            socialsecuritynumber: String(ssnRef.current?.value),
            address: String(addressRef.current?.value),
            postalcode: Number(pCodeRef.current?.value),
            postcity: String(pAddressRef.current?.value),
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
                        <InputField type={"text"} label={"First Name"} ref={firstNameRef}/>
                        <InputField type={"text"} label={"Last Name"} ref={lastNameRef}/>
                        <InputField type={"text"} label={"SSN"} ref={ssnRef}/>
                        <InputField type={"text"} label={"Address"} ref={addressRef}/>
                        <InputField type={"number"} label={"Postal Code"} ref={pCodeRef}/>
                        <InputField type={"text"} label={"Postal Address"} ref={pAddressRef}/>
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