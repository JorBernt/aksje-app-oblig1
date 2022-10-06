import ProfileTextDisplay from "./ProfileTextDisplay";

const ProfileInfoContainer = () => {
    return (
        <>
            <div className="grid-cols-1">
                <h1 className="flex justify-center py-1 text-text-display">Mephisto</h1>
                <hr className="border-text-display"/>
                <ProfileTextDisplay title="Penger på konto" content="1500$"/>
                <ProfileTextDisplay title="Portfolio Value" content="214390.23$"/>
                <ProfileTextDisplay title="Change this week" content="+0.09%"/>
                <ProfileTextDisplay title="Penger på konto" content="1500$"/>
                <ProfileTextDisplay title="Portfolio Value" content="214390.23$"/>
                <hr className="border-text-display"/>
                <ProfileTextDisplay title="Your stocks" content="AAPL, AMZN, TSM, NVDA, TSLA, BRK.A, MSFT, GOOGL"/>

            </div>
        </>
    )
}
export default ProfileInfoContainer;