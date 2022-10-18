import StockPreview from "./StockPreview";
import React, {useEffect, useState} from "react";
import {Stock} from "../models";
import {API} from "../../Constants";
import LoadingSpinner from "../UI/LoadingSpinner";

type Props = {
    text: String;
    showAmount: boolean;
    sorted: string;
    height: string;
}

const StockContainer = (props: Props) => {
    let data: Stock[] = [{symbol: "", name: "Waiting", change: 1.01, value: ""}]
    let view = React.createRef<HTMLDivElement>();
    const [stockView, setStockView] = useState(data);
    const [showLoading, setShowLoading] = useState(true);
    const [error, setError] = useState(false)
    useEffect(() => {
        fetch(API.GET_STOCK_OVERVIEW)
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
                        <LoadingSpinner/>
                    }
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