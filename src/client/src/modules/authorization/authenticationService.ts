import { TokenAuthenticationResult } from "./accountApi";

export interface RefreshTokenData {
    refreshToken: string;
    username: string;
}

export interface UserAuthorizationInfo extends TokenAuthenticationResult {
    loggedInAt: number;
}

export class AuthenticationService {
    private accessTokenInfo?: UserAuthorizationInfo = null;

    public set = (data: UserAuthorizationInfo) => {
        this.accessTokenInfo = data;
    }

    public get = () => {
        return this.accessTokenInfo;
    }

    public getAuthHeader = () => {
        if (this.accessTokenInfo && this.accessTokenInfo.token) {
            return `Bearer ${this.accessTokenInfo.token}`;
        }

        return null;
    }

    public logout = () => {
        this.accessTokenInfo = null;
    }

    public registerLogoutHandler = (handler: () => void) => {
        // SMELL: not protected from multiple registrations
        window.addEventListener('storage', (e: StorageEvent) => {
            if (e.key === 'logout') {
                handler();
            }
        });
    }
}

export const authenticationService = new AuthenticationService();