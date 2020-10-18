import { TokenAuthorizationResult } from "./authorizationApi";

const localStorageKey = "userTokenKey";

export class AuthorizationService {
    public getAuthData = (): TokenAuthorizationResult => {
        const data = localStorage.getItem(localStorageKey);
        if (data) {
            return JSON.parse(data);
        }

        return null;
    }

    public setAuthData = (data: TokenAuthorizationResult) => {
        localStorage.setItem(localStorageKey, JSON.stringify(data));
    }

    public getAuthHeader = () => {
        const data = this.getAuthData();
        if (data && data.token) {
            return `Bearer ${data.token}`;
        }

        return null;
    }

    public removeAuthData = () => {
        localStorage.removeItem(localStorageKey);
    }
}

export const authService = new AuthorizationService();