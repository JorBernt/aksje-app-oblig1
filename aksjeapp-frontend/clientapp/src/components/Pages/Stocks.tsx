import Navbar from "../Navbar/Navbar";
import Card from "../UI/Card/Card";
import StockContainer from "../StockViews/StockContainer";
import NewsContainer from "../NewsContainer";

const Stocks = () => {
    return (
        <>
            <div>
                <Navbar/>
                <h1 className="text-center font-semi text-5xl bold py-10">All Stocks</h1>
                <div className="flex flex-row justify-center">
                    <div className="flex justify-center">
                        <Card>
                            <StockContainer text="All stocks" showAmount={false} sorted="valAsc" height="h-[28.5rem]"/>
                        </Card>
                    </div>
                    <div>
                        <Card>
                            <NewsContainer/>
                        </Card>
                    </div>
                </div>
            </div>
        </>
    )
}

export default Stocks;