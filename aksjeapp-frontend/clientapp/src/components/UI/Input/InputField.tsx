import {IconButton} from "@mui/material";
import React, {useEffect, useState} from "react";

import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';


type Props = {
    type: string,
    label: string
    setValue?: any
    ref?: any
    onKeyDown?: (key: string) => void
    initVal?: string | number
    validate?: boolean
    password?: string
    setPassword?: any
}

const InputField: React.FC<Props> = React.forwardRef<HTMLInputElement, Props>((props, ref) => {
    const [message, setMessage] = useState('');
    const [value, setValue] = useState(props.initVal);
    const align = () => {
        switch (props.label) {
            case "Last Name":
            case "Address":
            case "City":
            case "Retype Password":
                return "right";
            default :
                return "left";
        }
    }

    useEffect(() => {
        setValue(props.initVal)
    }, [props.initVal])

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {

        setValues({password: event.target.value, showPassword: values.showPassword});
        setValue(event.target.value);
        if (typeof props.validate !== "undefined" && !props.validate) {
            return
        }
        const regExName = /^[a-zæøå A-ZÆØÅ,.'-]+$/i;
        const regExSSN = /^[0-9]{11}$/;
        const regExPaswd = /(?=.*[a-zA-ZæøåÆØÅ])(?=.*\d)[a-zA-ZæøåÆØÅ\d]{8,}/;
        const regExPost = /^[0-9]{4}$/;
        const regExAdr = /^[a-zA-ZæøåÆØÅ. -]+([0-9]*){2,20}$/;
        if (props.setPassword)
            props.setPassword(event.target.value)

        if ((props.label === "First Name" || props.label === "Last Name") && regExName.test(event.target.value)) {
            setMessage("")
        } else if (props.label === "Password" && regExPaswd.test(event.target.value)) {
            setMessage("")
        } else if (props.label === "SSN" && regExSSN.test(event.target.value)) {
            setMessage("")
        } else if (props.label === "Postal Code" && regExPost.test(event.target.value)) {
            setMessage("")
        } else if (props.label === "Retype Password") {
            if (event.target.value === props.password) {
                setMessage("")
            } else {
                setMessage("Password do not match")
            }
        } else if ((props.label === "Address" || props.label === "City") && regExAdr.test(event.target.value)) {
            setMessage("")
        } else {
            if (event.target.value === "") {
                setMessage("")
                return;
            }
            if (props.label === "Password") {
                setMessage("Password must be at least 8 characters long, and contain at least 1 uppercase and digit.")
            } else {
                setMessage(`${props.label} is invalid`)
            }
        }
    };
    const [values, setValues] = useState({
        password: "",
        showPassword: false,
    });
    const handleClickShowPassword = () => {
        setValues({...values, showPassword: !values.showPassword});
    };

    return (
        <>
            <div className="flex flex-col justify-between my-2 text-l mx-8">
                <p>{props.label}</p>
                {message.length > 0 &&
                    <div
                        className={`z-40 absolute translate-y-4 ${align() === "right" ? "translate-x-60" : "-translate-x-60"} bg-white rounded-2xl drop-shadow-xl shadow-black p-4`}>
                        <p className="text-red-500 w-48">{message}</p>
                    </div>
                }
                <div className="flex flex-row ">
                    <input type={props.type === "password" ? values.showPassword ? "text" : "password" : props.type}
                           className="bg-transparent border focus:border-pink-500 rounded-2xl pl-4 py-2"
                           style={{outline: "none"}} ref={ref}
                           onKeyDown={(event => props.onKeyDown && props.onKeyDown(event.key))}
                           onChange={handleChange}
                           value={props.type === "password" ? values.password : value}/>

                    {props.type === "password" &&
                        <div className="-mr-12">
                            <IconButton
                                onClick={handleClickShowPassword}
                            >
                                {values.showPassword ? <VisibilityIcon/> : <VisibilityOffIcon/>}
                            </IconButton>
                        </div>
                    }
                </div>
            </div>
        </>
    )
})
export default InputField;