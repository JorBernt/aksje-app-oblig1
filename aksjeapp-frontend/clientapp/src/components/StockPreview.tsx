import React, {FC} from 'react';

interface PropsObject {
    id: number;
    name: string;
    chart: string;
    difference: number;
    value: number;
}

type Props = {
    items: PropsObject;
}

const StockPreview : FC<Props> = (props: Props) => {

    return(
        <div className="grid grid-cols-4 w-96">
            <p className="py-1 pl-4">{props.items.name}</p>
            <p className="py-1">{props.items.chart}</p>
            <p className="py-1">{props.items.difference}</p>
            <p className="py-1 pr-4">{props.items.value}</p>
        </div>
    );
}

export default StockPreview;