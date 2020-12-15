import { createContext, useContext } from "react";
import { UserProfile } from "../profile/profileApi";
import { ExternalAuthenticationResult, TokenAuthenticationResult } from "./authorizationApi";

export interface AuthorizationContext {
    onLoggedIn: (data: TokenAuthenticationResult | ExternalAuthenticationResult) => Promise<void>;
    onLoggedOut: () => void;
    isAuthenticated: boolean;
    isLoading: boolean;
    userProfile: UserProfile;
}

export const initialState: AuthorizationContext = {
    isAuthenticated: false,
    isLoading: true,
    userProfile: null,
    onLoggedIn: () => Promise.reject("Authorization context state is not initialized yet"),
    onLoggedOut: () => { /* not initialized */ },
};

export const AuthorizationContext = createContext<AuthorizationContext>(initialState);
export const useAuthContext = () => useContext(AuthorizationContext);
