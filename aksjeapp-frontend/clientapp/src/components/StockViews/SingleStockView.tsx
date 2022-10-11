import React from "react";
import Chart from "../UI/Chart/Chart";
import Card from "../UI/Card/Card";
import DataDisplay from "../UI/TextDisplay/DataDisplay";

type Props = {
    stockData: { [key: string]: string | number }
}

export const colorHandler = (color: number | string) => {
    return color === 0 ? "text-black" : color > 0 ? "text-green-500" : "text-red-500";
}

const SingleStockView: React.FC<Props> = (props) => {
    const stockData = props.stockData;

    return (
        <>
            <Card color={"default"} customCss="w-max m-5 p-5">
                <p className="text-5xl text-center pb-5">{stockData.name}</p>
                <Chart/>
                <div className="grid grid-rows-3 grid-cols-3 mt-5 ">
                    <DataDisplay title={"Name"} content={stockData["name"]}></DataDisplay>
                    <DataDisplay title={"Last"} content={stockData["last"]}></DataDisplay>
                    <DataDisplay title={"Today %"} content={stockData["todayPercent"] + "%"}
                                 color={colorHandler(stockData["todayPercent"])}></DataDisplay>
                    <DataDisplay title={"Today +/-"} content={stockData["todayDifference"]}
                                 color={colorHandler(stockData["todayDifference"])}></DataDisplay>
                    <DataDisplay title={"Buy"} content={stockData["buy"]}></DataDisplay>
                    <DataDisplay title={"Sell"} content={stockData["sell"]}></DataDisplay>
                    <DataDisplay title={"High"} content={stockData["high"]}></DataDisplay>
                    <DataDisplay title={"Low"} content={stockData["low"]}></DataDisplay>
                    <DataDisplay title={"Turnover"} content={stockData["turnover"]}></DataDisplay>
                </div>
            </Card>
        </>
    )
}

export default SingleStockView;