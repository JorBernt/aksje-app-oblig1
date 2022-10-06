import React from 'react';
import Card from "../UI/Card/Card";
import Navbar from "../Navbar/Navbar";
import StockContainer from "../StockContainer";
import SingleStockView from "../StockViews/SingleStockView";

const LandingPage = () => {
    document.body.style.backgroundColor = "black"
    const stockData = {
        "name": "Porsche AG",
        "last": 90.78,
        "todayPercent": "+3.25%",
        "todayDifference": "+2.86",
        "buy": 90.78,
        "sell": 90.80,
        "high": 93.70,
        "low": 89.50,
        "turnover": 1386098
    };
    let rowNames = new Map([
        ["Last", "last"],
        ["Today %", "todayPercent"],
        ["Today +/-", "todayDifference"],
        ["Buy", "buy"],
        ["Sell", "sell"],
        ["High", "high"],
        ["Low", "low"],
        ["Turnover", "turnover"]]);
    return (
        <>
            <Navbar/>
            <div className="flex flex-row justify-center mt-5">
                <div className="basis-1">
                    <div className="flex flex-auto">
                        <SingleStockView stockData={stockData} rowNames={rowNames}/>
                    </div>
                </div>
                <div className="basis-1">
                    <div className="flex flex-col">
                        <Card color="default">
                            <StockContainer text = "Dagens vinnere"/>
                        </Card>
                        <Card color="default">
                            <StockContainer text = {"Dagens tapere"}/>
                        </Card>
                    </div>
                </div>
            </div>
        </>
    )
}

export default LandingPage;