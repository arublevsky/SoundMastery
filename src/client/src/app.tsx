import React from "react";
import { ThemeProvider } from '@material-ui/core/styles';
import { useRoutes } from "react-router-dom";
import { AuthorizationContext } from "./modules/authorization/context";
import { authRoutes, publicRoutes } from "./routes";
import { useAuthorization } from "./modules/authorization/useAuthorization";
import theme from './theme';

export const App = () => {
    const { onLoggedIn, onLoggedOut, isAuthorized } = useAuthorization();
    const authRouting = useRoutes(authRoutes);
    const publicRouting = useRoutes(publicRoutes);

    const routes = isAuthorized() ? authRouting : publicRouting;

    return (
        <AuthorizationContext.Provider value={{ onLoggedIn, onLoggedOut, isAuthorized }}>
            <ThemeProvider theme={theme}>
                {routes}
            </ThemeProvider>
        </AuthorizationContext.Provider>);
};
