import React, {useEffect, useState} from "react";
import Chart from "../UI/Chart/Chart";
import Card from "../UI/Card/Card";
import DataDisplay from "../UI/TextDisplay/DataDisplay";
import {API} from "../../Constants";
import LoadingSpinner from "../UI/LoadingSpinner";


interface Results {
    closePrice: number;
    openPrice: number;
    highestPrice: number;
    lowestPrice: number;
    date: string;
}

export interface MappableData {
    name: string;
    uv: number;
}

interface StockData {
    name: string
    last: number,
    change: number,
    todayDifference: number,
    buy: number,
    sell: number,
    high: number,
    low: number,
    turnover: number,
    symbol: string
    results: Array<Results>
}

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
    const mappableStockPriceData: Array<MappableData> = []
    useEffect(() => {
        setLoading(true)
        fetch(API.GET_STOCK_PRICES(props.symbol, props.fromDate, props.toDate))
            .then(response => response.json()
                .then((response: StockData) => {
                    response.results.forEach(r => mappableStockPriceData.push({
                        name: r.date,
                        uv: r.closePrice
                    }))
                    setStockPrices(mappableStockPriceData)
                    if (props.setStockPrice)
                        props.setStockPrice(response.last)
                    setMax(Math.max(...mappableStockPriceData.map(d => d.uv)))
                    setMin(Math.min(...mappableStockPriceData.map(d => d.uv)))
                    console.log(response)
                    setStockData(response)
                    setLoading(false)
                }))
        fetch(API.GET_STOCK_NAME(props.symbol)).then(response => response.text().then(text => setName(text)))

    }, [props.symbol])

    return (
        <>
            <Card color={"default"} customCss="w-[42.5rem] m-5 p-5">
                <p className="m text-2xl text-center pb-5">{name}</p>
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
                        <div className="grid grid-rows-3 grid-cols-3 mt-5 h-[14rem]">
                            <DataDisplay title={"Symbol"} content={stockData?.name}></DataDisplay>
                            <DataDisplay title={"Last"} content={stockData?.last}></DataDisplay>
                            <DataDisplay title={"Today %"} content={stockData?.change.toFixed(2) + "%"}
                                         color={colorHandler(stockData?.change)}></DataDisplay>
                            <DataDisplay title={"Today +/-"} content={stockData?.todayDifference.toFixed(2)}
                                         color={colorHandler(stockData?.todayDifference)}></DataDisplay>
                            <DataDisplay title={"Buy"} content={"TBA"}></DataDisplay>
                            <DataDisplay title={"Sell"} content={"TBA"}></DataDisplay>
                            <DataDisplay title={"High"} content={stockData?.high}></DataDisplay>
                            <DataDisplay title={"Low"} content={stockData?.low}></DataDisplay>
                            <DataDisplay title={"Turnover"} content={stockData?.turnover}></DataDisplay>
                        </div>
                    </>
                }
            </Card>
        </>
    )
}

export default SingleStockView;