import React from "react";

interface transaction {
    time: string;
    price: number;
    amount: number;
}

type Props = {
    transaction: transaction;
}

const TransactionPreview = (props: Props) => {

    return (
        <>
            <div className="grid w-96 grid-cols-4">
                <p className="py-1 pl-4 text-left col-span-2 text-sm pt-1.5">{props.transaction.time}</p>
                <p className="py-1 text-left">{props.transaction.price}</p>
                <p className="py-1 pr-4 text-left">{props.transaction.amount}</p>
            </div>
        </>
    )
}
export default TransactionPreview