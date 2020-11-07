import { createContext, useContext } from "react";
import { TokenAuthorizationResult } from "./authorizationApi";

export interface AuthorizationContext {
    onLoggedIn: (data: TokenAuthorizationResult) => void;
    onLoggedOut: () => void;
    isAuthorized: () => boolean;
}

export const AuthorizationContext = createContext<AuthorizationContext>(null);
export const useAuthContext = () => useContext(AuthorizationContext);
