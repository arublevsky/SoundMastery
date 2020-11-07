import { useEffect, useState } from "react";
import { TokenAuthorizationResult } from "./authorizationApi";
import { authService } from "./authtorizationService";

export const useAuthorization = () => {
    const data = authService.getAuthData();
    const [authResult, setAuthResult] = useState<TokenAuthorizationResult>(data);

    useEffect(() => {
        setAuthResult(data);
    }, []);

    const onLoggedIn = (data: TokenAuthorizationResult) => {
        authService.setAuthData(data);
        setAuthResult(data);
    };

    const onLoggedOut = () => {
        authService.removeAuthData();
        setAuthResult(null);
    };

    const isAuthorized = () => authResult && !!authResult.token;

    return { onLoggedIn, onLoggedOut, isAuthorized };
};
