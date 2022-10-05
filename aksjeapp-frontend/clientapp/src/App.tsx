import React from 'react';
import './App.css';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import LandingPage from "./components/Pages/LandingPage";
import ProfilePage from "./components/Pages/ProfilePage";

function App() {

    return (
        <Router>
            <Routes>
                <Route path="/" element={<LandingPage/>}/>
                <Route path="/profile" element={<ProfilePage/>}/>
            </Routes>
        </Router>
    );
}

export default App;
