import React from "react";

type Props = {
    type: string,
    label: string
    setValue?: any
    ref?: any
}

const InputField: React.FC<Props> = React.forwardRef<HTMLInputElement, Props>((props, ref) => {
    return (
        <>
            <div className="flex flex-col justify-between my-2 text-l w-72">
                <p>{props.label}</p>
                <input type={props.type} className="bg-transparent border focus:border-pink-500 rounded-2xl  pl-4 py-2"
                       style={{outline: "none"}} ref={ref}/>
            </div>
        </>
    )
})

export default InputField;