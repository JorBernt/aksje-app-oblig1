import NewsDisplay from "./UI/TextDisplay/NewsDisplay";
import React, {useEffect, useState} from "react";
import {API} from "../Constants";
import {News} from "./models";
import LoadingSpinner from "./UI/LoadingSpinner";

const NewsContainer = () => {

    const [news, setNews] = useState<News[]>([]);
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        fetch(API.GET_NEWS)
            .then(response => response.json()
                .then((res) => {
                    setNews(p => [...res.results])
                    setLoading(false)
                    console.log(res)
                }).catch(e => {
                    console.log(e.message)
                }))
    }, [])
    console.log(news[0])


    return (
        <>
            <div className="h-[32rem] w-[40rem]">
                <div className="flex justify-center text-stock-preview-text-1 pb-2">
                    <h1 className="font-bold">News and other events</h1>
                </div>
                <div className={"text-black px-5 grid grid-cols-4"}>
                    <p className="text-left pl-1 col-span-2">Content</p>
                    <p className="text-left pl-[5.5rem] col-span-2">Related stocks</p>
                </div>
                {loading &&
                    <div className="flex justify-center items-center h-96">
                        <LoadingSpinner/>
                    </div>
                }
                {!loading &&
                    <>
                        <hr className="border-text-display mb-1"/>
                        <div
                            className="scroll h-[28.5rem] overflow-y-auto pr-0.5 scrollbar scrollbar-track-white scrollbar-thumb-rounded-3xl scrollbar-thin scrollbar-thumb-gray-300 scrollbar">
                            {news.map((val) => {
                                return <NewsDisplay title={val.author} content={val.title} date={val.date}
                                                    affectedStocks={val.stocks} url={val.url}/>
                            })}
                        </div>
                    </>
                }
            </div>
        </>
    )
}
export default NewsContainer