import StockPreview from "./StockPreview";
import React from "react";

type Props = {
    text: String;
    showAmount: boolean;
    sorted: string;
}

const StockContainer = (props: Props) => {
    const data = [{id: 0, name: "AAPL", amount: 100, chart: "chart", difference: 2.56, value: 100.45},
        {name: "AMZN", amount: 100, chart: "chart", difference: -1.09, value: 80.10},
        {name: "TSLA", amount: 100, chart: "chart", difference: 3.48, value: 90.81},
        {name: "MSFT", amount: 100, chart: "chart", difference: 30.91, value: 211.21},
        {name: "GOOGL", amount: 100, chart: "chart", difference: 2.45, value: 82.45},
        {name: "BRK.A", amount: 100, chart: "chart", difference: -1.80, value: 89.92},
        {name: "NVDA", amount: 100, chart: "chart", difference: 2.10, value: 34.90},
        {name: "TSM", amount: 100, chart: "chart", difference: 12.22, value: 12.60},
        {name: "AAPL", amount: 100, chart: "chart", difference: 2.56, value: 100.45},
        {name: "AMZN", amount: 100, chart: "chart", difference: -1.09, value: 80.10},
        {name: "TSLA", amount: 100, chart: "chart", difference: 3.48, value: 90.81},
        {name: "MSFT", amount: 100, chart: "chart", difference: 30.91, value: 211.21},
        {name: "GOOGL", amount: 100, chart: "chart", difference: 2.45, value: 82.45},
        {name: "BRK.A", amount: 100, chart: "chart", difference: -1.80, value: 89.92},
        {name: "NVDA", amount: 100, chart: "chart", difference: 2.10, value: 34.90},
        {name: "TSM", amount: 100, chart: "chart", difference: 12.22, value: 12.60}];


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
    return (
        <>
            <div className="justify-center flex-row px-0">
                {/*TODO: Legge til tittel over hver kolonne */}
                <div className="flex justify-center text-black pb-2">
                    <h1 className="font-bold">{props.text}</h1>
                </div>
                <hr className="pb-1 border-black"/>
                <div className="w-max h-96 scroll max-h-screen overflow-y-auto">
                    {sorted.map((val) => {
                            counter++;
                            return counter % 2 === 0 ?
                                <div className="text-black"><StockPreview items={val} showAmount={props.showAmount}/>
                                </div> :
                                <div className="bg-stock-preview-line text-white"><StockPreview items={val}
                                                                                                showAmount={props.showAmount}/>
                                </div>
                        }
                    )}
                </div>
            </div>
        </>

    );
}

export default StockContainer