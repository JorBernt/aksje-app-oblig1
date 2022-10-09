import React from "react";

const TransactionHistory = () => {
    return (
        <>
            <div>
                <h1 className="flex justify-center py-1 text-text-display">Transaction history</h1>
                <div>
                    <div className={"text-black px-5 grid grid-cols-2"}>
                        <p className="text-left">Time</p>
                        <p className="text-left">Price</p>
                        <p className="text-left">Amount</p>
                        <p className="text-left">Buyers</p>
                        <p className="text-left">Sellers</p>
                    </div>
                    <hr className="border-text-display"/>
                </div>
            </div>
        </>
    )
}
export default TransactionHistory;