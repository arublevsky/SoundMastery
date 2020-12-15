import React from "react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { ApiError } from "../common/apiErrors";
import { getProfile, UserProfile } from "../profile/profileApi";
import { ExternalAuthenticationResult, externalLogin, refreshToken, TokenAuthenticationResult } from "./authorizationApi";
import { authenticationService, UserAuthorizationInfo } from "./authenticationService";
import { AuthorizationContext, initialState } from "./context";
import { logoutExternal } from "./externalAuthentication";

export interface AuthorizationProviderProps {
    children?: React.ReactNode;
}

const isExternalAuth = (
    object: TokenAuthenticationResult | ExternalAuthenticationResult
): object is ExternalAuthenticationResult => 'type' in object;

const AuthenticationProvider = ({ children }: AuthorizationProviderProps) => {
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(initialState.isLoading);
    const [profile, setProfile] = useState<UserProfile>(initialState.userProfile);
    const [authorizationInfo, setAuthorizationInfo] = useState<UserAuthorizationInfo>(authenticationService.get());

    useEffect(() => {
        authenticationService.registerLogoutHandler(() => navigate("/login"));

        async function initialize() {
            const isAuthenticated = getIsAuthenticated();
            if (isAuthenticated) {
                await loadProfile();
            } else {
                await tryRefreshToken();
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

    const onLoggedOut = () => {
        setAuthorizationInfo(null);
        logoutExternal();
        window.localStorage.setItem('logout', Date.now().toString());
    };

    async function tryRefreshToken() {
        await executeWithLoading(async () => {
            const token = await refreshToken();
            await onLoggedIn(token);
        });
    }

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
            setIsLoading(true);
            await action();
        } catch (e) {
            if (e instanceof ApiError && e.isUnauthenticated()) {
                navigate("/login");
            }
            throw e;
        }
        finally {
            setIsLoading(false);
        }
    };

    return (
        <AuthorizationContext.Provider value={{
            onLoggedIn,
            onLoggedOut,
            isAuthenticated: getIsAuthenticated(),
            isLoading,
            userProfile: profile,
        }}>
            {children}
        </AuthorizationContext.Provider>
    );
};

export default AuthenticationProvider;
