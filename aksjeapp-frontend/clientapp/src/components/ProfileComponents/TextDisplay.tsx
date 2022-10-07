type Props = {
    title?: string;
    content?: string
}

const TextDisplay = (props: Props) => {

    return (
        <>
            <div className="pt-2.5 pb-2.5 text-text-display">
                {props.title == null ? null : <h2 className="text-center font-semibold">{props.title}</h2>}
                {props.content == null ? null : <p className="text-center">{props.content}</p>}
            </div>
        </>
    )
}
export default TextDisplay;