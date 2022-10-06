import StockPreview from "./StockPreview";
import React from "react";

type Props = {
    text: String;
    showAmount: boolean;
}

const StockContainer = (props: Props) => {
    const data = [{id: 0, name: "AAPL", amount: 100, chart: "chart", difference: 2.56, value: 100.45},
        {id: 1, name: "AMZN", amount: 100, chart: "chart", difference: -1.09, value: 80.10},
        {id: 2, name: "TSLA", amount: 100, chart: "chart", difference: 3.48, value: 90.81},
        {id: 3, name: "MSFT", amount: 100, chart: "chart", difference: 30.91, value: 211.21},
        {id: 4, name: "GOOGL", amount: 100, chart: "chart", difference: 2.45, value: 82.45},
        {id: 5, name: "BRK.A", amount: 100, chart: "chart", difference: -1.80, value: 89.92},
        {id: 6, name: "NVDA", amount: 100, chart: "chart", difference: 2.10, value: 34.90},
        {id: 7, name: "TSM", amount: 100, chart: "chart", difference: 12.22, value: 12.60},
        {id: 0, name: "AAPL", amount: 100, chart: "chart", difference: 2.56, value: 100.45},
        {id: 1, name: "AMZN", amount: 100, chart: "chart", difference: -1.09, value: 80.10},
        {id: 2, name: "TSLA", amount: 100, chart: "chart", difference: 3.48, value: 90.81},
        {id: 3, name: "MSFT", amount: 100, chart: "chart", difference: 30.91, value: 211.21},
        {id: 4, name: "GOOGL", amount: 100, chart: "chart", difference: 2.45, value: 82.45},
        {id: 5, name: "BRK.A", amount: 100, chart: "chart", difference: -1.80, value: 89.92},
        {id: 6, name: "NVDA", amount: 100, chart: "chart", difference: 2.10, value: 34.90},
        {id: 7, name: "TSM", amount: 100, chart: "chart", difference: 12.22, value: 12.60}];
    return (
        <>
            <div className="justify-center flex-row px-0">
                {/*TODO: Legge til tittel over hver kolonne */}
                <div className="flex justify-center text-black pb-2">
                    <h1 className="font-bold">{props.text}</h1>
                </div>
                <hr className="pb-1 border-black"/>
                <div className="w-max h-96 scroll max-h-screen overflow-y-auto">
                    {data.map((val) => {
                            return val.id % 2 === 0 ?
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