export interface StockData {
    name: string
    last: number,
    change: number,
    todayDifference: number,
    buy: number,
    sell: number,
    high: number,
    low: number,
    turnover: number,
    symbol: string
    results: Array<Results>
}

export interface Results {
    closePrice: number;
    openPrice: number;
    highestPrice: number;
    lowestPrice: number;
    date: string;
}

export interface MappableData {
    name: string;
    uv: number;
}


export interface User {
    username: string;
    password: string;
}

export interface Stock {
    amount?: number;
    symbol: string;
    name: string;
    change: number;
    value: string;
}

export interface Transaction {
    id: number,
    socialSecurityNumber: string,
    date: string,
    symbol: string,
    amount: number,
    totalPrice: number,
    isActive: boolean,
    awaiting: boolean
    //Vet ikke om denne må være med?
}

export interface PortfolioList {
    symbol: string,
    amount: number;
}

export interface Portfolio {
    stockPortfolio: Array<Stock>,
    value: number,
}

export interface ProfileInfo {
    socialSecurityNumber: string,
    firstName: string,
    lastName: string,
    address: string,
    balance: number,
    portfolio: Portfolio
    transactions: Transaction[],
    postalCode: number,
    postCity: string
}

export interface News {
    publisher: string,
    title: string,
    author: string,
    date: string,
    stocks: string[],
    url: string
}

export interface UserData {
    firstName: string;
    lastName: string;
    socialSecurityNumber: string;
    address: string;
    balance?: number;
    postalCode: string;
    postCity: string;
    password?: string;
}

export interface UserDataSubmit {
    FirstName: string;
    LastName: string;
    SocialSecurityNumber: string;
    Address: string;
    Balance?: number;
    PostalCode: string;
    PostCity: string;
    User?: User;
}
