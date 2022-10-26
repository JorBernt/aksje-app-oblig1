import React, {useEffect, useRef, useState} from 'react';
import {CartesianGrid, Line, LineChart, Tooltip, XAxis, YAxis, TooltipProps} from "recharts";
import {API} from "../../../Constants";

type Props = {
    symbol: string;
    fromDate: string;
    toDate: string;
}

interface Results {
    closePrice: number;
    openPrice: number;
    highestPrice: number;
    lowestPrice: number;
}

interface StockPrice {
    symbol: string;
    results: Array<Results>;
}

interface MappableData {
    name: string;
    uv: number;
}

type ValueType = number | string | Array<number | string>;
type NameType = number | string;


const Chart: React.FC<Props> = (props) => {
    const [stockPrices, setStockPrices] = useState<MappableData[]>()
    const mappableStockPriceData: Array<MappableData> = []
    let weekNumber = 0;
    const [max, setMax] = useState(0)
    const [min, setMin] = useState(0)
    useEffect(() => {
        fetch(API.GET_STOCK_PRICES(props.symbol, props.fromDate, props.toDate))
            .then(response => response.json()
                .then((response: StockPrice) => {
                    response.results.map(r => r.closePrice).forEach(r => mappableStockPriceData.push({
                        name: (weekNumber++).toString(),
                        uv: r
                    }))
                    setStockPrices(mappableStockPriceData)
                    setMax(Math.max(...mappableStockPriceData.map(d => d.uv)))
                    setMin(Math.min(...mappableStockPriceData.map(d => d.uv)))
                }))
    },[])
    let item = ""
    const mouseEnterHandler = (key : string) => {
        item = key;
    };

    const CustomTooltip = ({
                               active,
                               payload,
                               label,
                           }: TooltipProps<ValueType, NameType>) => {
        if (active) {
            return (
                <div className="bg-white p-2 rounded-2xl">
                    <p className="label">{`Week ${label}`}</p>
                    <p>{`$${payload?.[0].value}`}</p>
                </div>
            );
        }

        return null;
    };


    return (
        <>
            <div className="bg-gradient-to-tl from-green-500 to-blue-700 p-5 rounded-2xl">
                <LineChart width={600} height={300} margin={{top: 5, right: 30, left: 0, bottom: 5}} data={stockPrices}>
                    <CartesianGrid stroke="#dbdbdb" />
                    <Tooltip content={<CustomTooltip/>}/>
                    <Line type="monotone" dataKey="uv" strokeWidth={2} stroke="#000000" />
                    <XAxis dataKey="name" stroke="black"/>
                    <YAxis stroke="black" type="number" domain={[Math.round(min * 0.9), Math.round(max * 1.1)]}/>
                </LineChart>
            </div>
        </>
    );
}

export default Chart;
