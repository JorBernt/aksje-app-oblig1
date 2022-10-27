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
    const symbol = String(searchParams.get("symbol"))
    const [name, setName] = useState("")
    useEffect(() => {
        fetch(API.GET_STOCK_NAME(symbol)).then(response => response.text().then(text => setName(text)))
    }, [])
    return (
        <>
            <div>
                <Navbar/>
                <h1 className="text-center font-semi text-5xl bold py-10">{name}</h1>
                <div className="flex flex-row justify-center">
                    <div className="flex justify-center">
                        <SingleStockView symbol={symbol} fromDate={"2022-09-24"} toDate={"2022-10-24"}/>
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
                        <Card>
                            <div className="flex flex-col justify-between">
                                <StockBuySell data={"Sell"} className="bg-red-500 hover:shadow-red-300"/>
                            </div>
                        </Card>
                    </div>
                </div>

            </div>
        </>
    )
}

export default SingleStockPage;