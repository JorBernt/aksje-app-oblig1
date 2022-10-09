import NewsDisplay from "./UI/TextDisplay/NewsDisplay";
import React from "react";

const NewsContainer = () => {
    return (
        <>
            <div className="h-[32rem]">
                <div className="flex justify-center text-stock-preview-text-1 pb-2">
                    <h1 className="font-bold">News and other events</h1>
                </div>
                <div className={"text-black px-5 grid grid-cols-4"}>
                    <p className="text-left pl-1 col-span-2">Content</p>
                    <p className="text-left pl-[5.5rem] col-span-2">Related stocks</p>
                </div>
                <hr className="border-text-display mb-1"/>
                <div
                    className="scroll h-[28.5rem] overflow-y-auto pr-0.5 scrollbar scrollbar-track-white scrollbar-thumb-rounded-3xl scrollbar-thin scrollbar-thumb-gray-300 scrollbar">
                    <NewsDisplay title={"Another red day on the market"} date="08-10-2022 08:32"
                                 content={"Market remains under pressure as..."}
                                 affectedStocks="AAPL, AMZN, MSFT, GOOGL, SHOP, TSLA, META"/>
                    <NewsDisplay title={"Another red day on the market"} date="08-10-2022 08:32"
                                 content={"Market remains under pressure as..."}
                                 affectedStocks="AAPL, AMZN, MSFT, GOOGL, SHOP, TSLA, META"/>
                    <NewsDisplay title={"Another red day on the market"} date="08-10-2022 08:32"
                                 content={"Market remains under pressure as..."}
                                 affectedStocks="AAPL, AMZN, MSFT, GOOGL, SHOP, TSLA, META"/>
                    <NewsDisplay title={"Another red day on the market"} date="08-10-2022 08:32"
                                 content={"Market remains under pressure as..."}
                                 affectedStocks="AAPL, AMZN, MSFT, GOOGL, SHOP, TSLA, META"/>
                    <NewsDisplay title={"Another red day on the market"} date="08-10-2022 08:32"
                                 content={"Market remains under pressure as..."}
                                 affectedStocks="AAPL, AMZN, MSFT, GOOGL, SHOP, TSLA, META"/>
                    <NewsDisplay title={"Another red day on the market"} date="08-10-2022 08:32"
                                 content={"Market remains under pressure as..."}
                                 affectedStocks="AAPL, AMZN, MSFT, GOOGL, SHOP, TSLA, META"/>
                    <NewsDisplay title={"Another red day on the market"} date="08-10-2022 08:32"
                                 content={"Market remains under pressure as..."}
                                 affectedStocks="AAPL, AMZN, MSFT, GOOGL, SHOP, TSLA, META"/>
                    <NewsDisplay title={"Another red day on the market"} date="08-10-2022 08:32"
                                 content={"Market remains under pressure as..."}
                                 affectedStocks="AAPL, AMZN, MSFT, GOOGL, SHOP, TSLA, META"/>
                    <NewsDisplay title={"Another red day on the market"} date="08-10-2022 08:32"
                                 content={"Market remains under pressure as..."}
                                 affectedStocks="AAPL, AMZN, MSFT, GOOGL, SHOP, TSLA, META"/>
                    <NewsDisplay title={"Another red day on the market"} date="08-10-2022 08:32"
                                 content={"Market remains under pressure as..."}
                                 affectedStocks="AAPL, AMZN, MSFT, GOOGL, SHOP, TSLA, META"/>
                </div>
            </div>
        </>
    )
}
export default NewsContainer