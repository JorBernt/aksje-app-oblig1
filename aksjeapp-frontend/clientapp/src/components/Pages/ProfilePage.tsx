import React, {useEffect, useState} from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import ProfileInfoContainer from "../ProfileComponents/ProfileInfoContainer";
import StockContainer from "../StockViews/StockContainer";
import {API} from "../../Constants";
import {ProfileInfo} from "../models";
import TransactionsView from "../StockViews/TransactionsView";

const ProfilePage = () => {
    const [profileInfo, setProfileInfo] = useState<ProfileInfo>();

    const [reload, setReload] = useState(false)
    const handleCallback = () => {
        setReload(val => !val)
    }

    useEffect(() => {
        fetch(API.GET_CUSTOMER_PORTOFOLIO("12345678910"))
            .then(response => response.json()
                .then(res => {
                    setProfileInfo(res)
                }).catch(e => {
                    console.log(e.message)
                })
            )
    }, [reload])
    return (
        <>
            <div>
                <Navbar/>
                <h1 className="text-center text-4xl pt-10 pb-5">Your profile</h1>
                <div className="flex flex-row justify-center">
                    <div className="w-96">
                        <Card color={"default"} customCss={"p-5 m-5 h-[30rem]"}>
                            <ProfileInfoContainer profileInfo={profileInfo}/>
                        </Card>
                    </div>
                    <div>
                        <Card color={"default"} customCss={"p-5 m-5 h-[30rem]"}>
                            <StockContainer text="Your stocks" showAmount={true} sorted="valAsc" height="h-[24rem]"
                                            data={profileInfo?.portfolio.stockPortfolio}/>
                        </Card>
                    </div>
                    
                </div>
                <div className="flex justify-center">
                    <TransactionsView callBack={handleCallback}/>
                </div>
            </div>
        </>
    )
}

export default ProfilePage;