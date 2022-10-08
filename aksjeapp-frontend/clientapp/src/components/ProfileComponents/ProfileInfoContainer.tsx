import TextDisplay from "../UI/TextDisplay/TextDisplay";

const ProfileInfoContainer = () => {
    let number: number = 8200.80
    let diff: number = +0.09
    return (
        <>
            <div className="grid-cols-1">
                <h1 className="flex justify-center py-1 text-text-display">Mephisto</h1>
                <hr className="border-text-display"/>
                <TextDisplay title="Account balance" content="1500$"/>
                <TextDisplay title="Portfolio Value" content="214390.23$"/>
                <TextDisplay title="Today  %" content={diff > 0 ? "+" + diff + "%" : diff + "%"}
                             color={+diff < 0 ? "text-red-500" : "text-green-500"}/>
                <TextDisplay title="Today +/-" content={number > 0 ? "+" + number + "$" : number + "$"}
                             color={+number < 0 ? "text-red-500" : "text-green-500"}/>
                <TextDisplay title="Portfolio Value" content="214390.23$"/>
                <hr className="border-text-display"/>
                <TextDisplay title="Your stocks" content="AAPL, AMZN, TSM, NVDA, TSLA, BRK.A, MSFT, GOOGL"/>

            </div>
        </>
    )
}
export default ProfileInfoContainer;