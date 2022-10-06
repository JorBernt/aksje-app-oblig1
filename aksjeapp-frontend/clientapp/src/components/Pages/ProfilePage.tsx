import React from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import ProfileInfoContainer from "../ProfileComponents/ProfileInfoContainer";
import Chart from "../UI/Chart/Chart";

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
                            <p className="text-3xl text-center pb-5 text-black">Your best stock this week!</p>
                            <Chart/>
                        </Card>
                    </div>
                </div>
            </div>
        </>
    )
}

export default ProfilePage;