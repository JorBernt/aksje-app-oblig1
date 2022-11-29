import DataDisplay from "../UI/TextDisplay/DataDisplay";
import React, {useState} from "react";
import {ProfileInfo} from "../models";
import Button from "../UI/Input/Button";
import {API} from "../../Constants";

type Props = {
    profileInfo?: ProfileInfo
    callback: any
}

const ProfileInfoContainer: React.FC<Props> = (props) => {
    const profileInfo = props.profileInfo;
    const [inputValue, setInputValue] = useState(0)
    const [animate, setAnimate] = useState(false)
    const [hideInput, setHideInput] = useState(false)
    const [transactionType, setTransactionType] = useState<string>("")

    const handleOnDepositClick = () => {
        setInputValue(0);
        setTransactionType("Deposit")
        setHideInput(false)
        setAnimate(true)

    }

    const handleOnWithdrawClick = () => {
        setInputValue(0);
        setTransactionType("Withdraw")
        setHideInput(false)
        setAnimate(true)
    }

    const handleTransactionClick = () => {
        if (transactionType === "Success" || transactionType === "Failed") {
            setAnimate(false)
            return
        }
        const amount = inputValue
        if (transactionType === "Withdraw") {
            API.CLIENT.WITHDRAW(amount)
                .then(response => {
                    if (response) {
                        setTransactionType("Success")
                        props.callback()
                    } else {
                        setTransactionType("Failed")
                    }
                })
        } else {
            API.CLIENT.DEPOSIT(amount)
                .then(response => {
                    if (response) {
                        setTransactionType("Success")
                        props.callback()
                    } else {
                        setTransactionType("Failed")
                    }
                })
        }
        setHideInput(true)
    }

    const handleUserInput = (e: any) => {
        setInputValue(e.target.value);
    };

    const handleCancelTransactionClick = () => {
        setAnimate(false)
    }

    return (
        <>
            {
                profileInfo &&
                <>
                    <div className="grid-cols-1">
                        <h1 className="flex justify-center py-1 text-text-display">{profileInfo.socialSecurityNumber}</h1>
                        <hr className="border-text-display"/>
                        <DataDisplay title="Name"
                                     content={`${profileInfo.firstName} ${profileInfo.lastName}`}/>
                        <DataDisplay title="Account balance" content={`${profileInfo.balance}$`}/>
                        <DataDisplay title="Portfolio Value" content={`${profileInfo.portfolio.value}$`}/>
                        <DataDisplay title="Address" content={`${profileInfo.address}`}/>
                        <DataDisplay title="City" content={`${profileInfo.postalCode}, ${profileInfo.postCity}`}/>
                        <div className="overflow-hidden flex justify-center">
                            <div className={`${animate && "-translate-y-16"} transition-all duration-700 ease-in-out`}>
                                <div className="w-fill flex justify-center mt-4 gap-4">
                                    <Button text={"Deposit"} onClick={handleOnDepositClick}/>
                                    <Button text={"Withdraw"} onClick={handleOnWithdrawClick}/>
                                </div>
                                <div
                                    className={`mt-8 flex justify-center transition-all ${!animate && "opacity-0 duration-300"} ${animate && "opacity-100 duration-1000"}`}>
                                    <div className="flex-row flex">
                                        <div
                                            className={`${hideInput && "opacity-0 -translate-x-12"} ${!hideInput && "opacity-100"} transition-all duration-300 w-1/2 flex justify-center mt-1`}>
                                            <input
                                                className="w-4/5 h-10 bg-transparent focus:border focus:border-pink-500 rounded-2xl pl-4 border border-black text-center"
                                                type="number" disabled={!animate} onChange={handleUserInput}
                                                value={inputValue}/>
                                        </div>
                                        <div
                                            className={`${hideInput && "-translate-x-10"} transition-all duration-300 flex flex-row gap-4`}>
                                            <Button text={transactionType} onClick={handleTransactionClick}/>
                                            <div
                                                className={`${hideInput && "opacity-0"} bg-gray-300 w-12 rounded-2xl w-fit group hover:bg-gradient-to-tl hover:from-gradient-start hover:to-gradient-end p-2 transition duration-300 ease-in-out`}
                                                onClick={handleCancelTransactionClick}>
                                                <svg
                                                    className="z-20 svg-icon group-hover:fill-white transition-all duration-300 ease-in-out"
                                                    viewBox="0 0 20 20">
                                                    <path
                                                        d="M15.898,4.045c-0.271-0.272-0.713-0.272-0.986,0l-4.71,4.711L5.493,4.045c-0.272-0.272-0.714-0.272-0.986,0s-0.272,0.714,0,0.986l4.709,4.711l-4.71,4.711c-0.272,0.271-0.272,0.713,0,0.986c0.136,0.136,0.314,0.203,0.492,0.203c0.179,0,0.357-0.067,0.493-0.203l4.711-4.711l4.71,4.711c0.137,0.136,0.314,0.203,0.494,0.203c0.178,0,0.355-0.067,0.492-0.203c0.273-0.273,0.273-0.715,0-0.986l-4.711-4.711l4.711-4.711C16.172,4.759,16.172,4.317,15.898,4.045z"></path>
                                                </svg>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </>
            }
            {
                !profileInfo &&
                <>
                    <div className="flex justify-center items-center w-full h-full">
                        <h2>No profile data</h2>
                    </div>
                </>
            }
        </>
    )
}
export default ProfileInfoContainer;