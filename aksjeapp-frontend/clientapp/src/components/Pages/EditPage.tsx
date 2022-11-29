import React, {useEffect, useState} from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import InputField from "../UI/Input/InputField";
import Button from "../UI/Input/Button";
import {User, UserData, UserDataSubmit} from "../models";
import {API} from "../../Constants";
import {useNavigate} from "react-router-dom";
import {useLoggedInContext} from "../../App";

const userFields: { [key: string]: boolean } = {};

const EditPage = () => {
    const navigate = useNavigate();
    const firstNameRef = React.createRef<HTMLInputElement>()
    const lastNameRef = React.createRef<HTMLInputElement>()
    const ssnRef = React.createRef<HTMLInputElement>()
    const addressRef = React.createRef<HTMLInputElement>()
    const pCodeRef = React.createRef<HTMLInputElement>()
    const pAddressRef = React.createRef<HTMLInputElement>()
    const passwordRef = React.createRef<HTMLInputElement>()
    const retypePasswordRef = React.createRef<HTMLInputElement>()

    const [customerData, setCustomerData] = useState<UserData>();
    const [matchingPasswords, setMatchingPasswords] = useState(true)
    const [passwordChanged, setPasswordChange] = useState(false)
    const [passwordChangeResponse, setPasswordChangeResponse] = useState("")
    const [editMessage, setEditMessage] = useState("")
    const [showEditMessage, setShowEditMessage] = useState(false)
    const [editMessageColor, setEditMessageColor] = useState("")
    const [passworChangeColor, setPasswordChangeColor] = useState("")
    const [password, setPassword] = useState("")

    const logInContext = useLoggedInContext()

    useEffect(() => {
        fetch(API.STOCK.GET_CUSTOMER_DATA, {credentials: 'include',})
            .then(res => res.json()
                .then(res => {
                    setCustomerData(res)
                    console.log(res)
                }).catch(e => {
                    console.log(e.message)
                })
            )
    }, [])

    const handleSavePasswordOnClick = () => {
        setPasswordChangeResponse("")
        setPasswordChange(false)
        setShowEditMessage(false)
        setEditMessage("")
        let password = String(passwordRef.current?.value)
        let reTypedPassword = String(passwordRef.current?.value)
        if (password)
            if (password !== reTypedPassword) {
                setMatchingPasswords(false)
                return;
            }
        setMatchingPasswords(true)
        const regExPaswd = /(?=.*[a-zA-ZæøåÆØÅ])(?=.*\d)[a-zA-ZæøåÆØÅ\d]{8,}/;
        if (!regExPaswd.test(password)) {
            setPasswordChange(true)
            setPasswordChangeResponse("Not a valid password!")
            setPasswordChangeColor("text-red-500")
            return;
        }
        const user: User = {
            username: "",
            password: password
        }
        API.CLIENT.CHANGE_PASSWORD(user)
            .then(response => {
                if (response) {
                    setPasswordChangeResponse("Password has been changed successfully!")
                    setPasswordChangeColor("text-green-400")
                    setPasswordChange(true)
                } else {
                    setPasswordChange(true)
                    setPasswordChangeColor("text-red-500")
                    setPasswordChangeResponse("Something went wrong.")

                }
            })
    }

    const handleOnClick = () => {
        const userData: UserDataSubmit = {
            FirstName: String(firstNameRef.current?.value),
            LastName: String(lastNameRef.current?.value),
            SocialSecurityNumber: String(ssnRef.current?.value),
            Address: String(addressRef.current?.value),
            PostalCode: String(pCodeRef.current?.value),
            PostCity: String(pAddressRef.current?.value)
        }

        let ok = true
        if (!userFields["First Name"] ||
            !userFields["Last Name"] ||
            !userFields["SSN"] ||
            !userFields["Address"] ||
            !userFields["Postal Code"] ||
            !userFields["City"]) {
            setEditMessage("Some field(s) are not valid!")
            setEditMessageColor("text-red-500")
            setShowEditMessage(true)
            ok = false;
        }

        if (userData.FirstName === "" ||
            userData.LastName === "" ||
            userData.SocialSecurityNumber === "" ||
            userData.Address === "" ||
            userData.PostalCode === "" ||
            userData.PostCity === "") {
            setEditMessage("No empty fields!")
            setEditMessageColor("text-red-500")
            setShowEditMessage(true)
            ok = false
        }
        if (!ok)
            return;

        API.CLIENT.UPDATE_CUSTOMER(userData).then(response => {
            if (response) {
                setEditMessage("Changes saved!")
                setEditMessageColor("text-green-400")
                setShowEditMessage(true)
            } else {
                setEditMessage("Something went wrong!")
                setEditMessageColor("text-red-500")
                setShowEditMessage(true)
            }
        })
    }
    return (
        <>
            <Navbar/>
            {!logInContext.loggedIn ?
                <>
                    <div className={"w-screen flex flex-col gap-4 justify-center items-center mt-96"}>
                        <p className={"text-2xl "}>You are not logged in</p>
                        <Button text={"Log in"} onClick={() => navigate("/login")}/>
                    </div>
                </>
                :
                <div className="flex justify-center mt-24">
                    <div className="w-fit">
                        <Card>
                            <div className="flex flex-col h-fit items-center">
                                <h1 className={"text-center text-2xl mb-4"}>Edit account</h1>
                                <div className="grid grid-cols-2">
                                    <InputField type={"text"} label={"First Name"} ref={firstNameRef}
                                                initVal={customerData?.firstName} field={userFields}/>
                                    <InputField type={"text"} label={"Last Name"} ref={lastNameRef}
                                                initVal={customerData?.lastName} field={userFields}/>
                                    <InputField type={"text"} label={"SSN"} ref={ssnRef}
                                                initVal={customerData?.socialSecurityNumber} field={userFields}/>
                                    <InputField type={"text"} label={"Address"} ref={addressRef}
                                                initVal={customerData?.address} field={userFields}/>
                                    <InputField type={"text"} label={"Postal Code"} ref={pCodeRef}
                                                initVal={customerData?.postalCode} field={userFields}/>
                                    <InputField type={"text"} label={"City"} ref={pAddressRef}
                                                initVal={customerData?.postCity} field={userFields}/>

                                </div>
                                {showEditMessage &&
                                    <p className={editMessageColor}>{editMessage}</p>
                                }
                                <div className="flex flex-row gap-3">
                                    <div className="mt-4">
                                        <Button text={"Save"} onClick={handleOnClick}/>
                                    </div>
                                    <div className="mt-4">

                                    </div>
                                </div>
                            </div>
                        </Card>
                        <Card>
                            <div className="flex justify-center gap-4 flex-col">
                                <p className="text-2xl text-center">Change password</p>
                                <div className="flex flex-row">
                                    <InputField type={"password"} label={"Password"} ref={passwordRef}
                                                initVal={customerData?.password} setPassword={setPassword}/>
                                    <InputField type={"password"} label={"Retype Password"} ref={retypePasswordRef}
                                                initVal={customerData?.password} password={password}/>

                                </div>
                                <div className="flex justify-center">
                                    {!matchingPasswords &&
                                        <p className={"text-sm text-red-500"}>Passwords is not matching</p>
                                    }
                                    {passwordChanged &&
                                        <p className={`${passworChangeColor} text-sm`}>{passwordChangeResponse}</p>
                                    }
                                </div>
                                <div className="flex justify-center">
                                    <Button text={"Save"} onClick={handleSavePasswordOnClick}/>
                                </div>
                            </div>
                        </Card>
                        <div className="flex justify-center mt-12">
                            <button
                                className="rounded-2xl w-fit bg-red-500 text-white font-semibold py-2 px-4 hover:shadow-red-500 hover:shadow-xl transition-all duration-300 ease-in-out hover:scale-105"
                                onClick={handleOnClick}>
                                Delete Account
                            </button>
                        </div>
                    </div>
                </div>
            }
        </>
    )
}
export default EditPage;