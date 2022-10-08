import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import StockContainer from "../StockViews/StockContainer";

const Stocks = () => {
    return (
        <>
            <div>
                <Navbar/>
                <h1 className="text-center font-semi text-5xl bold py-10">All Stocks</h1>
                <div className="flex justify-center">
                    <Card>
                        <StockContainer text="All stocks" showAmount={false} sorted="valAsc"/>
                    </Card>
                </div>
            </div>
        </>
    )
}

export default Stocks;