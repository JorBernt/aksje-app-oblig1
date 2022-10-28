import React, {useEffect, useRef, useState} from "react";
import {useNavigate} from "react-router-dom";
import {API, SSN} from "../../Constants";
import {Transaction} from "../models";
import {Simulate} from "react-dom/test-utils";
import input = Simulate.input;


const TransactionsView = () => {


    let data: Transaction[] = [{
        id: 1,
        socialSecurityNumber: "string",
        date: "string",
        symbol: "string",
        amount: 12,
        totalPrice: 45,
        isActive: true,
        awaiting: false
    }];
    const [transactionView, setTransactionView] = useState(data);
    const [editButton, setEditButton] = useState("Edit")
    const [editAmount, setEditAmount] = useState(false)
    const inputRef = useRef<HTMLInputElement>(null);

    //const isVisible = true;
    useEffect(() => {
        fetch(API.GET_ALL_TRANSACTIONS("12345678910"))
            .then(response => response.json()
                .then(response => {
                    setTransactionView(p => [...response])
                }).catch(e => {
                    console.log(e.message)
                })
            )
    },[])
    const handleOnClickDelete = (id: number) => {
        fetch(API.DELETE_TRANSACTION(SSN, id))

    }
    const handleOnClickUpdate = (transaction: Transaction) => {


        if (!editAmount) {
            setEditAmount(true)
            setEditButton("Save")
        } else {
            if (inputRef.current)
                 transaction.amount = inputRef.current.valueAsNumber
            fetch(API.UPDATE_TRANSACTION, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "access-control-allow-origin": "",
                    'Access-Control-Allow-Headers': 'Content-Type, Authorization',
                    'Access-Control-Allow-Methods': '',
                },
                body: JSON.stringify(transaction)

            })
            setEditAmount(false)
            setEditButton("Edit")
            console.log(transaction)

        }
    }
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
                                            {
                                                editAmount ?
                                                    <td className="px-6 py-4 whitespace-nowrap justify-center mt-6">
                                                        <input
                                                            className="w-20 bg-gray-200 px-2 rounded-xl"
                                                            type="number" ref={inputRef}/></td> :
                                                    <td className="px-6 py-4 whitespace-nowrap justify-center"> {val.amount}</td>
                                            }
                                            <td className="px-6 py-4 whitespace-nowrap justify-center">{val.totalPrice}</td>
                                            <td className="px-6 py-4 whitespace-nowrap justify-center">{val.date}</td>
                                            <td className="px-6 py-4 whitespace-nowrap justify-center">
                                                <button
                                                    className="text-white font-bold py-2 px-4 rounded-full m-4 hover:shadow-xl w-40 transition duration-300 bg-yellow-500 hover:shadow-yellow-300"
                                                    onClick={() => handleOnClickUpdate(val)}
                                                    style={{visibility: val.awaiting ? 'visible' : 'hidden'}}>{editButton}
                                                </button>
                                            </td>
                                            <td className="px-6 py-4 whitespace-nowrap">
                                                <button
                                                    className="text-white font-bold py-2 px-4 rounded-full m-4 hover:shadow-xl w-40 transition duration-300 bg-red-500 hover:shadow-red-500"
                                                    onClick={() => handleOnClickDelete(val.id)}
                                                    style={{visibility: val.awaiting ? 'visible' : 'hidden'}}>Slett
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
    )
        ;
};


export default TransactionsView;