import React from 'react';
import Card from "../UI/Card/Card";
import Navbar from "../Navbar/Navbar";
import StockContainer from "../StockViews/StockContainer";
import SingleStockView from "../StockViews/SingleStockView";
import {API} from "../../Constants";

const LandingPage = () => {

    return (
        <>
            <div className="bg-background">
                <Navbar/>
                <div className="flex flex-row justify-center mt-5">
                    <div className="basis-1">
                        <div className="flex flex-auto">
                            <SingleStockView symbol={"AAPL"} fromDate={"2022-09-24"} toDate={"2022-10-24"}/>
                        </div>
                    </div>
                    <div className="basis-1">
                        <div className="flex flex-col">
                            <Card color="default">
                                <StockContainer text="Winners of the day" showAmount={false} sorted="valDsc"
                                                height="h-[14.25rem]" API={API.GET_WINNERS}/>
                            </Card>
                            <Card color="default">
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