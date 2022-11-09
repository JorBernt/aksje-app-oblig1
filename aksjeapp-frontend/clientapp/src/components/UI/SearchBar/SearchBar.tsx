import React, {useRef, useState} from 'react';
import {Stock} from "../../models";
import {API} from "../../../Constants";
import {useNavigate} from "react-router-dom";

type Props = {}

const SearchBar: React.FC<Props> = () => {
    const [hidden, setHidden] = useState(true)
    const inputRef = useRef<HTMLInputElement>(null)
    const [input, setInput] = useState("")
    const [searchResult, setSearchResult] = useState(Array<Stock>)

    let navigate = useNavigate();
    const handleOnClick = (data: Stock) => {
        setSearchResult([])
        setInput("")
        if (inputRef.current != null)
            inputRef.current.value = ""
        search()
        navigate(`/singleStock?symbol=${data.symbol}&name=${data.name}`)
    }

    const search = () => {
        let query = inputRef.current?.value;
        if (query !== "") {
            setHidden(false)
        } else setHidden(true)
        if (typeof query === "undefined")
            return
        setInput(query)
        fetch(API.STOCK.SEARCH_RESULTS(query))
            .then(response => response.json()
                .then(response => setSearchResult(() => [...response]))
            )
    }
    return (
        <>
            <div
                className="flex justify-between bg-white rounded-2xl shadow-inner shadow-gray-400 overflow-hidden ">
                <input className="w-[32rem] bg-transparent focus:border focus:border-pink-500 rounded-l-2xl pl-4"
                       style={{outline: "none"}} type="text" placeholder="Search stocks..." ref={inputRef}
                       onChange={search}/>
                <button
                    className="bg-transparent bg-gray-300 hover:bg-gradient-to-tl hover:from-gradient-start hover:to-gradient-end text-black font-semibold py-2 px-4 hover:text-white transition duration-300 ease-in-out">
                    Search
                </button>
                <div
                    className={"absolute top-16 z-10 origin-bottom rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none w-[32rem] px-5 scroll overflow-y-auto max-h-96 scrollbar scrollbar-track-white scrollbar-thumb-rounded-3xl scrollbar-thin scrollbar-thumb-blue-700 " + (hidden ? "hidden" : "")}
                    role="menu" aria-orientation="vertical" aria-labelledby="menu-button">
                    {searchResult.length !== 0 &&
                        <div className="py-1" role="none">
                            {searchResult.map(data => {
                                return (
                                    <div
                                        className={"cursor-pointer flex flex-row justify-between hover:bg-gradient-to-br hover:from-white hover:to-gray-200 hover:rounded-lg transition duration-150 ease-in-out group hover:animate-pulse"}>
                                        <div onClick={() => handleOnClick(data)}
                                             className="text-gray-700 block px-4 py-2 text-sm hover:scale-105 truncate w-96 group-hover:font-bold"
                                             role="menuitem"
                                             id="menu-item-0">{data.name}
                                        </div>
                                        <a href="#"
                                           className=" block px-4 py-2 text-sm text-green-500 font-bold group-hover:text-blue-700 group-hover:animate-spin"
                                           role="menuitem"
                                           id="menu-item-0">{data.symbol}</a>
                                    </div>
                                )
                            })}
                        </div>
                    }
                    {searchResult.length === 0 &&
                        <div className="p-5 truncate w-64">
                            No result for "{input}"
                        </div>
                    }
                </div>
            </div>


        </>
    );
}

export default SearchBar;