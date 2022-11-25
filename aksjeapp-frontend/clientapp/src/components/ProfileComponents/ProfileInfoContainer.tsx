import DataDisplay from "../UI/TextDisplay/DataDisplay";
import React from "react";
import {ProfileInfo} from "../models";
import Button from "../UI/Input/Button";
import {useNavigate} from "react-router-dom";

type Props = {
    profileInfo?: ProfileInfo
}

const ProfileInfoContainer: React.FC<Props> = (props) => {
    let number: number = 8200.80
    let diff: number = -0.09
    const profileInfo = props.profileInfo;
    const navigate = useNavigate();

    const handleOnClick = () => {
        navigate("/edit")
    }


    return (
        <>
            {
                profileInfo &&
                <>
                    <div className="grid-cols-1">
                        <h1 className="flex justify-center py-1 text-text-display">{profileInfo.socialSecurityNumber}</h1>
                        <hr className="border-text-display"/>
                        <DataDisplay title="Name"
                                     content={`${profileInfo.firstName} ${profileInfo.lastName}`}/>
                        <DataDisplay title="Account balance" content={`${profileInfo.balance}$`}/>
                        <DataDisplay title="Portfolio Value" content={`${profileInfo.portfolio.value}$`}/>
                        <DataDisplay title="Today  %" content={(diff > 0 ? "+" : "") + diff + "%"}
                                     color={+diff < 0 ? "text-red-500" : "text-green-500"}/>
                        <DataDisplay title="Today +/-" content={(number > 0 ? "+" : "") + number + "$"}
                                     color={+number < 0 ? "text-red-500" : "text-green-500"}/>
                        <div className="w-fill flex justify-center mt-4">
                            <Button text={"Edit Profile"} onClick={handleOnClick}/>
                        </div>
                    </div>
                </>
            }
            {
                !profileInfo &&
                <>
                    <div className="flex justify-center items-center h-80">
                        <h2>No profile data</h2>
                    </div>
                </>
            }
        </>
    )
}
export default ProfileInfoContainer;