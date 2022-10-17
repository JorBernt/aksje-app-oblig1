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