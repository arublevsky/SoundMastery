import React from "react";
import 'chart.js/auto';
import { BrowserRouter } from 'react-router-dom';
import AuthenticationProvider from "./modules/authorization/authenticationProvider";
import { App } from "./app";
import { createRoot } from 'react-dom/client';

const container = document.getElementById("app");
const root = createRoot(container);
root.render(
    <BrowserRouter>
        <AuthenticationProvider>
            <App />
        </AuthenticationProvider>
    </BrowserRouter>);