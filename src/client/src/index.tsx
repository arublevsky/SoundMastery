import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter } from 'react-router-dom';
import AuthorizationProvider from "./modules/authorization/authorizationProvider";
import { App } from "./app";

ReactDOM.render(
    <BrowserRouter>
        <AuthorizationProvider>
            <App />
        </AuthorizationProvider>
    </BrowserRouter>,
    document.getElementById("app"));