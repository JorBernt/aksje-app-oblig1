import React from 'react';
import {CartesianGrid, Line, LineChart, XAxis, YAxis} from "recharts";

const Chart = () => {
    const data = [{name: '1', uv: 10, pv: 2400, amt: 2400},{name: '2', uv: 500, pv: 1500, amt: 2700},{name: '3', uv: 400, pv: 1800, amt: 1200}];
    return (
        <>
            <div className="bg-gray-800 p-5 rounded-2xl">
                <LineChart width={600} height={300} data={data}>
                    <Line type="monotone" dataKey="uv" stroke="#8884d8" />
                    <CartesianGrid stroke="#ccc" />
                    <XAxis dataKey="name" />
                    <YAxis />
                </LineChart>
            </div>
        </>
    );
}

export default Chart;