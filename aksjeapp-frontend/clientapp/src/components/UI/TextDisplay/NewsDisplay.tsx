import {useNavigate} from "react-router-dom";

type Props = {
    title: string;
    content: string | number;
    date: string;
    affectedStocks: string[];
    url: string;
}

const NewsDisplay = (props: Props) => {
    const className = "pt-2.5 pb-2.5 text-text-display grid grid-cols-3 bg-gray-300 px-5 mb-5 rounded-xl mx-2"
    let navigate = useNavigate();
    const handleOnClick = (symbol: string) => {
        navigate(`/singleStock?symbol=${symbol}`)
    }
    let items = 0;
    return (
        <>
            <div className={className}>
                <div className="pr-5 col-span-2">
                    <p className="text-xs">{props.date}</p>
                    <a className="text-left font-semibold text-xl hover:underline" target="_blank"
                       href={props.url}>{props.content}</a>
                    <p className="text-left">{props.title}</p>
                </div>
                <div className="w-44 col-span-1">
                    <div className="flex flex-row flex-wrap">
                        {props.affectedStocks.map((stock, index) => {
                            return <p className="hover:underline cursor-pointer"
                                      onClick={() => handleOnClick(stock)}
                                      key={index}>{stock + (items++ < props.affectedStocks.length - 1 ? "," : "")}</p>
                        })}
                    </div>
                </div>
            </div>
        </>
    )
}
export default NewsDisplay;