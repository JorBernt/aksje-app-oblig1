type Props = {
    title: string;
    content: string | number | undefined;
    color?: string
}

const DataDisplay = (props: Props) => {
    return (
        <>
            <div className="pt-2.5 pb-2.5 text-text-display bg-transparent">
                <p className={"text-xl font-semibold text-center " + (props.color == null ? "" : props.color)}>{props.content}</p>
                <h2 className="text-center text-xs text-gray-600">{props.title}</h2>
            </div>
        </>
    )
}
export default DataDisplay;