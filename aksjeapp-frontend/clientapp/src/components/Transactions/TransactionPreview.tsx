import React from "react";
import {Transaction} from "../models";


type Props = {
    transaction: Transaction
}

const TransactionPreview = (props: Props) => {
    return (
        <>
            <div className="grid w-96 grid-cols-4">
                <p className="py-1 pl-4 text-left col-span-2 text-sm pt-1.5">{props.transaction.date}</p>
                <p className="py-1 text-left">{props.transaction.totalPrice}</p>
                <p className="py-1 pr-4 text-left">{props.transaction.amount}</p>
            </div>
        </>
    )
}
export default TransactionPreview