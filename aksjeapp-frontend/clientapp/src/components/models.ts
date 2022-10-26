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
    isActive: boolean //Vet ikke om denne må være med?
}

export interface profileInfo {
    socialSecurityNumber: string,
    firstName: string,
    lastName: string,
    address: string,
    balance: number,
    transactions: Transaction[],
    postalCode: number,
    postCity: string
}

export interface News {
    publisher: string,
    title: string,
    author: string,
    date: string,
    stocks: string[]
}