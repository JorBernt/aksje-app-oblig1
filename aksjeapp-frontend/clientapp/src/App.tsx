import React from 'react';
import './App.css';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import LandingPage from "./components/Pages/LandingPage";
import ProfilePage from "./components/Pages/ProfilePage";
import StockPage from "./components/Pages/StockPage";

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<LandingPage/>}/>
                <Route path="/profile" element={<ProfilePage/>}/>
                <Route path="/stocks" element={<StockPage/>}/>
            </Routes>
        </Router>
    );
}

export default App;
