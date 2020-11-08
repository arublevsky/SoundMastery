import { TokenAuthorizationResult } from "./authorizationApi";

const localStorageKey = "userTokenKey";


export interface UserAuthorizationInfo extends TokenAuthorizationResult {
    loggedInAt: number;
}

export class AuthorizationService {
    public get = (): UserAuthorizationInfo => {
        const data = localStorage.getItem(localStorageKey);
        if (data) {
            return JSON.parse(data);
        }

        return null;
    }

    public set = (data: UserAuthorizationInfo) => {
        localStorage.setItem(localStorageKey, JSON.stringify(data));
    }

    public getAuthHeader = () => {
        const data = this.get();
        if (data && data.token) {
            return `Bearer ${data.token}`;
        }

        return null;
    }

    public clear = () => {
        localStorage.removeItem(localStorageKey);
    }
}

export const authService = new AuthorizationService();