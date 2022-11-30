import React, {useState} from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import InputField from "../UI/Input/InputField";
import Button from "../UI/Input/Button";
import {User, UserDataSubmit} from "../models";
import {API} from "../../Constants";
import {useNavigate} from "react-router-dom";

const userFields: { [key: string]: boolean } = {};

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


    const [password, setPassword] = useState("")
    const [editMessage, setEditMessage] = useState("")
    const [showEditMessage, setShowEditMessage] = useState(false)
    const [editMessageColor, setEditMessageColor] = useState("")

    const handleOnClick = () => {
        setShowEditMessage(false)


        const user: User = {
            username: "",
            password: String(passwordRef.current?.value)
        }

        const userData: UserDataSubmit = {
            FirstName: String(firstNameRef.current?.value),
            LastName: String(lastNameRef.current?.value),
            SocialSecurityNumber: String(ssnRef.current?.value),
            Address: String(addressRef.current?.value),
            PostalCode: String(pCodeRef.current?.value),
            PostCity: String(pAddressRef.current?.value),
            User: user
        }
        let ok = true
        if (!userFields["First Name"] ||
            !userFields["Last Name"] ||
            !userFields["SSN"] ||
            !userFields["Address"] ||
            !userFields["Postal Code"] ||
            !userFields["City"] ||
            !userFields["Password"] ||
            !userFields["Retype Password"]) {
            ok = false;
        }

        if (userData.FirstName === "" ||
            userData.LastName === "" ||
            userData.SocialSecurityNumber === "" ||
            userData.Address === "" ||
            userData.PostalCode === "" ||
            userData.PostCity === "" ||
            userData.User?.password === "") {
            setEditMessage("No empty fields!")
            setEditMessageColor("text-red-500")
            setShowEditMessage(true)
            ok = false
        }
        if (!ok)
            return;

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
                            <InputField type={"text"} label={"First Name"} ref={firstNameRef} field={userFields}/>
                            <InputField type={"text"} label={"Last Name"} ref={lastNameRef} field={userFields}/>
                            <InputField type={"text"} label={"SSN"} ref={ssnRef} field={userFields}/>
                            <InputField type={"text"} label={"Address"} ref={addressRef} field={userFields}/>
                            <InputField type={"text"} label={"Postal Code"} ref={pCodeRef} field={userFields}/>
                            <InputField type={"text"} label={"City"} ref={pAddressRef} field={userFields}/>
                            <InputField type={"password"} label={"Password"} ref={passwordRef}
                                        setPassword={setPassword} field={userFields}/>
                            <InputField type={"password"} label={"Retype Password"} ref={retypedPasswordRef}
                                        password={password} field={userFields}/>
                        </div>
                        {showEditMessage &&
                            <p className={editMessageColor}>{editMessage}</p>
                        }
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