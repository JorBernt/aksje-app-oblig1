import React from 'react';

type Props = {

}

const SearchBar: React.FC<Props> = (props) => {
    return (
        <>
            <div className="flex justify-between rounded-2xl drop-shadow-2xl bg-gray-50 shadow-inner h-16 pl-5 p-2">
                <input className="w-80" style={{outline:"none"}} type="text"/>
                <button className="bg-transparent hover:bg-blue-500 text-blue-700 font-semibold hover:text-white py-2 px-4 border border-blue-500 hover:border-transparent rounded-2xl">
                    Search</button>
            </div>

        </>
    );
}

export default SearchBar;