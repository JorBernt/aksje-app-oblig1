import StockPreview from "./StockPreview";
import React from "react";

type Props = {
    text: String;
    showAmount: boolean;
    sorted: string;
}

const StockContainer = (props: Props) => {
    const data = [{id: 0, name: "TCEHY", amount: 950, chart: "chart", difference: 4.01, value: 60.29},
        {name: "AMZN", amount: 120, chart: "chart", difference: -1.09, value: 80.10},
        {name: "TSLA", amount: 100000, chart: "chart", difference: 3.48, value: 90.81},
        {name: "MSFT", amount: 50, chart: "chart", difference: 30.91, value: 211.21},
        {name: "GOOGL", amount: 1500, chart: "chart", difference: 2.45, value: 82.45},
        {name: "BRK-B", amount: 1700, chart: "chart", difference: -1.80, value: 89.92},
        {name: "NVDA", amount: 800, chart: "chart", difference: 2.10, value: 34.90},
        {name: "TSM", amount: 500, chart: "chart", difference: 12.22, value: 12.60},
        {name: "AAPL", amount: 200, chart: "chart", difference: 2.56, value: 100.45},
        {name: "2222.SR", amount: 500, chart: "chart", difference: -2.08, value: 80.10},
        {name: "UNH", amount: 900, chart: "chart", difference: -2.48, value: 90.81},
        {name: "JNJ", amount: 1000, chart: "chart", difference: -4.72, value: 211.21},
        {name: "XOM", amount: 1000, chart: "chart", difference: 2.28, value: 82.45},
        {name: "V", amount: 10000, chart: "chart", difference: -9.30, value: 89.92},
        {name: "META", amount: 83000, chart: "chart", difference: 2.90, value: 34.90},
        {name: "WMT", amount: 9000, chart: "chart", difference: 15.32, value: 12.60},
        {name: "AMZN", amount: 120, chart: "chart", difference: -1.09, value: 80.10},
        {name: "TSLA", amount: 100000, chart: "chart", difference: 3.48, value: 90.81},
        {name: "MSFT", amount: 50, chart: "chart", difference: 30.91, value: 211.21},
        {name: "GOOGL", amount: 1500, chart: "chart", difference: 2.45, value: 82.45},
        {name: "BRK-B", amount: 1700, chart: "chart", difference: -1.80, value: 89.92},
        {name: "NVDA", amount: 800, chart: "chart", difference: 2.10, value: 34.90},
        {name: "TSM", amount: 500, chart: "chart", difference: 12.22, value: 12.60},
        {name: "AAPL", amount: 200, chart: "chart", difference: 2.56, value: 100.45},
        {name: "2222.SR", amount: 500, chart: "chart", difference: -2.08, value: 80.10},
        {name: "UNH", amount: 900, chart: "chart", difference: -2.48, value: 90.81},
        {name: "JNJ", amount: 1000, chart: "chart", difference: -4.72, value: 211.21},
        {name: "XOM", amount: 1000, chart: "chart", difference: 2.28, value: 82.45},
        {name: "V", amount: 10000, chart: "chart", difference: -9.30, value: 89.92},
        {name: "META", amount: 83000, chart: "chart", difference: 2.90, value: 34.90},
        {name: "WMT", amount: 9000, chart: "chart", difference: 15.32, value: 12.60},
        {name: "AMZN", amount: 120, chart: "chart", difference: -1.09, value: 80.10},
        {name: "TSLA", amount: 100000, chart: "chart", difference: 3.48, value: 90.81},
        {name: "MSFT", amount: 50, chart: "chart", difference: 30.91, value: 211.21},
        {name: "GOOGL", amount: 1500, chart: "chart", difference: 2.45, value: 82.45},
        {name: "BRK-B", amount: 1700, chart: "chart", difference: -1.80, value: 89.92},
        {name: "NVDA", amount: 800, chart: "chart", difference: 2.10, value: 34.90},
        {name: "TSM", amount: 500, chart: "chart", difference: 12.22, value: 12.60},
        {name: "AAPL", amount: 200, chart: "chart", difference: 2.56, value: 100.45},
        {name: "2222.SR", amount: 500, chart: "chart", difference: -2.08, value: 80.10},
        {name: "UNH", amount: 900, chart: "chart", difference: -2.48, value: 90.81},
        {name: "JNJ", amount: 1000, chart: "chart", difference: -4.72, value: 211.21},
        {name: "XOM", amount: 1000, chart: "chart", difference: 2.28, value: 82.45},
        {name: "V", amount: 10000, chart: "chart", difference: -9.30, value: 89.92},
        {name: "META", amount: 83000, chart: "chart", difference: 2.90, value: 34.90},
        {name: "WMT", amount: 9000, chart: "chart", difference: 15.32, value: 12.60},
        {name: "MSFT", amount: 50, chart: "chart", difference: 30.91, value: 211.21},
        {name: "GOOGL", amount: 1500, chart: "chart", difference: 2.45, value: 82.45},
        {name: "BRK-B", amount: 1700, chart: "chart", difference: -1.80, value: 89.92},
        {name: "NVDA", amount: 800, chart: "chart", difference: 2.10, value: 34.90},
        {name: "TSM", amount: 500, chart: "chart", difference: 12.22, value: 12.60},
        {name: "AAPL", amount: 200, chart: "chart", difference: 2.56, value: 100.45},
        {name: "2222.SR", amount: 500, chart: "chart", difference: -2.08, value: 80.10},
        {name: "UNH", amount: 900, chart: "chart", difference: -2.48, value: 90.81},
        {name: "JNJ", amount: 1000, chart: "chart", difference: -4.72, value: 211.21},
        {name: "XOM", amount: 1000, chart: "chart", difference: 2.28, value: 82.45},
        {name: "V", amount: 10000, chart: "chart", difference: -9.30, value: 89.92},
        {name: "META", amount: 83000, chart: "chart", difference: 2.90, value: 34.90},
        {name: "WMT", amount: 9000, chart: "chart", difference: 15.32, value: 12.60},
        {name: "MSFT", amount: 50, chart: "chart", difference: 30.91, value: 211.21},
        {name: "GOOGL", amount: 1500, chart: "chart", difference: 2.45, value: 82.45},
        {name: "BRK-B", amount: 1700, chart: "chart", difference: -1.80, value: 89.92},
        {name: "NVDA", amount: 800, chart: "chart", difference: 2.10, value: 34.90},
        {name: "TSM", amount: 500, chart: "chart", difference: 12.22, value: 12.60},
        {name: "AAPL", amount: 200, chart: "chart", difference: 2.56, value: 100.45},
        {name: "2222.SR", amount: 500, chart: "chart", difference: -2.08, value: 80.10},
        {name: "UNH", amount: 900, chart: "chart", difference: -2.48, value: 90.81},
        {name: "JNJ", amount: 1000, chart: "chart", difference: -4.72, value: 211.21},
        {name: "XOM", amount: 1000, chart: "chart", difference: 2.28, value: 82.45},
        {name: "V", amount: 10000, chart: "chart", difference: -9.30, value: 89.92},
        {name: "META", amount: 83000, chart: "chart", difference: 2.90, value: 34.90},
        {name: "WMT", amount: 9000, chart: "chart", difference: 15.32, value: 12.60},
        {name: "MSFT", amount: 50, chart: "chart", difference: 30.91, value: 211.21},
        {name: "GOOGL", amount: 1500, chart: "chart", difference: 2.45, value: 82.45},
        {name: "BRK-B", amount: 1700, chart: "chart", difference: -1.80, value: 89.92},
        {name: "NVDA", amount: 800, chart: "chart", difference: 2.10, value: 34.90},
        {name: "TSM", amount: 500, chart: "chart", difference: 12.22, value: 12.60},
        {name: "AAPL", amount: 200, chart: "chart", difference: 2.56, value: 100.45},
        {name: "2222.SR", amount: 500, chart: "chart", difference: -2.08, value: 80.10},
        {name: "UNH", amount: 900, chart: "chart", difference: -2.48, value: 90.81},
        {name: "JNJ", amount: 1000, chart: "chart", difference: -4.72, value: 211.21},
        {name: "XOM", amount: 1000, chart: "chart", difference: 2.28, value: 82.45},
        {name: "V", amount: 10000, chart: "chart", difference: -9.30, value: 89.92},
        {name: "META", amount: 83000, chart: "chart", difference: 2.90, value: 34.90},
        {name: "WMT", amount: 9000, chart: "chart", difference: 15.32, value: 12.60},
        {name: "MSFT", amount: 50, chart: "chart", difference: 30.91, value: 211.21},
        {name: "GOOGL", amount: 1500, chart: "chart", difference: 2.45, value: 82.45},
        {name: "BRK-B", amount: 1700, chart: "chart", difference: -1.80, value: 89.92},
        {name: "NVDA", amount: 800, chart: "chart", difference: 2.10, value: 34.90},
        {name: "TSM", amount: 500, chart: "chart", difference: 12.22, value: 12.60},
        {name: "AAPL", amount: 200, chart: "chart", difference: 2.56, value: 100.45},
        {name: "2222.SR", amount: 500, chart: "chart", difference: -2.08, value: 80.10},
        {name: "UNH", amount: 900, chart: "chart", difference: -2.48, value: 90.81},
        {name: "JNJ", amount: 1000, chart: "chart", difference: -4.72, value: 211.21},
        {name: "XOM", amount: 1000, chart: "chart", difference: 2.28, value: 82.45},
        {name: "V", amount: 10000, chart: "chart", difference: -9.30, value: 89.92},
        {name: "META", amount: 83000, chart: "chart", difference: 2.90, value: 34.90},
        {name: "WMT", amount: 9000, chart: "chart", difference: 15.32, value: 12.60}];


    let sorted;
    switch (props.sorted) {
        case "valAsc": {
            sorted = [...data].sort((a, b) => a.value - b.value);
            break;
        }
        case "valDsc": {
            sorted = [...data].sort((a, b) => b.value - a.value);
            break;
        }
        case "diffAsc": {
            sorted = [...data].sort((a, b) => a.difference - b.difference);
            break;
        }
        case "diffDsc": {
            sorted = [...data].sort((a, b) => b.difference - a.difference);
            break;
        }
        case "nameAsc": {
            sorted = [...data].sort((a, b) => a.name > b.name ? 1 : -1);
            break;
        }
        case "nameDsc": {
            sorted = [...data].sort((a, b) => a.name > b.name ? -1 : 1);
            break;
        }
        case "amountAsc": {
            sorted = [...data].sort((a, b) => a.name > b.name ? 1 : -1);
            break;
        }
        case "amountDsc": {
            sorted = [...data].sort((a, b) => a.amount - b.amount);
            break;
        }
        default:
            sorted = [...data].sort((a, b) => b.amount - a.amount);
    }

    let counter: number = -1;
    const headers = "text-stock-preview-text-1 grid px-5 " + (props.showAmount ? "grid-cols-5" : "grid-cols-4");
    return (
        <>
            <div className="justify-center flex-row px-0">
                <div className="flex justify-center text-black pb-2">
                    <h1 className="font-bold">{props.text}</h1>
                </div>
                <div className={headers}>
                    <p className="text-center">Name</p>
                    {props.showAmount ? <p className="text-center">Amount</p> : null}
                    <p className="text-center">Chart</p>
                    <p className="text-center">Change</p>
                    <p className="text-center">Value</p>
                </div>
                <hr className="pb-1 border-black"/>
                <div className="w-max px-5 h-[28.5rem] scroll max-h-screen overflow-y-auto
                scrollbar scrollbar-track-white scrollbar-thumb-rounded-3xl scrollbar-thin scrollbar-thumb-blue-700">
                    {sorted.map((val) => {
                        counter++;
                        return counter % 2 === 0 ?
                            <div
                                className="hover:scale-105 hover:bg-gradient-to-br hover:from-white hover:to-gray-200 hover:rounded-lg rounded-lg transition duration-150 ease-in-out text-stock-preview-text-1 font-semibold">
                                <StockPreview items={val} showAmount={props.showAmount}/>
                            </div> :
                            <div
                                className="hover:scale-105 transition duration-150 ease-in-out bg-gradient-to-tl rounded-lg from-green-500 to-blue-700 text-stock-preview-text-2 font-semibold">
                                <StockPreview items={val} showAmount={props.showAmount}/>
                                </div>
                        }
                    )}
                </div>
            </div>
        </>

    );
}

export default StockContainer