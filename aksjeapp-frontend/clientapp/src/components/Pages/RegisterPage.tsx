import React from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import InputField from "../UI/Input/InputField";
import Button from "../UI/Input/Button";
import {UserData} from "../models";
import {API} from "../../Constants";
import {useNavigate} from "react-router-dom";


const LoginPage = () => {
    const navigate = useNavigate();
    const firstNameRef = React.createRef<HTMLInputElement>()
    const lastNameRef = React.createRef<HTMLInputElement>()
    const ssnRef = React.createRef<HTMLInputElement>()
    const addressRef = React.createRef<HTMLInputElement>()
    const pCodeRef = React.createRef<HTMLInputElement>()
    const pAddressRef = React.createRef<HTMLInputElement>()
    const passwordRef = React.createRef<HTMLInputElement>()
    const retypedPasswordRef = React.createRef<HTMLInputElement>()
    const handleOnClick = () => {
        if (String(passwordRef.current?.value) !== String(retypedPasswordRef.current?.value)) {
            console.log("Passwords don't match!")
            return;
        }

        const userData: UserData = {
            firstName: String(firstNameRef.current?.value),
            lastName: String(lastNameRef.current?.value),
            socialSecurityNumber: String(ssnRef.current?.value),
            address: String(addressRef.current?.value),
            postalCode: Number(pCodeRef.current?.value),
            postCity: String(pAddressRef.current?.value),
            password: String(passwordRef.current?.value)
        }

        if (userData.firstName === "" ||
            userData.lastName === "" ||
            userData.socialSecurityNumber === "" ||
            userData.address === "" ||
            userData.postalCode === 0 ||
            userData.postCity === "" ||
            userData.password === "") {
            return;
        }

        API.CLIENT.REGISTER_CUSTOMER(userData).then(response => {
            if (response) {
                navigate("/login")
            }
        })
    }
    return (
        <>
            <Navbar/>
            <div className="flex justify-center ">
                <Card>
                    <div className="flex flex-col h-fit items-center">
                        <h1 className={"text-center text-2xl mb-4"}>Register</h1>
                        <div className="grid grid-cols-2">
                            <InputField type={"text"} label={"First Name"} ref={firstNameRef}/>
                            <InputField type={"text"} label={"Last Name"} ref={lastNameRef}/>
                            <InputField type={"text"} label={"SSN"} ref={ssnRef}/>
                            <InputField type={"text"} label={"Address"} ref={addressRef}/>
                            <InputField type={"number"} label={"Postal Code"} ref={pCodeRef}/>
                            <InputField type={"text"} label={"Postal Address"} ref={pAddressRef}/>
                            <InputField type={"password"} label={"Password"} ref={passwordRef}/>
                            <InputField type={"password"} label={"Retype password"} ref={retypedPasswordRef}/>
                        </div>
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