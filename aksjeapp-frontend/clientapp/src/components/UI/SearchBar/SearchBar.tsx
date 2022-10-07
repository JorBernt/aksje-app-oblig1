import React from 'react';

type Props = {

}

const SearchBar: React.FC<Props> = () => {
    return (
        <>
            <div
                className="flex justify-between bg-white rounded-2xl drop-shadow-2xl shadow-inner pl-5 overflow-hidden">
                <input className="w-80" style={{outline: "none"}} type="text"/>
                <button
                    className="bg-transparent hover:bg-gray-300 bg-gradient-to-tl from-purple-700 to-pink-500 text-black font-semibold py-2 px-4 hover:text-black transition duration-300 ease-in-out">
                    Search
                </button>
            </div>

        </>
    );
}

export default SearchBar;