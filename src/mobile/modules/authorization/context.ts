import { createContext, useContext } from "react";
import { UserProfile } from "../api/profileApi";
import { ExternalAuthenticationResult, TokenAuthenticationResult } from "../api/accountApi";

export interface AuthorizationContext {
    onLoggedIn: (data: TokenAuthenticationResult | ExternalAuthenticationResult) => Promise<void>;
    onLoggedOut: () => Promise<void>;
    updateProfile: (newProfile: UserProfile) => void;
    isAuthenticated: boolean;
    userProfile: UserProfile | null;
}

export const initialState: AuthorizationContext = {
    isAuthenticated: false,
    userProfile: null,
    updateProfile: (_: UserProfile) => {},
    onLoggedIn: () => Promise.reject("Authorization context state is not initialized yet"),
    onLoggedOut: () => Promise.reject("Authorization context state is not initialized yet"),
};

export const AuthorizationContext = createContext<AuthorizationContext>(initialState);
export const useAuthContext = () => useContext(AuthorizationContext);
