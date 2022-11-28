import StockPreview from "./StockPreview";
import React, {useEffect, useState} from "react";
import {Stock} from "../models";
import LoadingSpinner from "../UI/LoadingSpinner";

type Props = {
    text: String;
    showAmount: boolean;
    sorted: string;
    height: string;
    API?: string
    data?: Array<Stock>
    setName?: React.Dispatch<React.SetStateAction<string>>
}

const StockContainer = (props: Props) => {
    let data: Stock[] = []
    let view = React.createRef<HTMLDivElement>();
    const [stockView, setStockView] = useState<Stock[]>(data);
    const [showLoading, setShowLoading] = useState(true);
    const [error, setError] = useState(false)
    useEffect(() => {
        if (props.data) {
            setStockView(p => [...props.data ? props.data : p])
            setShowLoading(false)
            return;
        }
        if (typeof props.API !== "undefined")
            fetch(props.API)
                .then(response => response.json()
                    .then(response => {
                        setStockView(p => [...response])
                        setShowLoading(false)
                        if (props.setName) {
                            props.setName(response[0].symbol)
                        }
                    }).catch((error: Error) => {
                        setShowLoading(false)
                        setError(true)
                        console.log(error)
                    }))
    }, [props.data, props.API])
    let counter: number = 0;
    const headers = "text-stock-preview-text-1 grid px-5 " + (props.showAmount ? "grid-cols-5" : "grid-cols-4");
    const className = "w-full px-5 scroll max-h-screen overflow-y-auto scrollbar scrollbar-track-white scrollbar-thumb-rounded-3xl scrollbar-thin scrollbar-thumb-blue-700 " + props.height;
    return (
        <>
            <div className="justify-center flex-row px-0 min-w-max">
                <div className="flex justify-center text-black pb-0">
                    <h1 className="font-bold">{props.text} </h1>
                </div>
                <div className={headers}>
                    <p className="text-center">Name</p>
                    {props.showAmount &&
                        <p className="text-center">Amount</p>
                    }
                    <p className="text-center">Chart</p>
                    <p className="text-center">Change %</p>
                    <p className="text-center">Value $</p>
                </div>
                <hr className="pb-1 border-black pb-3"/>
                <div className={className}>
                    {!showLoading && stockView.length === 0 ?
                        <>
                            <div className="ml-28">
                                <p>No stocks to show.</p>
                            </div>
                        </>
                        :
                        <>
                            {error &&
                                <p className={"w-96 flex justify-center mt-48 text-6xl"}>ERROR!</p>
                            }
                            {showLoading && !error &&
                                <div className="flex justify-center items-center w-full h-full">
                                    <LoadingSpinner/>
                                </div>
                            }
                            {!showLoading && !error &&
                                <>
                                    {stockView.map((val) => {
                                            counter++;
                                            return counter % 2 === 0 ?
                                                <div key={counter}
                                                     className="cursor-pointer hover:scale-105 hover:bg-gradient-to-br hover:from-white hover:to-gray-200 hover:rounded-lg rounded-lg transition duration-150 ease-in-out text-stock-preview-text-1 font-semibold"
                                                     ref={view}>
                                                    <StockPreview key={val.symbol} items={val}
                                                                  showAmount={props.showAmount}/>
                                                </div> :
                                                <div key={counter}
                                                     className="cursor-pointer hover:scale-105 transition duration-150 ease-in-out bg-gradient-to-tl rounded-lg from-green-500 to-blue-700 text-stock-preview-text-2 font-semibold">
                                                    <StockPreview key={val.symbol} items={val}
                                                                  showAmount={props.showAmount}/>
                                                </div>
                                        }
                                    )}
                                </>
                            }
                        </>

                    }
                </div>
            </div>
        </>

    );
}

export default StockContainer