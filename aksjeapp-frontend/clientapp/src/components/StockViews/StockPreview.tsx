import React, {FC} from 'react';
import {colorHandler} from "./SingleStockView";

interface PropsObject {
    amount?: number;
    name: string;
    chart: string;
    difference: number;
    value: number;
}

type Props = {
    items: PropsObject;
    showAmount: boolean;
}

const StockPreview : FC<Props> = (props: Props) => {
    const redOrGreen = "py-1 text-center " + colorHandler(props.items.difference);
    const ifAmount = props.showAmount ? <p className="py-1 text-center">{props.items.amount}</p> : null

    const cols = "grid w-96 " + (props.showAmount ? "grid-cols-5" : "grid-cols-4")
    return (
        <div className={cols}>
            <p className="py-1 pl-4 text-center">{props.items.name}</p>
            {ifAmount}
            <p className="py-1 text-center">{props.items.chart}</p>
            <p className={redOrGreen}>{"+" + props.items.difference + "%"}</p>
            <p className="py-1 pr-4 text-center">{"$" + props.items.value}</p>
        </div>
    );
}

export default StockPreview;