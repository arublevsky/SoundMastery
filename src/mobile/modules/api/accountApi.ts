import {httpGet, httpPost} from "../common/requestApi";

const apiController = "account";

export enum ExternalAuthProviderType {
    Facebook = 1,
    Google = 2,
    Microsoft = 3,
    Twitter = 4,
}

export interface TokenAuthenticationResult {
    expiresInMilliseconds: number;
    token: string;
}

export interface ExternalAuthenticationResult {
    token: string;
    type: ExternalAuthProviderType;
}

export interface RegisterUserModel {
    email: string;
    firstName: string;
    lastName: string;
    password: string;
    role: number;
}

export const login = async (username: string, password: string) => {
    return httpPost<TokenAuthenticationResult>(`${apiController}/login`, {
        body: {username, password}
    });
};

export const externalLogin = async (accessToken: string, type: ExternalAuthProviderType) => {
    return httpPost<TokenAuthenticationResult>(`${apiController}/external-login`, {
        body: {accessToken, type}
    });
};

export const registerUser = async (body: RegisterUserModel) => {
    return httpPost<TokenAuthenticationResult>(`${apiController}/register`, {body: body});
};

export const acquireTwitterRequestToken = () => httpGet<string>(`${apiController}/twitter-request-token`);

export const logout = () => httpGet<void>(`${apiController}/logout`);
