import React from "react";
import MainLayout from './layout/main/index';
import DashboardLayout from './layout/dashboard/index';
import LoginView from './views/auth/login';
import RegisterView from './views/auth/register';
import NotFoundView from "./views/errors/notFoundView";
import DashboardView from './views/dashboard';
import AccountView from './views/account';
import ProductListView from './views/products';
import CustomerListView from './views/customers';
import PublicView from './views/public/index';
import SettingsView from './views/settings';
import { Navigate } from "react-router-dom";

export const publicRoutes = [
    {
        path: '/',
        element: <MainLayout />,
        children: [
            { path: 'public', element: <PublicView /> },
            { path: 'login', element: <LoginView /> },
            { path: 'register', element: <RegisterView /> },
            { path: '404', element: <NotFoundView /> },
            { path: '/', element: <Navigate to="/public" /> },
            { path: '*', element: <Navigate to="/404" /> }
        ]
    }
];

export const authRoutes = [
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