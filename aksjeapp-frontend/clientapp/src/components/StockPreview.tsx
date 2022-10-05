import React, {FC} from 'react';

interface Props1 {
    id: number;
    name: string;
    chart: string;
    difference: number;
    value: number;
}

type Props = {
    items: Props1;
}

const StockPreview : FC<Props> = (props: Props) => {

    return(
        <div className="flex justify-between">
            <p className="px-5 py-1">{props.items.name}</p>
            <p className="px-5 py-1">{props.items.chart}</p>
            <p className="px-5 py-1">{props.items.difference}</p>
            <p className="px-5 py-1">{props.items.value}</p>
        </div>
    );
}

export default StockPreview;