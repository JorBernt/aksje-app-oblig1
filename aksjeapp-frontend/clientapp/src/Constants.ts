import {Transaction} from "./components/models";

const url = "https://localhost:7187/Stock/"

export const API = {
    GET_ALL_STOCKS: url + "GetAllStocks",
    GET_STOCK_PRICES:
        (symbol: string, fromDate: string, toDate: string) => {
            return url + `StockPrices?symbol+=${symbol}&fromDate=${fromDate}&toDate=${toDate}`
        },
    BUY_STOCK:
        (socialSecurityNumber: string, symbol: string, number: number) => {
            return url + `BuyStock?socialSecurityNumber=${socialSecurityNumber}&symbol=${symbol}&number=${number}`
        },
    SELL_STOCK:
        (socialSecurityNumber: string, symbol: string, number: number) => {
            return url + `SellStock?socialSecurityNumber=${socialSecurityNumber}&symbol=${symbol}&number=${number}`
        },
    SEARCH_RESULTS:
        (keyPhrase: string) => {
            return url + "SearchResults?keyPhrase=" + keyPhrase;
        },
    GET_ALL_TRANSACTIONS:
        (socialSecurityNumber: string) => {
            return url + "GetAllTransactions?socialSecurityNumber=" + socialSecurityNumber
        },
    GET_TRANSACTION:
        (socialSecurityNumber: string, id: number) => {
            return url + `GetTransaction?socialSecurityNumber=${socialSecurityNumber}&id=${id}`
        },
    UPDATE_TRANSACTION:
        (transaction: Transaction) => {
            return url + `GetTransaction?transaction=${transaction}`
        },
    DELETE_TRANSACTION:
        (socialSecurityNumber: string, id: number) => {
            return url + `DeleteTransaction?socialSecurityNumber=${socialSecurityNumber}&id=${id}`
        },
    STOCK_CHANGE:
        (symbol: string) => {
            return url + `StockChange?symbol=${symbol}`
        },
    GET_STOCK_OVERVIEW: url + "GetStockOverview",
    GET_NEWS: url + "GetNews",
    GET_CUSTOMER_PORTOFOLIO:
        (socialSecurityNumber: string) => {
            return url + "GetCustomerPortofolio?socialSecurityNumber=" + socialSecurityNumber
        },
}
