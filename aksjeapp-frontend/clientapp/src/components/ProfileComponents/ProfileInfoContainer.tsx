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
    let number: number = 8200.80
    let diff: number = -0.09
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
                    } else {
                        setTransactionType("Failed")
                    }
                })
        } else {
            API.CLIENT.DEPOSIT(amount)
                .then(response => {
                    if (response) {
                        setTransactionType("Success")
                    } else {
                        setTransactionType("Failed")
                    }
                })
        }
        setHideInput(true)
        props.callback()
    }

    const handleUserInput = (e: any) => {
        setInputValue(e.target.value);
    };

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
                        <DataDisplay title="Today  %" content={(diff > 0 ? "+" : "") + diff + "%"}
                                     color={+diff < 0 ? "text-red-500" : "text-green-500"}/>
                        <DataDisplay title="Today +/-" content={(number > 0 ? "+" : "") + number + "$"}
                                     color={+number < 0 ? "text-red-500" : "text-green-500"}/>
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
                                            className={`${hideInput && "opacity-0 -translate-x-12"} ${!hideInput && "opacity-100"} transition-all duration-300 w-1/2 flex justify-center`}>
                                            <input
                                                className="w-4/5 h-10 bg-transparent focus:border focus:border-pink-500 rounded-2xl pl-4 border border-black text-center"
                                                type="number" disabled={!animate} onChange={handleUserInput}
                                                value={inputValue}/>
                                        </div>
                                        <div
                                            className={`${hideInput && "-translate-x-10"} transition-all duration-300`}>
                                            <Button text={transactionType} onClick={handleTransactionClick}/>
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