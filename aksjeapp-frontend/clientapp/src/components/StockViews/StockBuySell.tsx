import React from "react";


const StockBuySell = () => {

    return (
        <>

            <div className="flex flex-col">
                <div className="flex flex-col justify-center">
                    <p className="flex flex-row justify-center text-2xl">Antall</p>
                    <input
                        className="w-200 bg-transparent focus:border focus:border-pink-500 rounded pl-4 border border-black"
                        type="number"/>
                </div>
                <div className="flex flex-row justify-center">
                    <button
                        className="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded-full m-4 hover:shadow-xl hover:shadow-blue-300 w-40 transition duration-300">Buy
                    </button>
                    <button
                        className="bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 rounded-full m-4 hover:shadow-xl hover:shadow-red-300 w-40 transition duration-300">Sell
                    </button>
                </div>

            </div>

        </>
    );

};

export default StockBuySell;
