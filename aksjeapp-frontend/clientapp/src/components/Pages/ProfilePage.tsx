import React from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import ProfileInfoContainer from "../ProfileComponents/ProfileInfoContainer";
import StockContainer from "../StockViews/StockContainer";
import {API} from "../../Constants";

const ProfilePage = () => {
    return (
        <>
            <div>
                <Navbar/>
                <h1 className="text-center text-4xl pt-10 pb-5">Your profile</h1>
                <div className="flex flex-row justify-center">
                    <div className="w-96">
                        <Card color={"default"}>
                            <ProfileInfoContainer/>
                        </Card>
                    </div>
                    <div>
                        <Card color={"default"}>
                            <StockContainer text="Your stocks" showAmount={true} sorted="valAsc" height="h-[24rem]"
                                            API={API.GET_STOCK_OVERVIEW}/>
                        </Card>
                    </div>
                </div>
            </div>
        </>
    )
}

export default ProfilePage;