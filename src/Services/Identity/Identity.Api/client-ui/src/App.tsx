import React from 'react';
import logo from './logo.svg';
import './App.css';
import {
  BrowserRouter,
  Routes,
  Route,
} from "react-router-dom";
import Login from "./Login";
import Home from './Home';
import Logout from './Logout';
import SignOut from './SignOutCallback';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home  />}/>
        <Route path="/login" element={<Login  />}/>
        <Route path="/logout/" element={<Logout />}/>
        <Route path="/logout/:logoutId" element={<Logout />}/>
        <Route path='/sign-out-callback' element={<SignOut />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
