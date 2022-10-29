import React, {useState} from 'react';
import Card from "../UI/Card/Card";
import Navbar from "../Navbar/Navbar";
import StockContainer from "../StockViews/StockContainer";
import SingleStockView from "../StockViews/SingleStockView";
import {API} from "../../Constants";

interface SingeStockViewData {
    symbol: string,
    fromDate: string,
    toDate: string;
}

const LandingPage = () => {

    const [stockName, setStockName] = useState("")

    return (
        <>
            <div className="bg-background">
                <Navbar/>
                <div className="flex flex-row justify-center mt-5">
                    <div className="basis-1">
                        <div className="flex flex-auto">
                            {stockName.length > 0 && <SingleStockView symbol={"" + stockName} fromDate={"2022-08-20"}
                                                                      toDate={"2022-09-20"}/>}
                        </div>
                    </div>
                    <div className="basis-1">
                        <div className="flex flex-col">
                            <Card color="default" customCss={"m-5 px-5 pt-3 pb-4"}>
                                <StockContainer text="Winners of the day" showAmount={false} sorted="valDsc"
                                                height="h-[14.25rem]" API={API.GET_WINNERS} setName={setStockName}/>
                            </Card>
                            <Card color="default" customCss={"m-5 px-5 pt-3 pb-4"}>
                                <StockContainer text="Losers of the day" showAmount={false} sorted="valAsc"
                                                height="h-[14.25rem]" API={API.GET_LOSERS}/>
                            </Card>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}

export default LandingPage;