import React, {FC} from 'react';
import {Stock} from "../models";
import {useNavigate} from "react-router-dom";
import {colorHandler} from "./SingleStockView";

type Props = {
    items: Stock;
    showAmount: boolean;
}

const StockPreview : FC<Props> = (props: Props) => {

    let navigate = useNavigate();
    const handleOnClick = () => {
        navigate(`/singleStock?symbol=${props.items.symbol}`)
    }

    const changeValue = Math.round(props.items.change * 100) / 100;

    const textColor = colorHandler(changeValue)

    const className = "grid w-96 " + (props.showAmount ? "grid-cols-5" : "grid-cols-4")
    return (
        <>
            <div className={className} onClick={handleOnClick} title={props.items.name}>
                <p className="py-1 pl-4 text-center">{props.items.symbol}</p>
                {props.showAmount &&
                    <p className="py-1 pl-4 text-center">{props.items.amount}</p>
                }
                <p className="py-1 text-center">{"TBA"}</p>
                <p className={"py-1 text-center " + textColor}>{`${changeValue}`}</p>
                <p className="py-1 pr-4 text-center">{`${props.items.value}`}</p>
            </div>

        </>
    );
}

export default StockPreview;