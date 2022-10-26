import React, {useEffect, useState} from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import ProfileInfoContainer from "../ProfileComponents/ProfileInfoContainer";
import StockContainer from "../StockViews/StockContainer";
import {API} from "../../Constants";
import {ProfileInfo} from "../models";

const ProfilePage = () => {
    const [profileInfo, setProfileInfo] = useState<ProfileInfo>();

    useEffect(() => {
        fetch(API.GET_CUSTOMER_PORTOFOLIO("12345678910"))
            .then(response => response.json()
                .then(res => {
                    setProfileInfo(res)
                    console.log(res)
                }).catch(e => {
                    console.log(e.message)
                })
            )
    }, [])
    return (
        <>
            <div>
                <Navbar/>
                <h1 className="text-center text-4xl pt-10 pb-5">Your profile</h1>
                <div className="flex flex-row justify-center">
                    <div className="w-96">
                        <Card color={"default"}>
                            <ProfileInfoContainer profileInfo={profileInfo}/>
                        </Card>
                    </div>
                    <div>
                        <Card color={"default"}>
                            <StockContainer text="Your stocks" showAmount={true} sorted="valAsc" height="h-[24rem]"
                                            data={profileInfo?.portfolio.stockPortfolio}/>
                        </Card>
                    </div>

                </div>
            </div>
        </>
    )
}

export default ProfilePage;