import React, {useRef, useState} from "react";
import {API, SSN} from "../../Constants";
import LoadingSpinner from "../UI/LoadingSpinner";

type Props = {
    data: string;
    className: string
    symbol: string
    cost: number
    callBack: any
};

const StockBuySell = (props: Props) => {
    const [showTransactionResponse, setShowTransactionResponse] = useState(false)
    const [successTransaction, setSuccessTransaction] = useState("")
    const [okButton, setOkButton] = useState("OK")
    const [showLoading, setShowLoading] = useState(false)

    const inputRef = useRef<HTMLInputElement>(null);
    const handleOnClickBuySell = () => {
        if (!inputRef.current)
            return

        setShowLoading(true)
        if (props.data === "Buy")
            fetch(API.STOCK.BUY_STOCK(SSN, props.symbol, parseInt(inputRef.current.value)))
                .then(response => response.text()
                    .then(response => {
                        if (inputRef.current)
                            inputRef.current.value = ""
                        setShowTransactionResponse(true)
                        setSuccessTransaction(response)
                        setOkButton("Buy More")
                        setShowLoading(false)
                        props.callBack();
                    }))

        else
            fetch(API.STOCK.SELL_STOCK(SSN, props.symbol, parseInt(inputRef.current.value)))
                .then(response => response.text()
                    .then(response => {
                        if (inputRef.current)
                            inputRef.current.value = ""
                        setShowTransactionResponse(true)
                        setSuccessTransaction(response)
                        setOkButton("Sell More")
                        setShowLoading(false)
                        props.callBack();
                    }))
    }


    const handleOnClickOK = () => {
        setShowTransactionResponse(false)
        if (inputRef.current)
            inputRef.current.value = ""
        setTotalSum(0)
        setOkButton("Buy More")
    }

    const [totalSum, setTotalSum] = useState(0)

    const handleOnChange = () => {
        if (!inputRef.current)
            return
        const sum = props.cost * inputRef.current.valueAsNumber
        const min = 1, max = 1000
        inputRef.current.value = (Math.max(min, Math.min(max, Number(inputRef.current.valueAsNumber)))).toString();
        if (inputRef.current.value === "") {
            setTotalSum(0);
        }

        if (sum >= 0)
            setTotalSum(sum)
    }

    const className = "text-white font-bold py-2 px-4 rounded-full m-4 hover:shadow-xl w-40 transition duration-300 " + props.className;
    const amount = "Amount";

    return (
        <>
            {showLoading &&
                <div className="h-[6.5rem] flex justify-center items-center">
                    <LoadingSpinner/>
                </div>
            }
            {!showTransactionResponse && !showLoading &&
                <div className="flex flex-col ">
                    <div className="grid grid-cols-3 ml-4">
                        <p className="text-2xl ">{amount} </p>
                        <input
                            className="mx-4 w-24 bg-transparent focus:border focus:border-pink-500 rounded pl-4 border border-black"
                            type="number" ref={inputRef} onChange={handleOnChange}/>

                        <p className="ml-1 self-end text-2xl">{totalSum.toFixed(2)} $</p>
                    </div>
                    <div className="flex justify-center">
                        <button
                            className={className} onClick={handleOnClickBuySell}>{props.data}
                        </button>
                    </div>
                </div>
            }

            {showTransactionResponse && !showLoading &&
                <div className="flex flex-col h-[6.5rem]">
                    <div className="flex justify-center ">
                        <p className="text-0.5 xl">{successTransaction}</p>
                    </div>
                    <div className="flex justify-center">
                        <button
                            className={className} onClick={handleOnClickOK}>{okButton}
                        </button>
                    </div>
                </div>

            }
        </>
    );
};

export default StockBuySell;
