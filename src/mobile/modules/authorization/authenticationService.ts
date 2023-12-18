import {TokenAuthenticationResult} from "../api/accountApi";

export interface UserAuthorizationInfo extends TokenAuthenticationResult {
    loggedInAt: number;
}

export class AuthenticationService {
    private accessTokenInfo: UserAuthorizationInfo | null = null;
    private logoutHandler: (() => void) | null = null;

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
        if (this.logoutHandler) {
            this.logoutHandler();
        }

        this.accessTokenInfo = null;
    };

    public registerLogoutHandler = (handler: () => void) => {
        this.logoutHandler = handler;
    };
}

export const authenticationService = new AuthenticationService();