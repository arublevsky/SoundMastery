import { createContext, useContext } from "react";
import { UserProfile } from "../api/profileApi";
import { ExternalAuthenticationResult, TokenAuthenticationResult } from "./accountApi";

export interface AuthorizationContext {
    onLoggedIn: (data: TokenAuthenticationResult | ExternalAuthenticationResult) => Promise<void>;
    onLoggedOut: () => Promise<void>;
    isAuthenticated: boolean;
    isLoading: boolean;
    userProfile: UserProfile | null;
}

export const initialState: AuthorizationContext = {
    isAuthenticated: false,
    isLoading: true,
    userProfile: null,
    onLoggedIn: () => Promise.reject("Authorization context state is not initialized yet"),
    onLoggedOut: () => Promise.reject("Authorization context state is not initialized yet"),
};

export const AuthorizationContext = createContext<AuthorizationContext>(initialState);
export const useAuthContext = () => useContext(AuthorizationContext);
