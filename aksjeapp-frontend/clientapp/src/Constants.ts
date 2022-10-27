import {Transaction} from "./components/models";

export const DATE_TODAY = new Date().toLocaleDateString("nb-NO").replace("/", "-");

const url = "https://localhost:7187/Stock/"

export const API = {
    GET_ALL_STOCKS: url + "GetAllStocks",
    GET_STOCK_PRICES:
        (symbol: string, fromDate: string, toDate: string): string => {
            return url + `GetStockPrices?symbol=${symbol}&fromDate=${fromDate}&toDate=${toDate}`
        },
    BUY_STOCK:
        (socialSecurityNumber: string, symbol: string, number: number) : string => {
            return url + `BuyStock?socialSecurityNumber=${socialSecurityNumber}&symbol=${symbol}&number=${number}`
        },
    SELL_STOCK:
        (socialSecurityNumber: string, symbol: string, number: number) : string => {
            return url + `SellStock?socialSecurityNumber=${socialSecurityNumber}&symbol=${symbol}&number=${number}`
        },
    SEARCH_RESULTS:
        (keyPhrase: string) : string => {
            return url + "SearchResults?keyPhrase=" + keyPhrase;
        },
    GET_ALL_TRANSACTIONS:
        (socialSecurityNumber: string) : string => {
            return url + "GetAllTransactions?socialSecurityNumber=" + socialSecurityNumber
        },
    GET_TRANSACTION:
        (socialSecurityNumber: string, id: number) : string => {
            return url + `GetTransaction?socialSecurityNumber=${socialSecurityNumber}&id=${id}`
        },
    UPDATE_TRANSACTION:
        (transaction: Transaction) : string => {
            return url + `GetTransaction?transaction=${transaction}`
        },
    DELETE_TRANSACTION:
        (socialSecurityNumber: string, id: number): string => {
            return url + `DeleteTransaction?socialSecurityNumber=${socialSecurityNumber}&id=${id}`
        },
    STOCK_CHANGE:
        (symbol: string): string => {
            return url + `StockChange?symbol=${symbol}`
        },
    GET_STOCK_OVERVIEW: url + "GetStockOverview",
    GET_WINNERS: url + "GetWinners",
    GET_LOSERS: url + "GetLosers",
    GET_NEWS: url + "GetNews?symbol=TSLA",
    GET_STOCK_NAME:
        (symbol: string): string => {
            return url + `GetStockName?symbol=${symbol}`
        },

    GET_CUSTOMER_PORTOFOLIO:
        (socialSecurityNumber: string) => {
            return url + "GetCustomerPortofolio?socialSecurityNumber=" + socialSecurityNumber
        },
}
