import React from "react";
import { ThemeProvider } from '@material-ui/core/styles';
import { useRoutes } from "react-router-dom";
import { publicRoutes, protectedRoutes } from "./routes";
import theme from './theme';
import { useAuthContext } from "./modules/authorization/context";
import AppContentLoader from "./components/appContentLoader";

export const App = () => {
    const { isAuthenticated, isLoading } = useAuthContext();
    const routing = isLoading ? [] : (isAuthenticated ? protectedRoutes : publicRoutes);
    const routes = useRoutes(routing);

    return (
        <ThemeProvider theme={theme}>
            {isLoading && <AppContentLoader />}
            {routes}
        </ThemeProvider>
    );
};

