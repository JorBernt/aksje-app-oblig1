import React from "react";
import Chart from "../UI/Chart/Chart";
import Card from "../UI/Card/Card";
import TextDisplay from "../UI/TextDisplay/TextDisplay";

type Props = {
    stockData: { [key: string]: string | number }
}


const SingleStockView: React.FC<Props> = (props) => {
    const stockData = props.stockData;
    return (
        <>
            <Card color={"default"} customCss="w-max m-5 p-5">
                <p className="text-5xl text-center pb-5">{stockData.name}</p>
                <Chart/>
                <div className="grid grid-rows-3 grid-cols-3 mt-5 ">
                    <TextDisplay title={"Name"} content={stockData["name"]}></TextDisplay>
                    <TextDisplay title={"Last"} content={stockData["last"]}></TextDisplay>
                    <TextDisplay title={"Today %"} content={stockData["todayPercent"]}
                                 color={+stockData["todayPercent"] < 0 ? "text-red-500" : "text-green-500"}></TextDisplay>
                    <TextDisplay title={"Today +/-"} content={stockData["todayDifference"]}
                                 color={+stockData["todayDifference"] < 0 ? "text-red-500" : "text-green-500"}></TextDisplay>
                    <TextDisplay title={"Buy"} content={stockData["buy"]}></TextDisplay>
                    <TextDisplay title={"Sell"} content={stockData["sell"]}></TextDisplay>
                    <TextDisplay title={"High"} content={stockData["high"]}></TextDisplay>
                    <TextDisplay title={"Low"} content={stockData["low"]}></TextDisplay>
                    <TextDisplay title={"Turnover"} content={stockData["turnover"]}></TextDisplay>
                </div>
            </Card>
        </>
    )
}

export default SingleStockView;