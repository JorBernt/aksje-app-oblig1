import Navbar from "../Navbar/Navbar";
import SingleStockView from "../StockViews/SingleStockView";
import Card from "../UI/Card/Card";
import TransactionContainer from "../Transactions/TransactionContainer";
import {useSearchParams} from "react-router-dom";
import StockBuySell from "../StockViews/StockBuySell";
import React, {useEffect, useState} from "react";


const SingleStockPage = () => {
    const [searchParams] = useSearchParams();
    const [symbol, setSymbol] = useState(String(searchParams.get("symbol")))
    const [stockPrice, setStockprice] = useState(0)
    const [reload, setReload] = useState(false)

    useEffect(() => {
        setSymbol(String(searchParams.get("symbol")))
    }, [searchParams])

    const handleCallback = () => {
        setReload(val => !val)
    }

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
                            <TransactionContainer reloadComponent={reload} symbol={symbol}/>
                        </Card>
                        <Card>
                            <div className="flex flex-col justify-between">
                                <StockBuySell data={"Buy"} className="bg-green-500 hover:shadow-green-300"
                                              symbol={symbol} cost={stockPrice} callBack={handleCallback}/>
                            </div>
                        </Card>
                        <Card>
                            <div className="flex flex-col justify-between">
                                <StockBuySell data={"Sell"} className="bg-red-500 hover:shadow-red-300" symbol={symbol}
                                              cost={stockPrice} callBack={handleCallback}/>
                            </div>
                        </Card>
                    </div>
                </div>

            </div>
        </>
    )
}

export default SingleStockPage;