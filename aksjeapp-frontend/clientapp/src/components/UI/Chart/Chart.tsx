import React from 'react';
import {CartesianGrid, Line, LineChart, Tooltip, TooltipProps, XAxis, YAxis} from "recharts";
import LoadingSpinner from "../LoadingSpinner";
import {MappableData} from "../../StockViews/SingleStockView";

type Props = {
    data?: Array<MappableData>,
    min: number,
    max: number,
    loading: boolean;
}

type ValueType = number | string | Array<number | string>;
type NameType = number | string;

const Chart: React.FC<Props> = (props) => {
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
            <div className="bg-gradient-to-tl from-green-500 to-blue-700 p-5 rounded-2xl w-[40rem] h-[21rem]">
                {props.loading &&
                    <>
                        <div className="ml-28">
                            <LoadingSpinner/>
                        </div>
                    </>
                }
                {!props.loading &&
                    <>

                        <LineChart width={600} height={300} margin={{top: 5, right: 30, left: 0, bottom: 5}}
                                   data={props.data}>
                            <CartesianGrid stroke="#dbdbdb"/>
                            <Tooltip content={<CustomTooltip/>}/>
                            <Line type="monotone" dataKey="uv" strokeWidth={2} stroke="#000000"/>
                            <XAxis dataKey="name" stroke="black"/>
                            <YAxis stroke="black" type="number"
                                   domain={[Math.round(props.min * 0.9), Math.round(props.max * 1.1)]}/>
                        </LineChart>
                    </>
                }
            </div>

        </>
    );
}

export default Chart;
