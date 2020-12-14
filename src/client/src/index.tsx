import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter } from 'react-router-dom';
import AuthenticationProvider from "./modules/authorization/authenticationProvider";
import { App } from "./app";

ReactDOM.render(
    <BrowserRouter>
        <AuthenticationProvider>
            <App />
        </AuthenticationProvider>
    </BrowserRouter>,
    document.getElementById("app"));