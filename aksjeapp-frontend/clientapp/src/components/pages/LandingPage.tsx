import React from 'react';
import Card from "../UI/Card/Card";
import Navbar from "../Navbar/Navbar";

const LandingPage = () => {
    return (
        <>
            <Navbar/>
            <div className="flex flex-row justify-between">
                <div className="basis-1">
                    <div className="flex flex-auto">
                        <Card color={"default"}>
                            <p className="text-5xl">Hello World!</p>
                        </Card>

                    </div>
                </div>
                <div className="basis-1">
                    <div className="flex flex-col">
                        <Card color="default">
                            <p className="text-5xl">Card 2</p>
                        </Card>
                        <Card color="default">
                            <p className="text-5xl">Card 3</p>
                        </Card>

                    </div>
                </div>
            </div>
        </>
    )
}

export default LandingPage;