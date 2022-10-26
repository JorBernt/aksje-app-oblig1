import React, {useEffect, useState} from "react";
import {useNavigate} from "react-router-dom";
import {API} from "../../Constants";
import {Transaction} from "../models";


const TransactionsView = () => {

    const data1 = [
        {name: "Apple", amount: 52, buyPrice: "30", date: "2022-26-10"},
        {name: "Amazon", amount: 100, buyPrice: "100", date: "2022-26-10"},
        {name: "Tesla", amount: 35, buyPrice: "30", date: "2022-26-10"},
        {name: "Telenor", amount: 35, buyPrice: "30", date: "2022-26-10"},
        {name: "Tinder", amount: 35, buyPrice: "30", date: "2022-26-10"},
        {name: "Meta", amount: 35, buyPrice: "30", date: "2022-26-10"}
    ]

    let data : Transaction[] = [{id: 1, socialSecurityNumber: "string", date: "string", symbol: "string", amount: 12, totalPrice: 45, isActive: true, awaiting: false}];
    const [transactionView, setTransactionView] = useState(data);
    let navigate = useNavigate();
    const handleOnClick = () => {
        navigate(`/singleStock?symbol=${"AAA"}&name=${"ADDA"}`);
    }

    //const isVisible = true;
    useEffect(() =>{
        fetch(API.GET_ALL_TRANSACTIONS("12345678910"))
            .then(response => response.json()
                .then(response => {
                    setTransactionView(p => [...response])
                }).catch(e => {
                    console.log(e.message)
                })
            )
    })

    return (
        <>
            <div className="mt-2 flex flex-col">
                <div className="-my-2 overflow-x-auto -mx-4 sm:-mx-6 lg:-mx-8">
                    <div className="py-2 align-middle inline-block min-w-full sm:px-6 lg:px-8">
                        <div className="shadow overflow-hidden border-b border-gray-200 sm:rounded-lg">
                            <table className="min-w-full divide-y divide-gray-200">
                                <thead>
                                <tr>
                                    <th className="px-6 py-2 text-xl text-black-500">Navn</th>
                                    <th className="px-6 py-2 text-xl text-black-500">Antall</th>
                                    <th className="px-6 py-2 text-xl text-black-500">Kjøpsris</th>
                                    <th className="px-6 py-2 text-xl text-black-500">Dato</th>
                                </tr>
                                </thead>
                                {transactionView.map((val) => {
                                    return (
                                        <tbody>
                                        <tr>
                                            <td className="px-6 py-4 whitespace-nowrap justify-center">{val.symbol}</td>
                                            <td className="px-6 py-4 whitespace-nowrap justify-center">{val.amount}</td>
                                            <td className="px-6 py-4 whitespace-nowrap justify-center">{val.totalPrice}</td>
                                            <td className="px-6 py-4 whitespace-nowrap justify-center">{val.date}</td>
                                            <td className="px-6 py-4 whitespace-nowrap justify-center">
                                                <button
                                                    className="text-white font-bold py-2 px-4 rounded-full m-4 hover:shadow-xl w-40 transition duration-300 bg-green-500 hover:shadow-green-300"
                                                    onClick={handleOnClick} style={{visibility: val.awaiting ? 'visible' : 'hidden'}}>Buy
                                                </button>
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap">
                                                <button
                                                    className="text-white font-bold py-2 px-4 rounded-full m-4 hover:shadow-xl w-40 transition duration-300 bg-red-500 hover:shadow-red-300"
                                                    onClick={handleOnClick} style={{visibility: val.awaiting ? 'visible' : 'hidden'}}>Sell
                                                </button>
                                            </td>
                                        </tr>
                                        </tbody>
                                    )
                                })}
                            </table>
                        </div>
                    </div>
                </div>
            </div>

        </>
    );
};


export default TransactionsView;