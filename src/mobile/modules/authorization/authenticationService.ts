import { TokenAuthenticationResult } from "./accountApi";

export interface UserAuthorizationInfo extends TokenAuthenticationResult {
    loggedInAt: number;
}

export class AuthenticationService {
    private accessTokenInfo: UserAuthorizationInfo | null = null;

    public set = (data: UserAuthorizationInfo) => {
        this.accessTokenInfo = data;
    };

    public get = () => {
        return this.accessTokenInfo;
    };

    public getAuthHeader = () => {
        if (this.accessTokenInfo && this.accessTokenInfo.token) {
            return `Bearer ${this.accessTokenInfo.token}`;
        }

        return null;
    };

    public logout = () => {
        this.accessTokenInfo = null;
    };
}

export const authenticationService = new AuthenticationService();