import { TokenAuthorizationResult } from "../../../modules/authorization/authorizationApi";

export class AuthorizationState {
    private token: string;
    private expiresInMilliseconds: number;

    public login = (result: TokenAuthorizationResult) => {
        localStorage.setItem('isLoggedIn', 'true');
        this.expiresInMilliseconds = result.expiresInMilliseconds + new Date().getTime();
        this.token = result.token;
    }

    public logout = () => {
        localStorage.removeItem('isLoggedIn');
        this.expiresInMilliseconds = 0;
        this.token = undefined;
    }

    public getAuthHeader = () => {
        if (this.isAuthenticated() && this.token) {
            return `Bearer ${this.token}`;
        }
        return undefined;
    }

    public isAuthenticated = () => {
        return localStorage.getItem("isLoggedIn") === 'true' && new Date().getTime() < this.expiresInMilliseconds
    }
}