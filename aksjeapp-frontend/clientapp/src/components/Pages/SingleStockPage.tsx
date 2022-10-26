import Navbar from "../Navbar/Navbar";
import SingleStockView from "../StockViews/SingleStockView";
import Card from "../UI/Card/Card";
import TransactionContainer from "../Transactions/TransactionContainer";
import {useSearchParams} from "react-router-dom";
import StockBuySell from "../StockViews/StockBuySell";

let stockData = {
    "symbol" : "AAPL",
    "name": "Porsche AG",
    "last": 90.78,
    "todayPercent": 3.25,
    "todayDifference": -1.86,
    "buy": 90.78,
    "sell": 90.80,
    "high": 93.70,
    "low": 89.50,
    "turnover": 1386098
};

const SingleStockPage = () => {
    const [searchParams] = useSearchParams();
    stockData.symbol = String(searchParams.get("symbol"))

    return (
        <>
            <div>
                <Navbar/>
                <h1 className="text-center font-semi text-5xl bold py-10">{stockData.name}</h1>
                <div className="flex flex-row justify-center">
                    <div className="flex justify-center">
                        <SingleStockView stockData={stockData}/>
                    </div>
                    <div className="flex flex-col">
                        <Card customCss="h-1/2 p-5 m-5">
                            <TransactionContainer/>
                        </Card>
                        <Card>
                            <div className="flex flex-col justify-between">
                                <StockBuySell data={"Buy"} className="bg-green-500 hover:shadow-green-300"/>
                            </div>
                        </Card>
                        <Card >
                            <div className="flex flex-col justify-between">
                                <StockBuySell data={"Sell"} className="bg-red-500 hover:shadow-red-300"/>
                            </div>
                        </Card>
                    </div>
                </div>
                <div className="flex justify-center">

                </div>
            </div>
        </>
    )
}

export default SingleStockPage;