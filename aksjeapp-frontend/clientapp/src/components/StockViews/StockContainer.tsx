import StockPreview from "./StockPreview";
import React, {useEffect, useState} from "react";
import {Stock} from "../models";

type Props = {
    text: String;
    showAmount: boolean;
    sorted: string;
    height: string;
}

const StockContainer = (props: Props) => {
    let data: Stock[] = [{country: "", name: "", symbol: "Waiting", sector: ""}]
    let view = React.createRef<HTMLDivElement>();
    const [stockView, setStockView] = useState(data);
    const [showLoading, setShowLoading] = useState(true);
    const [error, setError] = useState(false)
    useEffect(() => {
        fetch("https://localhost:7187/Stock/GetAllStocks")
            .then(response => response.json()
                .then(response => {
                    setStockView(p => [...response])
                    setShowLoading(false)

                }).catch(e => {
                    setShowLoading(false)
                    setError(true)
                    console.log(e.message)
                }))
    }, [])
    let counter: number = -1;
    const headers = "text-stock-preview-text-1 grid px-5 " + (props.showAmount ? "grid-cols-5" : "grid-cols-4");
    const className = "w-max px-5 scroll max-h-screen overflow-y-auto scrollbar scrollbar-track-white scrollbar-thumb-rounded-3xl scrollbar-thin scrollbar-thumb-blue-700 " + props.height;
    return (
        <>
            <div className="justify-center flex-row px-0">
                <div className="flex justify-center text-black pb-2">
                    <h1 className="font-bold">{props.text} </h1>
                </div>
                <div className={headers}>
                    <p className="text-center">Name</p>
                    {props.showAmount ? <p className="text-center">Amount</p> : null}
                    <p className="text-center">Chart</p>
                    <p className="text-center">Change</p>
                    <p className="text-center">Value</p>
                </div>
                <hr className="pb-1 border-black"/>
                <div className={className}>
                    {error &&
                        <p className={"w-96 flex justify-center mt-48 text-6xl"}>ERROR!</p>
                    }
                    {showLoading && !error &&
                        <div role="status" className={"w-96 flex justify-center mt-52"}>
                            <svg
                                className="inline mr-2 w-10 h-10 text-gray-200 animate-spin dark:text-gray-600 fill-blue-600"
                                viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path
                                    d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z"
                                    fill="currentColor"/>
                                <path
                                    d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z"
                                    fill="currentFill"/>
                            </svg>
                            <span className="sr-only">Loading...</span>
                        </div>}
                    {!showLoading && !error &&
                        <>
                            {stockView.map((val) => {
                                    counter++;
                                    return counter % 2 === 0 ?
                                        <div
                                            className="hover:scale-105 hover:bg-gradient-to-br hover:from-white hover:to-gray-200 hover:rounded-lg rounded-lg transition duration-150 ease-in-out text-stock-preview-text-1 font-semibold"
                                            ref={view}>
                                            <StockPreview key={counter} items={val} showAmount={props.showAmount}/>
                                        </div> :
                                        <div
                                            className="hover:scale-105 transition duration-150 ease-in-out bg-gradient-to-tl rounded-lg from-green-500 to-blue-700 text-stock-preview-text-2 font-semibold">
                                            <StockPreview key={counter} items={val} showAmount={props.showAmount}/>
                                        </div>
                                }
                            )}
                        </>
                    }
                </div>
            </div>
        </>

    );
}

export default StockContainer