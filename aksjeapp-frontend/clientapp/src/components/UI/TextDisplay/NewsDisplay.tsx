
type Props = {
    title: string;
    content: string | number;
    date: string;
    affectedStocks: string[];
    url: string;
}

const NewsDisplay = (props: Props) => {
    const className = "pt-2.5 pb-2.5 text-text-display grid grid-cols-3 bg-gray-300 px-5 mb-5 rounded-xl mx-2"
    return (
        <>
            <div className={className}>
                <div className="pr-5 col-span-2">
                    <p className="text-xs">{props.date}</p>
                    <a className="text-left font-semibold text-xl" href={props.url}>{props.content}</a>
                    <p className="text-left">{props.title}</p>
                </div>
                <div className="w-44 col-span 1">
                    <p>{props.affectedStocks.join(", ")}</p>
                </div>
            </div>
        </>
    )
}
export default NewsDisplay;