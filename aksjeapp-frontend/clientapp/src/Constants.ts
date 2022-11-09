import Cookies from 'universal-cookie';

export const DATE_TODAY = new Date().toLocaleDateString("nb-NO").replace("/", "-");

const baseURL = "https://localhost:7187/"
const stockURL = baseURL + "Stock/"
const clientURL = baseURL + "Client/"

export const SSN = "12345678910";

const _cookie = new Cookies();
export const cookie = () => _cookie;
export const sessionToken = () => String(_cookie.get("sessionToken"))

export const API = {
    STOCK: {
        GET_ALL_STOCKS: stockURL + "GetAllStocks",
        GET_STOCK_PRICES:
            (symbol: string, fromDate: string, toDate: string): string => {
                return stockURL + `GetStockPrices?symbol=${symbol}&fromDate=${fromDate}&toDate=${toDate}`
            },
        BUY_STOCK:
            (socialSecurityNumber: string, symbol: string, number: number): string => {
                return stockURL + `BuyStock?socialSecurityNumber=${socialSecurityNumber}&symbol=${symbol}&number=${number}`
            },
        SELL_STOCK:
            (socialSecurityNumber: string, symbol: string, number: number): string => {
                return stockURL + `SellStock?socialSecurityNumber=${socialSecurityNumber}&symbol=${symbol}&number=${number}`
            },
        SEARCH_RESULTS:
            (keyPhrase: string): string => {
                return stockURL + "SearchResults?keyPhrase=" + keyPhrase;
            },
        GET_ALL_TRANSACTIONS:
            (socialSecurityNumber: string): string => {
                return stockURL + "GetAllTransactions?socialSecurityNumber=" + socialSecurityNumber
            },
        GET_SPECIFIC_TRANSACTIONS:
            (symbol: string): string => {
                return stockURL + "GetSpecificTransactions?symbol=" + symbol
            },
        GET_TRANSACTION:
            (socialSecurityNumber: string, id: number): string => {
                return stockURL + `GetTransaction?socialSecurityNumber=${socialSecurityNumber}&id=${id}`
            },
        DELETE_TRANSACTION:
            (socialSecurityNumber: string, id: number): string => {
                return stockURL + `DeleteTransaction?socialSecurityNumber=${socialSecurityNumber}&id=${id}`
            },
        STOCK_CHANGE:
            (symbol: string): string => {
                return stockURL + `StockChange?symbol=${symbol}`
            },
        UPDATE_TRANSACTION: stockURL + "UpdateTransaction",
        GET_STOCK_OVERVIEW: stockURL + "GetStockOverview",
        GET_WINNERS: stockURL + "GetWinners",
        GET_LOSERS: stockURL + "GetLosers",
        GET_NEWS: stockURL + "GetNews?symbol=TSLA",
        GET_STOCK_NAME:
            (symbol: string): string => {
                return stockURL + `GetStockName?symbol=${symbol}`
            },

        GET_CUSTOMER_PORTFOLIO:
            (socialSecurityNumber: string) => {
                return stockURL + "GetCustomerPortfolio?socialSecurityNumber=" + socialSecurityNumber
            }
    },
    CLIENT: {}
}
