import React, {useRef, useState} from 'react';
import {Stock} from '../../StockViews/StockContainer'

type Props = {}

const SearchBar: React.FC<Props> = () => {
    const [hidden, setHidden] = useState(true)
    const input = useRef<HTMLInputElement>(null)
    const [searchResult, setSearchResult] = useState(Array<Stock>)

    const search = () => {
        let query = input.current?.value;
        if (query !== "") {
            setHidden(false)
        } else setHidden(true)
        fetch(`https://localhost:7187/Stock/SearchResults/?keyPhrase=${query}`)
            .then(response => response.json()
                .then(response => setSearchResult(() => [...response]))
            )
    }

    return (
        <>
            <div
                className="flex justify-between bg-white rounded-2xl shadow-inner shadow-gray-400 overflow-hidden">
                <input className="w-80 bg-transparent focus:border focus:border-pink-500 rounded-l-2xl pl-4"
                       style={{outline: "none"}} type="text" placeholder="Search stocks..." ref={input}
                       onChange={search}/>
                <button
                    className="bg-transparent bg-gray-300 hover:bg-gradient-to-tl hover:from-gradient-start hover:to-gradient-end text-black font-semibold py-2 px-4 hover:text-white transition duration-300 ease-in-out">
                    Search
                </button>
                <div
                    className={"absolute top-16 z-10 origin-bottom rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none w-80 px-5 scroll overflow-y-auto max-h-96 scrollbar scrollbar-track-white scrollbar-thumb-rounded-3xl scrollbar-thin scrollbar-thumb-blue-700 " + (hidden ? "hidden" : "")}
                    role="menu" aria-orientation="vertical" aria-labelledby="menu-button">
                    {searchResult.length !== 0 &&
                        <div className="py-1" role="none">
                            {searchResult.map(data => {
                                return (
                                    <div
                                        className={"flex flex-row justify-between hover:bg-gradient-to-br hover:from-white hover:to-gray-200 hover:rounded-lg transition duration-150 ease-in-out group hover:animate-pulse"}>
                                        <a href="#"
                                           className="text-gray-700 block px-4 py-2 text-sm hover:scale-105 truncate w-64 group-hover:font-bold"
                                           role="menuitem"
                                           id="menu-item-0">{data.name}</a>
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
                            No result for "{input.current?.value}"
                        </div>
                    }
                </div>
            </div>


        </>
    );
}

export default SearchBar;