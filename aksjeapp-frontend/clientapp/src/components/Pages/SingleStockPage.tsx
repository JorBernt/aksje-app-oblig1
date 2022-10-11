import Navbar from "../Navbar/Navbar";
import SingleStockView from "../StockViews/SingleStockView";
import Card from "../UI/Card/Card";
import TransactionContainer from "../Transactions/TransactionContainer";

const stockData = {
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
    return (
        <>
            <div>
                <Navbar/>
                <h1 className="text-center font-semi text-5xl bold py-10">{stockData.name}</h1>
                <div className="flex flex-row justify-center">
                    <div className="flex justify-center">
                        <SingleStockView stockData={stockData}/>
                    </div>
                    <Card customCss="h-1/2 p-5 m-5">
                        <TransactionContainer/>
                    </Card>
                </div>
                <div className="flex justify-center">
                    <Card>
                        <h1 className="text-center font-semibold">Buy and Sell buttons</h1>
                    </Card>
                </div>
            </div>
        </>
    )
}

export default SingleStockPage;