import React from 'react';
import './App.css';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import LandingPage from "./components/Pages/LandingPage";
import ProfilePage from "./components/Pages/ProfilePage";
import SingleStockPage from "./components/Pages/SingleStockPage";
import Stocks from "./components/Pages/Stocks";
import LoginPage from "./components/Pages/LoginPage";
import RegisterPage from "./components/Pages/RegisterPage";

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<LandingPage/>}/>
                <Route path="/profile" element={<ProfilePage/>}/>
                <Route path="/singleStock" element={<SingleStockPage/>}/>
                <Route path="/stocks" element={<Stocks/>}/>
                <Route path="/login" element={<LoginPage/>}/>
                <Route path="/register" element={<RegisterPage/>}/>
            </Routes>
        </Router>
    );
}

export default App;
