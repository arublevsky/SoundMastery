import React from "react";
import {useEffect, useState} from "react";
import {ApiError} from "../common/apiErrors";
import {getProfile, UserProfile} from "../api/profileApi";
import {
    ExternalAuthenticationResult,
    externalLogin,
    logout,
    TokenAuthenticationResult
} from "../api/accountApi";
import {authenticationService, UserAuthorizationInfo} from "./authenticationService";
import {AuthorizationContext, initialState} from "./context";

export interface AuthorizationProviderProps {
    children?: React.ReactNode;
}

const isExternalAuth = (
    object: TokenAuthenticationResult | ExternalAuthenticationResult
): object is ExternalAuthenticationResult => 'type' in object;

const AuthenticationProvider = ({children}: AuthorizationProviderProps) => {
    const [profile, setProfile] = useState<UserProfile | null>(initialState.userProfile);
    const [authorizationInfo, setAuthorizationInfo] = useState<UserAuthorizationInfo | null>(authenticationService.get());

    useEffect(() => {

        authenticationService.registerLogoutHandler(onLoggedOut);

        const initialize = async () => {
            if (getIsAuthenticated()) {
                await loadProfile();
            }
        }

        initialize();
    }, []);

    const onLoggedIn = async (data: TokenAuthenticationResult | ExternalAuthenticationResult) => {
        if (isExternalAuth(data)) {
            data = await externalLogin(data.token, data.type);
        }

        const info = {
            ...data,
            loggedInAt: new Date().getTime(),
        };

        authenticationService.set(info);
        setAuthorizationInfo(info);
        await loadProfile();
    };

    const onLoggedOut = async () => {
        await logout();
        setAuthorizationInfo(null);
    };

    async function loadProfile() {
        await executeWithLoading(async () => {
            const response = await getProfile();
            setProfile(response);
        });
    }

    const getIsAuthenticated = () => {
        if (authorizationInfo) {
            const expiresAt = authorizationInfo.loggedInAt + authorizationInfo.expiresInMilliseconds;
            return !!profile && expiresAt > new Date().getTime();
        }

        return false;
    };

    const executeWithLoading = async (action: () => Promise<unknown>) => {
        try {
            await action();
        } catch (e) {
            if (e instanceof ApiError && e.isUnauthenticated()) {
                await onLoggedOut();
                return;
            }
            throw e;
        } finally {
        }
    };

    return (
        <AuthorizationContext.Provider value={{
            isAuthenticated: getIsAuthenticated(),
            userProfile: profile,
            onLoggedIn,
            onLoggedOut
        }}>
            {children}
        </AuthorizationContext.Provider>
    );
};

export default AuthenticationProvider;
