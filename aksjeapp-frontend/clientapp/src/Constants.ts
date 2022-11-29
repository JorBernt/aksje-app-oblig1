import {StockData, User, UserDataSubmit} from "./components/models";

const baseURL = "https://localhost:7187/"
const stockURL = baseURL + "Stock/"

export const SSN = "12345678910";

export const API = {
    STOCK: {
        GET_ALL_STOCKS: stockURL + "GetAllStocks",
        GET_STOCK_PRICES:
            async (symbol: string, fromDate: string, toDate: string): Promise<StockData> => {
                return await fetch(`${stockURL}GetStockPrices?symbol=${symbol}&fromDate=${fromDate}&toDate=${toDate}`)
                    .then(response => {
                        if (!response.ok)
                            throw new Error("Could not fetch stock prices")
                        return response;
                    })
                    .then(response => response.json())
                    .catch((error: Error) => {
                        console.log(error.message)
                    })
            },
        BUY_STOCK:
            (symbol: string, number: number): string => {
                return `${stockURL}BuyStock?symbol=${symbol}&number=${number}`
            },
        SELL_STOCK:
            (symbol: string, number: number): string => {
                return `${stockURL}SellStock?symbol=${symbol}&number=${number}`
            },
        SEARCH_RESULTS:
            (keyPhrase: string): string => {
                return `${stockURL}SearchResults?keyPhrase=${keyPhrase}`;
            },
        GET_ALL_TRANSACTIONS: stockURL + "GetAllTransactions",
        GET_SPECIFIC_TRANSACTIONS:
            (symbol: string): string => {
                return `${stockURL}GetSpecificTransactions?symbol=${symbol}`
            },
        DELETE_TRANSACTION:
            (id: number): string => {
                return `${stockURL}DeleteTransaction?id=${id}`
            },
        UPDATE_TRANSACTION: stockURL + "UpdateTransaction",
        GET_STOCK_OVERVIEW: stockURL + "GetStockOverview",
        GET_WINNERS: stockURL + "GetWinners",
        GET_LOSERS: stockURL + "GetLosers",
        GET_NEWS: stockURL + "GetNews?symbol=TSLA",
        GET_STOCK_NAME:
            (symbol: string): string => {
                return `${stockURL}GetStockName?symbol=${symbol}`
            },

        GET_CUSTOMER_PORTFOLIO: stockURL + "GetCustomerPortfolio",
        GET_CUSTOMER_DATA: stockURL + "GetCustomerData",
    },
    CLIENT: {
        LOGIN: async (user: User): Promise<boolean> => {
            try {
                const response = await fetch(`${stockURL}LogIn`, {
                    method: "POST",
                    credentials: 'include',
                    headers: {
                        "Content-Type": "application/json",
                        'Access-Control-Allow-Headers': 'Content-Type, Authorization, Set-Cookie',
                    },
                    body: JSON.stringify(user)
                });
                return response.text().then(response => response === "Ok")
            } catch (error: unknown) {
                return false;
            }
        },
        REGISTER_CUSTOMER: async (userData: UserDataSubmit): Promise<boolean | unknown> => {
            try {
                console.log(userData)
                const response = await fetch(`${stockURL}RegisterCustomer`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(userData)
                });
                return response.status === 200;
            } catch (error: unknown) {
                return error;
            }
        },
        UPDATE_CUSTOMER: async (userData: UserDataSubmit): Promise<boolean | unknown> => {
            try {
                const response = await fetch(`${stockURL}UpdateCustomer`, {
                    method: "POST",
                    credentials: 'include',
                    headers: {
                        "Content-Type": "application/json",
                        'Access-Control-Allow-Headers': 'Content-Type, Authorization, Set-Cookie',
                    },
                    body: JSON.stringify(userData)
                });
                return response.status === 200;
            } catch (error: unknown) {
                return error;
            }
        },
        AUTHENTICATE_USER: async (): Promise<boolean> => {
            try {
                const response = await fetch(`${stockURL}IsLoggedIn`, {
                    method: "GET",
                    credentials: 'include',
                });
                return response.text().then(t => t === "true")
            } catch (ignore) {
                return false;
            }
        },
        LOGOUT: async (): Promise<boolean | unknown> => {
            try {
                const response = await fetch(`${stockURL}LogOut`, {
                    credentials: 'include',
                });
                return response.status === 200;
            } catch (error: unknown) {
                return error;
            }
        },
        WITHDRAW: async (amount: number): Promise<boolean | Error> => {
            try {
                const response = await fetch(`${stockURL}Withdraw?amount=${amount}`, {
                    credentials: 'include',
                });
                return response.status === 200;
            } catch (error) {
                console.log("Withdraw", error)
                return false
            }
        },
        DEPOSIT: async (amount: number): Promise<boolean> => {
            try {
                const response = await fetch(`${stockURL}Deposit?amount=${amount}`, {
                    credentials: 'include',
                });
                return response.status === 200;
            } catch (error) {
                console.log("Deposit", error)
                return false
            }
        },
        DELETE_ACCOUNT: async (): Promise<boolean> => {
            try {
                const response = await fetch(`${stockURL}Delete_Account`, {
                    credentials: 'include',
                });
                return response.status === 200;
            } catch (error) {
                return false;
            }
        },
        CHANGE_PASSWORD: async (user: User): Promise<boolean> => {
            try {
                const response = await fetch(`${stockURL}ChangePassword`, {
                    method: "POST",
                    credentials: 'include',
                    headers: {
                        "Content-Type": "application/json",
                        'Access-Control-Allow-Headers': 'Content-Type, Authorization, Set-Cookie',
                    },
                    body: JSON.stringify(user)
                });
                return response.status === 200;
            } catch (error) {
                return false;
            }
        }

    }
}


