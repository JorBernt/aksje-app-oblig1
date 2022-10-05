import StockPreview from "./StockPreview";
import React from "react";

type Props = {
    text:String
}

const StockContainer = (props: Props) => {

    const data = [{id: 0, name: "AAPL", chart: "chart", difference: 2.56, value: 100.45},
                {id: 1, name: "AMZN", chart: "chart", difference: -1.09, value: 80.10},
                {id: 2, name: "TSLA", chart: "chart", difference: 3.48, value: 90.81},
                {id: 3, name: "MSFT", chart: "chart", difference: 30.91, value: 211.21},
                {id: 4, name: "GOOGL", chart: "chart", difference: 2.45, value: 82.45},
                {id: 5, name: "BRK.A", chart: "chart", difference: -1.80, value: 89.92},
                {id: 6, name: "NVDA", chart: "chart", difference: 2.10, value: 34.90},
                {id: 7, name: "TSM", chart: "chart", difference: 12.22, value: 12.60}];
    return(
        <>
            <div className="justify-center flex-row px-0">
                <div className="flex justify-center text-black pb-2">
                    <h1 className="font-bold">{props.text}</h1>
                </div>
                <hr className="pb-1 border-black"/>
                <div className="w-max">
                    {data.map((val) => {
                            return val.id % 2 === 0 ?
                                <div className="bg-gray-500 text-black"><StockPreview items={val}/></div> :
                                <div className="bg-gray-600 text-black"><StockPreview items={val}/></div>
                        }

                    )}
                </div>
            </div>
        </>

    );
}

export default StockContainer