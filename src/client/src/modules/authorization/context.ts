import { createContext, useContext } from "react";
import { TokenAuthorizationResult } from "./authorizationApi";

export interface AuthorizationContext extends TokenAuthorizationResult {
    onLogin: (data: TokenAuthorizationResult) => void;
    onLogout: () => void;
    isAuthorized: () => boolean;
}

export const AuthorizationContext = createContext<AuthorizationContext>(null);
export const useAuthorization = () => useContext(AuthorizationContext);
