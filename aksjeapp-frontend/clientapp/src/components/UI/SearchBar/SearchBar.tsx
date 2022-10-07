import React from 'react';

type Props = {

}

const SearchBar: React.FC<Props> = () => {
    return (
        <>
            <div
                className="flex justify-between bg-white rounded-2xl shadow-inner shadow-gray-400 overflow-hidden">
                <input className="w-80 bg-transparent focus:border focus:border-pink-500 rounded-l-2xl pl-4"
                       style={{outline: "none"}} type="text"/>
                <button
                    className="bg-transparent bg-gray-300 hover:bg-gradient-to-tl hover:from-gradient-start hover:to-gradient-end text-black font-semibold py-2 px-4 hover:text-white transition duration-300 ease-in-out">
                    Search
                </button>
            </div>
        </>
    );
}

export default SearchBar;