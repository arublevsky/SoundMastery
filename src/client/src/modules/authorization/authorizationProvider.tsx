import React from "react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { ApiError } from "../common/apiErrors";
import { getProfile, UserProfile } from "../profile/profileApi";
import { TokenAuthorizationResult } from "./authorizationApi";
import { authService, UserAuthorizationInfo } from "./authtorizationService";
import { AuthorizationContext, initialState } from "./context";

export interface AuthorizationProviderProps {
    children?: React.ReactNode;
}

const AuthorizationProvider = ({ children }: AuthorizationProviderProps) => {
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(initialState.isLoading);
    const [profile, setProfile] = useState<UserProfile>(initialState.userProfile);
    const [info, setInfo] = useState<UserAuthorizationInfo>(authService.get());

    useEffect(() => {
        (async () => {
            if (!info) {
                setIsLoading(false);
            } else if (!profile) {
                await loadProfile();
            }
        })();
    }, []);

    const onLoggedIn = (data: TokenAuthorizationResult) => {
        (async () => {
            const info = {
                ...data,
                loggedInAt: new Date().getMilliseconds(),
            };

            authService.set(info);
            setInfo(info);
            await loadProfile();
        })();
    };

    const onLoggedOut = () => {
        authService.clear();
        setInfo(null);
    };

    async function loadProfile() {
        try {
            setIsLoading(true);
            const response = await getProfile();
            setProfile(response);
        } catch (e) {
            if (e instanceof ApiError && e.isUnauthenticated()) {
                onLoggedOut();
                navigate("/login");
            }
        } finally {
            setIsLoading(false);
        }
    }

    const getIsAuthenticated = () => {
        const data = authService.get();
        if (data) {
            const expiresAt = data.loggedInAt + data.expiresInMilliseconds;
            return !!profile && expiresAt > new Date().getMilliseconds();
        }

        return false;
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
