import * as React from "react";
import { useEffect, useState } from "react";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { AdminPage } from "./components/adminPage";
import { HomePage } from "./components/homePage";
import { TopNavBar } from "./components/layout/navbar";
import { Login } from "./components/login/login";
import { Logout } from "./components/login/logout";
import { TokenAuthorizationResult } from "./modules/authorization/authorizationApi";
import { authService } from "./modules/authorization/authtorizationService";
import { AuthorizationContext } from "./modules/authorization/context";
import { PrivateRoute } from "./privateRoute";

export const App = () => {
    const [authData, setAuthData] = useState<TokenAuthorizationResult>(null);

    useEffect(() => {
        const data = authService.getAuthData();
        setAuthData(data);
    }, []);

    const onLogin = (data: TokenAuthorizationResult) => {
        authService.setAuthData(data);
        setAuthData(data);
    }

    const onLogout = () => {
        authService.removeAuthData();
        setAuthData(null);
    }

    const isAuthorized = () => authData && !!authData.token;

    return (
        <AuthorizationContext.Provider value={{ ...authData, onLogin, onLogout, isAuthorized }}>
            <Router>
                <TopNavBar />
                <Route path="/" exact component={HomePage} />
                <Route path="/home" exact component={HomePage} />
                <PrivateRoute path="/admin" component={AdminPage} />
                <Route path="/login" component={Login} />
                <Route path="/logout" component={Logout} />
            </Router>
        </AuthorizationContext.Provider>);
}
