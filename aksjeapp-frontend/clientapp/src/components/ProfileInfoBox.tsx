const ProfileInfoBox = () => {
    return (
        <>
            <div className="grid-cols-1">
                <h1 className="flex justify-center py-1">Mephisto</h1>
                <hr className="border-black"/>
                <div className="pt-5">
                    <h2 className="text-center font-semibold">Penger på konto</h2>
                    <p className="text-center">1500$</p>
                </div>
                <div className="pt-5">
                    <h2 className="text-center font-semibold">Portfolio value</h2>
                    <p className="text-center">214390.23$</p>
                </div>
                <div className="pt-5">
                    <h2 className="text-center font-semibold">Change this week</h2>
                    <p className="text-center">+0.09%</p>
                </div>
                K
            </div>
        </>
    )
}
export default ProfileInfoBox;