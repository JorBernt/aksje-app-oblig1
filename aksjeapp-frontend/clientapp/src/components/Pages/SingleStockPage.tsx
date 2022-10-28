import Navbar from "../Navbar/Navbar";
import SingleStockView from "../StockViews/SingleStockView";
import Card from "../UI/Card/Card";
import TransactionContainer from "../Transactions/TransactionContainer";
import {useSearchParams} from "react-router-dom";
import StockBuySell from "../StockViews/StockBuySell";
import React, {useEffect, useState} from "react";
import {API} from "../../Constants";


const SingleStockPage = () => {
    const [searchParams] = useSearchParams();
    const [symbol, setSymbol] = useState("")

    const [stockPrice, setStockprice] = useState(0)


    const [name, setName] = useState("")
    useEffect(() => {
        fetch(API.GET_STOCK_NAME(symbol)).then(response => response.text().then(text => setName(text)))
        setSymbol(String(searchParams.get("symbol")))
    }, [searchParams, symbol])
    return (
        <>
            <div>
                <Navbar/>
                <div className="flex flex-row justify-center">
                    <div className="flex justify-center">
                        <SingleStockView symbol={symbol} fromDate={"2022-09-24"} toDate={"2022-10-24"}
                                         setStockPrice={setStockprice}/>
                    </div>
                    <div className="flex flex-col">
                        <Card customCss="h-1/2 p-5 m-5">
                            <TransactionContainer symbol={symbol}/>
                        </Card>
                        <Card>
                            <div className="flex flex-col justify-between">
                                <StockBuySell data={"Buy"} className="bg-green-500 hover:shadow-green-300"
                                              symbol={symbol} cost={stockPrice}/>
                            </div>
                        </Card>
                        <Card>
                            <div className="flex flex-col justify-between">
                                <StockBuySell data={"Sell"} className="bg-red-500 hover:shadow-red-300" symbol={symbol}
                                              cost={stockPrice}/>
                            </div>
                        </Card>
                    </div>
                </div>

            </div>
        </>
    )
}

export default SingleStockPage;