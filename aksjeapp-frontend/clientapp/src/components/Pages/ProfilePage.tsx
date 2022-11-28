import React, {useEffect, useState} from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import ProfileInfoContainer from "../ProfileComponents/ProfileInfoContainer";
import StockContainer from "../StockViews/StockContainer";
import {API} from "../../Constants";
import {ProfileInfo} from "../models";
import TransactionsView from "../StockViews/TransactionsView";
import {useLoggedInContext} from "../../App";
import {useNavigate} from "react-router-dom";
import Button from "../UI/Input/Button";

const ProfilePage = () => {
    const [profileInfo, setProfileInfo] = useState<ProfileInfo>();
    const logInContext = useLoggedInContext()
    const navigate = useNavigate()
    const [reload, setReload] = useState(false)

    const handleCallback = () => {
        setReload(val => !val)
    }

    useEffect(() => {
        fetch(API.STOCK.GET_CUSTOMER_PORTFOLIO, {credentials: 'include',})
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
                {!logInContext.loggedIn ?
                    <>
                        <div className={"w-screen flex flex-col gap-4 justify-center items-center mt-96"}>
                            <p className={"text-2xl "}>You are not logged in</p>
                            <Button text={"Log in"} onClick={() => navigate("/login")}/>
                        </div>
                    </>
                    :
                    <div>
                        <h1 className="text-center text-4xl pt-10 pb-5">Your profile</h1>
                        <div className="flex flex-row justify-center">
                            <div className="w-96">
                                <Card color={"default"} customCss={"p-5 m-5 h-[30rem]"}>
                                    <ProfileInfoContainer profileInfo={profileInfo} callback={handleCallback}/>
                                </Card>
                            </div>
                            <div>
                                <Card color={"default"} customCss={"p-5 m-5 h-[30rem]"}>
                                    <StockContainer text="Your stocks" showAmount={true} sorted="valAsc"
                                                    height="h-[24rem]"
                                                    data={profileInfo?.portfolio.stockPortfolio}/>
                                </Card>
                            </div>

                        </div>
                        <div className="flex justify-center">
                            <TransactionsView callBack={handleCallback}/>
                        </div>
                    </div>
                }
            </div>
        </>
    )
}

export default ProfilePage;