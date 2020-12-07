import { httpGet, httpPost } from "../common/requestApi";
import { RegisterUserModel } from "./accountService";

const apiController = "account";

export interface TokenAuthorizationResult {
    expiresInMilliseconds: number;
    token: string;
}

export const login = async (username: string, password: string) => {
    return httpPost<TokenAuthorizationResult>(`${apiController}/login`, {
        body: { username, password }
    });
};

export const register = async (body: RegisterUserModel) => {
    return httpPost<TokenAuthorizationResult>(`${apiController}/register`, {
        body: body
    });
};

export const refreshToken = async () => {
    return httpGet<TokenAuthorizationResult>(`${apiController}/refresh-token`);
};
