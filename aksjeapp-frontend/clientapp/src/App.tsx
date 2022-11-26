import React, {createContext, useContext, useEffect, useState} from 'react';
import './App.css';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import LandingPage from "./components/Pages/LandingPage";
import ProfilePage from "./components/Pages/ProfilePage";
import SingleStockPage from "./components/Pages/SingleStockPage";
import Stocks from "./components/Pages/Stocks";
import LoginPage from "./components/Pages/LoginPage";
import RegisterPage from "./components/Pages/RegisterPage";
import EditPage from "./components/Pages/EditPage"
import {API} from "./Constants";

export type LoggedInContextType = {
    loggedIn: boolean
    setLoggedIn: (loggedIn: boolean) => void;
}

function createCtx<A extends {} | null>() {
    const ctx = createContext<A | undefined>(undefined);

    function useCtx() {
        const c = useContext(ctx);
        if (c === undefined)
            throw new Error("useCtx must be inside a Provider with a value");
        return c;
    }

    return [useCtx, ctx.Provider] as const; // 'as const' makes TypeScript infer a tuple
}


export const [useLoggedInContext, LoggedInContextProvider] = createCtx<LoggedInContextType>();


function App() {

    const [loggedIn, setLoggedIn] = useState(false)
    useEffect(() => {
        API.CLIENT.AUTHENTICATE_USER().then(response => {
            if (response)
                setLoggedIn(true)
        })
    }, [])

    return (
        <Router>
            <LoggedInContextProvider value={{loggedIn, setLoggedIn}}>
                <Routes>
                    <Route path="/" element={<LandingPage/>}/>
                    <Route path="/profile" element={<ProfilePage/>}/>
                    <Route path="/singleStock" element={<SingleStockPage/>}/>
                    <Route path="/stocks" element={<Stocks/>}/>
                    <Route path="/login" element={<LoginPage/>}/>
                    <Route path="/register" element={<RegisterPage/>}/>
                    <Route path="/edit" element={<EditPage/>}/>
                </Routes>
            </LoggedInContextProvider>
        </Router>
    );
}

export default App;
