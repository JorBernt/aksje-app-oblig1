import StockPreview from "./StockPreview";
import React from "react";


const StockContainer = (props: any) => {

    const data = [{id: 0, name: "AAPL", chart: "chart", difference: 2.56, value: 100.45},
                {id: 1, name: "AMZN", chart: "chart", difference: 2.56, value: 100.45},
                {id: 2, name: "TSLA", chart: "chart", difference: 2.56, value: 100.45},
                {id: 3, name: "MSFT", chart: "chart", difference: 2.56, value: 100.45},
                {id: 4, name: "GOOGL", chart: "chart", difference: 2.56, value: 100.45},
                {id: 5, name: "BRK.A", chart: "chart", difference: 2.56, value: 100.45},
                {id: 6, name: "NVDA", chart: "chart", difference: 2.56, value: 100.45},
                {id: 7, name: "TSM", chart: "chart", difference: 2.56, value: 100.45}];
    return(
        <>
            <div className="justify-center flex-row px-0">
                <div className="flex justify-center text-black mb-4">
                    <h1>{props.text}</h1>
                </div>
                <div className="w-max">
                    {data.map((val) => {
                            if (val.id % 2 === 0) return  <div className="bg-gray-800 text-white "><StockPreview items={val}/></div>

                            return <div className="bg-gray-500 text-black "><StockPreview items={val}/></div>
                        }

                    )}
                </div>
            </div>
        </>

    );
}

export default StockContainer