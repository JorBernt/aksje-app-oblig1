type Props = {
    title: string;
    content: string
}

const ProfileTextDisplay = (props: Props) => {

    return (
        <>
            <div className="pt-2.5 pb-2.5 text-text-display">
                <h2 className="text-center font-semibold">{props.title}</h2>
                <p className="text-center">{props.content}</p>
            </div>
        </>
    )
}
export default ProfileTextDisplay;