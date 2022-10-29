import React, {useEffect, useRef, useState} from "react";
import {API, SSN} from "../../Constants";
import {Transaction} from "../models";
import {Simulate} from "react-dom/test-utils";
import Card from "../UI/Card/Card";
import input = Simulate.input;

type Props = {
    callBack: any;
}

const TransactionsView: React.FC<Props> = (props) => {

    const [transactionView, setTransactionView] = useState<Transaction[]>([]);
    const inputRef = useRef<HTMLInputElement>(null);
    const [value, setValue] = useState(0)
    const [editTransactionArray, setEditTransactionArray] = useState<boolean[]>([])
    //const isVisible = true;

    const loadAllTransactions = () => {
        fetch(API.GET_ALL_TRANSACTIONS("12345678910"))
            .then(response => response.json()
                .then(response => {
                    setTransactionView(p => [...response])
                    for (let i = 0; i < transactionView.length; i++) {
                        editTransactionArray[transactionView[i].id] = false;
                    }
                    setEditTransactionArray(editTransactionArray)
                }).catch(e => {
                    setTransactionView([])
                    console.log(e.message)
                })
            )
    }
    useEffect(() => {
        loadAllTransactions()
    }, [])
    const handleOnClickDelete = (id: number) => {
        fetch(API.DELETE_TRANSACTION(SSN, id))
            .then(result => {
                loadAllTransactions()
                props.callBack();
            })
    }
    const handleOnClickUpdate = (transaction: Transaction) => {
        if (!editTransactionArray[transaction.id]) {
            setValue(transaction.amount)
            const t = [...editTransactionArray];
            t[transaction.id] = true;
            setEditTransactionArray(t)
        } else {
            if (inputRef.current)
                transaction.amount = inputRef.current.valueAsNumber
            const t = [...editTransactionArray];
            t[transaction.id] = false;
            setEditTransactionArray(t)
            fetch(API.UPDATE_TRANSACTION, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "access-control-allow-origin": "",
                    'Access-Control-Allow-Headers': 'Content-Type, Authorization',
                    'Access-Control-Allow-Methods': '',
                },
                body: JSON.stringify(transaction)

            }).then(result => props.callBack())
        }
    }

    const handleInputOnChange = (e: any) => {
        setValue(e.target.value)
    }
    return (
        <>
            {transactionView.length > 0 &&
                <Card customCss={"w-[53rem] p-5 m-5"}>
                    <div className="flex flex-col justify-between">
                        <h3 className="text-center font-semi text-3xl bold py-4">Your latest transactions</h3>
                        <hr className="border-text-display "/>
                        <div className="mt-2 flex flex-col">
                            <div className="-my-2 overflow-x-auto -mx-4 sm:-mx-6 lg:-mx-8">
                                <div className="py-2 align-middle inline-block min-w-full sm:px-6 lg:px-8">
                                    <div className="shadow overflow-hidden border-b border-gray-200 sm:rounded-lg">
                                        <table className="min-w-full divide-y divide-gray-200">
                                            <thead>
                                            <tr>
                                                <th className="px-6 py-2 text-xl text-black-500">Name</th>
                                                <th className="px-6 py-2 text-xl text-black-500">Amount</th>
                                                <th className="px-6 py-2 text-xl text-black-500">Date</th>
                                                <th className="px-6 py-2 text-xl text-black-500">Buy Price $</th>
                                            </tr>
                                            </thead>

                                            {transactionView.map((val) => {
                                                return (

                                                    <tbody>
                                                    <tr className={"text-center"} key={val.id}>
                                                        <td className="px-6 py-4 whitespace-nowrap ">{val.symbol}</td>
                                                        {
                                                            editTransactionArray[val.id] ?
                                                                <td className="px-6 py-4 whitespace-nowrap mt-6 w-12">
                                                                    <input
                                                                        className="w-[4.55rem] bg-gray-200 pl-[1.2rem] pr-1 rounded-xl text-center"
                                                                        type="number" ref={inputRef} value={value}
                                                                        onChange={handleInputOnChange}/></td> :
                                                                <td className="px-6 py-4 whitespace-nowrap"> {val.amount}</td>
                                                        }
                                                        <td className="px-6 py-4 whitespace-nowrap">{val.totalPrice}</td>
                                                        <td className="px-6 py-4 whitespace-nowrap">{val.date}</td>
                                                        {val.awaiting &&
                                                            <>
                                                                <td className="px-6 py-4 whitespace-nowrap">
                                                                    {!editTransactionArray[val.id] ?
                                                                        <>
                                                                            <button id={String(val.id)}
                                                                                    className="text-white font-bold py-2 rounded-full hover:shadow-xl w-24 transition duration-300 bg-yellow-500 hover:shadow-yellow-300"
                                                                                    onClick={() => handleOnClickUpdate(val)}
                                                                            >{"Edit"}
                                                                            </button>
                                                                        </>
                                                                        :
                                                                        <>
                                                                            <button id={String(val.id)}
                                                                                    className="text-white font-bold py-2 rounded-full hover:shadow-xl w-24 transition duration-300 bg-green-500 hover:shadow-green-500"
                                                                                    onClick={() => handleOnClickUpdate(val)}
                                                                            >{"Save"}
                                                                            </button>
                                                                        </>
                                                                    }
                                                                </td>
                                                                <td className="py-4 whitespace-nowrap">
                                                                    <button
                                                                        className="text-white font-bold py-2 rounded-full hover:shadow-xl w-24 transition duration-300 bg-red-500 hover:shadow-red-500"
                                                                        onClick={() => handleOnClickDelete(val.id)}>
                                                                        Delete
                                                                    </button>
                                                                </td>
                                                            </>
                                                        }
                                                    </tr>
                                                    </tbody>
                                                )
                                            })}
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </Card>
            }
        </>
    )
        ;
};


export default TransactionsView;