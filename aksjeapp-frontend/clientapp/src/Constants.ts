const url = "https://localhost:7187/Stocks/"

export const API = {
    GET_ALL_STOCKS: url + "GetAllStocks",
    GET_STOCK_PRICES:
        (symbol: string, fromDate: string, toDate: string) => {
            return `StockPrices?symbol+=${symbol}&fromDate=${fromDate}&toDate=${toDate}`
        },
    BUY_STOCK: url + "BuyStock",
    SELL_STOCK: url + "SellStock",
    SEARCH_RESULTS:
        (keyPhrase: string) => {
            return url + "SearchResults?keyPhrase=" + keyPhrase;
        }
}