import React from "react";
import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import ProfileInfoBox from "../ProfileInfoBox";
import Chart from "../UI/Chart/Chart";

const ProfilePage = () => {
    return (
        <>
            <div>
                <Navbar/>
                <div className="flex flex-row justify-center">
                    <div className="w-96">
                        <Card color={"default"}>
                            <ProfileInfoBox/>
                        </Card>

                    </div>
                    <div>
                        <Card color={"default"}>
                            <Chart/>
                        </Card>
                    </div>
                </div>
            </div>
        </>
    )
}

export default ProfilePage;