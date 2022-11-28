import React, {useEffect, useState} from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import InputField from "../UI/Input/InputField";
import Button from "../UI/Input/Button";
import {UserData} from "../models";
import {API} from "../../Constants";
import {useNavigate} from "react-router-dom";
import {useLoggedInContext} from "../../App";

const EditPage = () => {
    const navigate = useNavigate();
    const firstNameRef = React.createRef<HTMLInputElement>()
    const lastNameRef = React.createRef<HTMLInputElement>()
    const ssnRef = React.createRef<HTMLInputElement>()
    const addressRef = React.createRef<HTMLInputElement>()
    const pCodeRef = React.createRef<HTMLInputElement>()
    const pAddressRef = React.createRef<HTMLInputElement>()
    const passwordRef = React.createRef<HTMLInputElement>()

    const [customerData, setCustomerData] = useState<UserData>();

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

    const handleOnClick = () => {
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

        API.CLIENT.UPDATE_CUSTOMER(userData).then(response => {
            if (response) {
                navigate("/profile")
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
                <div className="flex justify-center ">
                    <Card>
                        <div className="flex flex-col h-fit items-center">
                            <h1 className={"text-center text-2xl mb-4"}>Edit account</h1>
                            <div className="grid grid-cols-2">
                                <InputField type={"text"} label={"First Name"} ref={firstNameRef}
                                            initVal={customerData?.firstName}/>
                                <InputField type={"text"} label={"Last Name"} ref={lastNameRef}
                                            initVal={customerData?.lastName}/>
                                <InputField type={"text"} label={"SSN"} ref={ssnRef}
                                            initVal={customerData?.socialSecurityNumber}/>
                                <InputField type={"text"} label={"Address"} ref={addressRef}
                                            initVal={customerData?.address}/>
                                <InputField type={"text"} label={"Postal Code"} ref={pCodeRef}
                                            initVal={customerData?.postalCode}/>
                                <InputField type={"text"} label={"Postal Address"} ref={pAddressRef}
                                            initVal={customerData?.postCity}/>
                                <InputField type={"password"} label={"Password"} ref={passwordRef}
                                            initVal={customerData?.password}/>
                            </div>
                            <div className="mt-4">
                                <Button text={"Save"} onClick={handleOnClick}/>
                            </div>
                        </div>
                    </Card>
                </div>
            }
        </>
    )
}
export default EditPage;