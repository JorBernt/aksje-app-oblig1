import React from "react";
import TransactionPreview from "./TransactionPreview";

const TransactionContainer = () => {
    const testTransaction = [
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},
        {time: "09-10-2022 17:02", price: 105.20, amount: 4800},]

    let counter: number = -1;
    return (
        <>
            <div className="justify-center flex-row px-0">
                <div className="flex justify-center text-black pb-2">
                    <h1 className="font-bold">Transaction history</h1>
                </div>
                <div className={"text-black grid grid-cols-4 px-5"}>
                    <p className="text-left col-span-2 ml-10">Time</p>
                    <p className="text-left">Price</p>
                    <p className="text-left">Amount</p>
                </div>
                <hr className="border-black pb-1 "/>
                <div className="w-max px-5 h-[14.25rem] scroll max-h-screen overflow-y-auto
                scrollbar scrollbar-track-white scrollbar-thumb-rounded-3xl scrollbar-thin scrollbar-thumb-blue-700">
                    {testTransaction.map((val) => {
                        counter++;
                        return counter % 2 === 0 ?
                            <div
                                className="hover:scale-105 hover:bg-gradient-to-br hover:from-white hover:to-gray-200 hover:rounded-lg rounded-lg transition duration-150 ease-in-out text-stock-preview-text-1 font-semibold">
                                <TransactionPreview transaction={val}/>
                            </div> :
                            <div
                                className="hover:scale-105 transition duration-150 ease-in-out bg-gradient-to-tl rounded-lg from-green-500 to-blue-700 text-stock-preview-text-2 font-semibold">
                                <TransactionPreview transaction={val}/>
                            </div>
                    })}
                </div>
            </div>
        </>
    )
}
export default TransactionContainer;