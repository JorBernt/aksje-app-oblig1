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
    firstname: string;
    lastname: string;
    socialsecuritynumber: string;
    address?: string;
    postalcode: number;
    postcity: string;
    password: string;
}