import React from 'react';
import {CartesianGrid, Line, LineChart, XAxis, YAxis} from "recharts";

const Chart = () => {
    const data = [
        {name: '1', uv: 10, pv: 2400, amt: 2400},
        {name: '2', uv: 500, pv: 1500, amt: 2700},
        {name: '3', uv: 400, pv: 1800, amt: 1200},
        {name: '1', uv: 10, pv: 2400, amt: 2400},
        {name: '2', uv: 500, pv: 1500, amt: 2700},
        {name: '3', uv: 400, pv: 1800, amt: 1200},
        {name: '1', uv: 10, pv: 2400, amt: 2400},
        {name: '2', uv: 500, pv: 1500, amt: 2700},
        {name: '3', uv: 400, pv: 1800, amt: 1200},
        {name: '1', uv: 10, pv: 2400, amt: 2400},
        {name: '2', uv: 500, pv: 1500, amt: 2700},
        {name: '3', uv: 400, pv: 1800, amt: 1200}];
    return (
        <>
            <div className="bg-gradient-to-tl from-green-500 to-blue-700 p-5 rounded-2xl">
                <LineChart width={600} height={300} data={data}>
                    <Line type="monotone" dataKey="uv" stroke="#000000"/>
                    <CartesianGrid stroke="#ccc"/>
                    <XAxis dataKey="name" stroke="black"/>
                    <YAxis stroke="black"/>
                </LineChart>
            </div>
        </>
    );
}

export default Chart;