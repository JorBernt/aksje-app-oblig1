import React, {useEffect, useState} from "react";
import Chart from "../UI/Chart/Chart";
import Card from "../UI/Card/Card";
import DataDisplay from "../UI/TextDisplay/DataDisplay";
import {API} from "../../Constants";
import LoadingSpinner from "../UI/LoadingSpinner";
import {MappableData, StockData} from "../models";

type Props = {
    symbol: string,
    fromDate: string,
    toDate: string;
    setStockPrice?: React.Dispatch<React.SetStateAction<number>>
}

export const colorHandler = (color: number | string | undefined) => {
    if (!color)
        return "text-black";
    return color === 0 ? "text-black" : color > 0 ? "text-green-500" : "text-red-500";
}

const SingleStockView: React.FC<Props> = (props) => {
    const [stockPrices, setStockPrices] = useState<MappableData[]>()
    const [stockData, setStockData] = useState<StockData>()
    const [max, setMax] = useState(0)
    const [min, setMin] = useState(0)
    const [loading, setLoading] = useState(true)
    const [name, setName] = useState("")
    const symbol = props.symbol
    const fromDate = props.fromDate
    const toDate = props.toDate
    const setStockPrice = props.setStockPrice
    useEffect(() => {
        setLoading(true)
        if (symbol === "")
            return
        const mappableStockPriceData: Array<MappableData> = []
        API.STOCK.GET_STOCK_PRICES(symbol, fromDate, toDate)
            .then((response) => {
                response.results.forEach(r => mappableStockPriceData.push({
                    name: r.date,
                    uv: r.closePrice
                }))
                setStockPrices(mappableStockPriceData)
                if (setStockPrice)
                    setStockPrice(response.last)
                setMax(Math.max(...mappableStockPriceData.map(d => d.uv)))
                setMin(Math.min(...mappableStockPriceData.map(d => d.uv)))
                setStockData(response)
                setLoading(false)
            })
            .catch((error: Error) => {
                console.log(error.message)
            })

        fetch(API.STOCK.GET_STOCK_NAME(symbol))
            .then(response => response.text()
                .then(text => setName(text)))
            .catch((error: Error) => {
                console.log(error.message)
                setName("Could not fetch name")
            })

    }, [fromDate, setStockPrice, symbol, toDate])

    return (
        <>
            <Card color={"default"} customCss="w-[42.5rem] m-5 p-5">
                <p className="m text-2xl text-center pb-5">{loading ? "Fetching name..." : name}</p>
                <Chart data={stockPrices} loading={loading} max={max} min={min}/>
                {loading &&
                    <>
                        <div className={"items-center flex justify-center h-[15.2rem]"}>
                            <LoadingSpinner/>
                        </div>
                    </>
                }
                {!loading &&
                    <>
                        <div className="grid grid-rows-3 grid-cols-3 mt-5 pt-10 h-[14rem]">
                            <DataDisplay title={"Symbol"} content={stockData?.name}></DataDisplay>
                            <DataDisplay title={"Last"} content={stockData?.last}></DataDisplay>
                            <DataDisplay title={"Today %"} content={stockData?.change.toFixed(2) + "%"}
                                         color={colorHandler(stockData?.change)}></DataDisplay>
                            <DataDisplay title={"Today +/-"} content={stockData?.todayDifference.toFixed(2)}
                                         color={colorHandler(stockData?.todayDifference)}></DataDisplay>
                            <DataDisplay title={"High"} content={stockData?.high}></DataDisplay>
                            <DataDisplay title={"Low"} content={stockData?.low}></DataDisplay>
                        </div>
                    </>
                }
            </Card>
        </>
    )
}

export default SingleStockView;