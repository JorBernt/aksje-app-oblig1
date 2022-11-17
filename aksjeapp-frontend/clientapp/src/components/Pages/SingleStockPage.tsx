import Navbar from "../Navbar/Navbar";
import SingleStockView from "../StockViews/SingleStockView";
import Card from "../UI/Card/Card";
import TransactionContainer from "../Transactions/TransactionContainer";
import {useNavigate, useSearchParams} from "react-router-dom";
import StockBuySell from "../StockViews/StockBuySell";
import React, {useEffect, useState} from "react";
import {useLoggedInContext} from "../../App";
import Button from "../UI/Input/Button";


const SingleStockPage = () => {
    const [searchParams] = useSearchParams();
    const [symbol, setSymbol] = useState(String(searchParams.get("symbol")))
    const [stockPrice, setStockprice] = useState(0)
    const [reload, setReload] = useState(false)
    const isLoggedIn = useLoggedInContext().loggedIn;
    const navigate = useNavigate()

    useEffect(() => {
        setSymbol(String(searchParams.get("symbol")))
    }, [searchParams])

    const handleCallback = () => {
        setReload(val => !val)
    }

    const handleOnClick = () => {
        navigate("/login")
    }

    const notLoggedInContainers = (text: string) => {
        return (
            <>
                <Card customCss="relative p-5 m-5">
                    <div className="z-10 absolute left-0 right-0 bottom-0 h-full">
                        <div className="flex justify-center items-center h-full">
                            <Button text={`Login to ${text} Stock`} onClick={handleOnClick}/>
                        </div>
                    </div>

                    <div className="flex flex-col justify-between blur grayscale">
                        <StockBuySell data={"Sell"} className="bg-red-500 hover:shadow-red-300"
                                      symbol={symbol}
                                      cost={stockPrice} callBack={handleCallback} disabled={true}/>
                    </div>
                </Card>
            </>
        )
    }
    return (
        <>
            <div>
                <Navbar/>
                <div className="flex flex-row justify-center">
                    <div className="flex justify-center">
                        <SingleStockView symbol={symbol} fromDate={"2022-08-20"} toDate={"2022-09-20"}
                                         setStockPrice={setStockprice}/>
                    </div>
                    <div className="flex flex-col">
                        <Card customCss="h-1/2 p-5 m-5">
                            <TransactionContainer reloadComponent={reload} symbol={symbol}/>
                        </Card>
                        {isLoggedIn ?
                            <>

                                <Card>
                                    <div className="flex flex-col justify-between">
                                        <StockBuySell data={"Buy"} className="bg-green-500 hover:shadow-green-300"
                                                      symbol={symbol} cost={stockPrice} callBack={handleCallback}/>
                                    </div>
                                </Card>
                                <Card>
                                    <div className="flex flex-col justify-between">
                                        <StockBuySell data={"Sell"} className="bg-red-500 hover:shadow-red-300"
                                                      symbol={symbol}
                                                      cost={stockPrice} callBack={handleCallback}/>
                                    </div>
                                </Card>
                            </>
                            :
                            <>
                                {notLoggedInContainers("Buy")}
                                {notLoggedInContainers("Sell")}
                            </>
                        }
                    </div>
                </div>

            </div>
        </>
    )
}

export default SingleStockPage;