import React from "react";
import PublicLayout from './layout/public/index';
import DashboardLayout from './layout/dashboard/index';
import LoginView from './views/auth/loginView';
import RegisterView from './views/auth/register';
import NotFoundView from "./views/errors/notFoundView";
import DashboardView from './views/dashboard';
import AccountView from './views/account';
import ProductListView from './views/products';
import CustomerListView from './views/customers';
import PublicView from './views/public/index';
import SettingsView from './views/settings';
import { Navigate } from "react-router-dom";
import TwitterAuthenticationView from "./views/auth/twitterAuthenticationView";

export const publicRoutes = [
    {
        path: '/',
        element: <PublicLayout />,
        children: [
            { path: 'public', element: <PublicView /> },
            { path: 'login', element: <LoginView /> },
            { path: 'twitter-sign-in-success', element: <TwitterAuthenticationView /> },
            { path: 'register', element: <RegisterView /> },
            { path: '404', element: <NotFoundView /> },
            { path: '/', element: <Navigate to="/public" /> },
            { path: '*', element: <Navigate to="/404" /> }
        ]
    }
];

export const protectedRoutes = [
    {
        path: 'admin',
        element: <DashboardLayout />,
        children: [
            { path: '/', element: <Navigate to="/admin/dashboard" /> },
            { path: 'account', element: <AccountView /> },
            { path: 'customers', element: <CustomerListView /> },
            { path: 'dashboard', element: <DashboardView /> },
            { path: 'products', element: <ProductListView /> },
            { path: 'settings', element: <SettingsView /> },
            { path: '*', element: <Navigate to="/404" /> }
        ]
    },
    ...publicRoutes,
];