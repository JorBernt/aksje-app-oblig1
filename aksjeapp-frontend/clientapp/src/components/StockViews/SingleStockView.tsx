import React from "react";
import Chart from "../UI/Chart/Chart";
import Card from "../UI/Card/Card";

type Props = {
    stockData: { [key: string]: string | number }
    rowNames : Map<string, string>
}


const SingleStockView: React.FC<Props> = (props) => {
    const stockData = props.stockData;
    const rowNames : string[] = [];
    const rowData : any[] = [];
    props.rowNames.forEach((key, value) => {
        rowData.push(stockData[key])
        rowNames.push(value)
    })
    return (
        <>
            <Card color={"default"} customCss="w-max">
                <p className="text-5xl text-center pb-5">{stockData.name}</p>
                <Chart/>
                <div className="grid grid-rows-2 grid-cols-8 mt-5 ">
                    {rowNames.map(value => {
                        return(
                            <>
                                <p className="text-2xl">{value}</p>
                            </>
                        )
                    })}
                    {rowData.map(value => {
                        return(
                            <>
                                <p className="text-2xl">{value}</p>
                            </>
                        )
                    })}
                </div>
            </Card>
        </>
    )
}

export default SingleStockView;