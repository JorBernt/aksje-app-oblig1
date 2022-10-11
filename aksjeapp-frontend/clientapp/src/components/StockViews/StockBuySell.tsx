import React from "react";

type Props = {
    data: string;
    className: string
};

const StockBuySell = (props: Props) => {

    const className = "hover:bg-red-700 text-white font-bold py-2 px-4 rounded-full m-4 hover:shadow-xl hover:shadow-red-300 w-40 transition duration-300 " + props.className;

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
                        className={className}>{props.data}
                    </button>
                </div>

            </div>

        </>
    );

};

export default StockBuySell;
