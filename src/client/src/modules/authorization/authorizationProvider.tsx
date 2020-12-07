import React from "react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { ApiError } from "../common/apiErrors";
import { getProfile, UserProfile } from "../profile/profileApi";
import { refreshToken, TokenAuthorizationResult } from "./authorizationApi";
import { authorizationService, UserAuthorizationInfo } from "./authorizationService";
import { AuthorizationContext, initialState } from "./context";

export interface AuthorizationProviderProps {
    children?: React.ReactNode;
}

const AuthorizationProvider = ({ children }: AuthorizationProviderProps) => {
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(initialState.isLoading);
    const [profile, setProfile] = useState<UserProfile>(initialState.userProfile);
    const [authorizationInfo, setAuthorizationInfo] = useState<UserAuthorizationInfo>(authorizationService.get());

    useEffect(() => {
        authorizationService.registerLogoutHandler(() => navigate("/login"));

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

    const onLoggedIn = async (data: TokenAuthorizationResult) => {
        const info = {
            ...data,
            loggedInAt: new Date().getTime(),
        };

        authorizationService.set(info);
        setAuthorizationInfo(info);
        await loadProfile();
    };

    const onLoggedOut = () => {
        setAuthorizationInfo(null);
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

export default AuthorizationProvider;
