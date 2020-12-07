import { TokenAuthorizationResult } from "./authorizationApi";

export interface RefreshTokenData {
    refreshToken: string;
    username: string;
}

export interface UserAuthorizationInfo extends TokenAuthorizationResult {
    loggedInAt: number;
}

export class AuthorizationService {
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

export const authorizationService = new AuthorizationService();