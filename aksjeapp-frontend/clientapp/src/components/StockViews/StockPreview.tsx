import React, {FC} from 'react';
import {Stock} from "../models";
import {useNavigate} from "react-router-dom";

type Props = {
    items: Stock;
    showAmount: boolean;
}

const StockPreview : FC<Props> = (props: Props) => {

    let navigate = useNavigate();
    const handleOnClick = () => {
        navigate(`/singleStock?symbol=${props.items.symbol}&name=${props.items.name}`)
    }

    const className = "grid w-96 " + (props.showAmount ? "grid-cols-5" : "grid-cols-4")
    return (
        <>
            <div className={className} onClick={handleOnClick} title={props.items.name}>
                <p className="py-1 pl-4 text-center">{props.items.symbol}</p>
            </div>

        </>
    );
}

export default StockPreview;