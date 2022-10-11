import React, {FC} from 'react';
import {colorHandler} from "./SingleStockView";
import {Stock} from "./StockContainer";

interface PropsObject {
    amount?: number;
    name: string;
    chart: string;
    difference: number;
    value: number;
}

type Props = {
    items: Stock;
    showAmount: boolean;
}

const StockPreview : FC<Props> = (props: Props) => {


    const cols = "grid w-96 " + (props.showAmount ? "grid-cols-5" : "grid-cols-4")
    return (
        <div className={cols}>
            <p className="py-1 pl-4 text-center">{props.items.symbol}</p>
        </div>
    );
}

export default StockPreview;