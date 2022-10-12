import React from 'react';
import {CartesianGrid, Line, LineChart, XAxis, YAxis} from "recharts";

const Chart = () => {
    const data = [
        {name: '1', uv: 10},
        {name: '2', uv: 500},
        {name: '3', uv: 400},
        {name: '1', uv: 10},
        {name: '2', uv: 500},
        {name: '3', uv: 400},
        {name: '1', uv: 10},
        {name: '2', uv: 500},
        {name: '3', uv: 400},
        {name: '1', uv: 10},
        {name: '2', uv: 500},
        {name: '3', uv: 400}];
    return (
        <>
            <div className="bg-gradient-to-tl from-green-500 to-blue-700 p-5 rounded-2xl">
                <LineChart width={600} height={300} data={data}>
                    <Line type="monotone" dataKey="uv" stroke="#000000" dot={false}/>
                    <CartesianGrid stroke="#ccc"/>
                    <XAxis dataKey="name" stroke="black"/>
                    <YAxis stroke="black"/>
                </LineChart>
            </div>
        </>
    );
}

export default Chart;