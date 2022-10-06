import React from 'react';
import Card from "../UI/Card/Card";
import Navbar from "../Navbar/Navbar";
import StockContainer from "../StockViews/StockContainer";
import SingleStockView from "../StockViews/SingleStockView";

const LandingPage = () => {
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
            <div className="bg-background">
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
                                <StockContainer text="Dagens vinnere" showAmount={false}/>
                            </Card>
                            <Card color="default">
                                <StockContainer text="Dagens tapere" showAmount={false}/>
                            </Card>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}

export default LandingPage;