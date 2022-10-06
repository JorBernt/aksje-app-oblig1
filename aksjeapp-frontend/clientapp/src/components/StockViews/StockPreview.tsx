import React, {FC} from 'react';

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
    const redOrGreen = props.items.difference > 0 ?
        <p className="py-1 text-green-500 text-center">{"+" + props.items.difference + "%"}</p> :
        <p className="py-1 text-red-500 text-center">{props.items.difference + "%"}</p>
    const ifAmount = props.showAmount ? <p className="py-1 text-center">{props.items.amount}</p> : null

    const cols = "grid w-96 " + (props.showAmount ? "grid-cols-5" : "grid-cols-4")
    return (
        <div className={cols}>
            <p className="py-1 pl-4 text-center">{props.items.name}</p>
            {ifAmount}
            <p className="py-1 text-center">{props.items.chart}</p>
            {redOrGreen}
            <p className="py-1 pr-4 text-center">{"$" + props.items.value}</p>
        </div>
    );
}

export default StockPreview;