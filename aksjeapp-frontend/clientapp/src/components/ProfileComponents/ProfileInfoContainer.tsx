import DataDisplay from "../UI/TextDisplay/DataDisplay";
import {useEffect, useState} from "react";
import {API} from "../../Constants";
import {profileInfo} from "../models";

const ProfileInfoContainer = () => {
    let number: number = 8200.80
    let diff: number = -0.09

    const [profileInfo, setProfileInfo] = useState<profileInfo>({
        socialSecurityNumber: "12345678910",
        firstName: "John",
        lastName: "Lennon",
        address: "Bygaten 15",
        balance: 10,
        transactions: [],
        postalCode: 10,
        postCity: "Oslo"
    });

    useEffect(() => {
        fetch(API.GET_CUSTOMER_PORTOFOLIO("12345678910"))
            .then(response => response.json()
                .then(res => {
                    setProfileInfo(res)
                }).catch(e => {
                    console.log(e.message)
                })
            )
    }, [])

    console.log(profileInfo)

    return (
        <>
            <div className="grid-cols-1">
                <h1 className="flex justify-center py-1 text-text-display">{profileInfo.firstName + " " + profileInfo.lastName}</h1>
                <hr className="border-text-display"/>
                <DataDisplay title="Account balance" content={profileInfo.balance + "$"}/>
                <DataDisplay title="Portfolio Value" content="214390.23$"/>
                <DataDisplay title="Today  %" content={(diff > 0 ? "+" : "") + diff + "%"}
                             color={+diff < 0 ? "text-red-500" : "text-green-500"}/>
                <DataDisplay title="Today +/-" content={(number > 0 ? "+" : "") + number + "$"}
                             color={+number < 0 ? "text-red-500" : "text-green-500"}/>
                <DataDisplay title="Portfolio Value" content="214390.23$"/>

            </div>
        </>
    )
}
export default ProfileInfoContainer;