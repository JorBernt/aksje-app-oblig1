import React, {useState} from "react";

type Props = {
    type: string,
    label: string
    setValue?: any
    ref?: any
    onKeyDown?: (key: string) => void
}
let checkPW = "";
const InputField: React.FC<Props> = React.forwardRef<HTMLInputElement, Props>((props, ref) => {
    const [message, setMessage] = useState('');
    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setMessage(event.target.value);
        console.log(event.target.value);
        console.log(props.label)
        const regExName = /^[a-z ,.'-]+$/i;
        const regExSSN = /^[0-9]{11}$/;
        const regExPaswd = /(?=.*[a-zA-ZæøåÆØÅ])(?=.*\d)[a-zA-ZæøåÆØÅ\d]{8,}/;
        const regExPost = /^[0-9]{4}$/;
        const regExAdr = /^[a-zA-ZæøåÆØÅ. \-]{2,20}$/;

        if ((props.label === "First Name" || props.label === "Last Name") && regExName.test(event.target.value)) {
            setMessage("")
        } else if (props.label === "Password" && regExPaswd.test(event.target.value)) {
            checkPW = event.target.value
            setMessage("")
        } else if (props.label === "SSN" && regExSSN.test(event.target.value)) {
            setMessage("")
        } else if (props.label === "Postal Code" && regExPost.test(event.target.value)) {
            setMessage("")
        } else if (props.label === "Retype password") {
            if (event.target.value === checkPW) {
                setMessage("")
            } else {
                setMessage(props.label + " does not match with typed pass")
            }
        } else if ((props.label === "Address" || props.label === "Postal Address") && regExAdr.test(event.target.value)) {
            setMessage("")
        } else {
            setMessage(props.label + " is not valid")
        }
    };
    return (
        <>
            <div className="flex flex-col justify-between my-2 text-l mx-5">
                <p>{props.label}</p>
                <input type={props.type} className="bg-transparent border focus:border-pink-500 rounded-2xl pl-4 py-2"
                       style={{outline: "none"}} ref={ref}
                       onKeyDown={(event => props.onKeyDown && props.onKeyDown(event.key))}
                       onChange={handleChange}
                />
                <p style={{color: 'red'}}>{message}</p>
            </div>
        </>
    )
})

export default InputField;