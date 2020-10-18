import { httpPost } from "../common/requestApi";

const apiController = "account";

export const login = async (username: string, password: string) => {
    return httpPost<TokenAuthorizationResult>(`${apiController}/login`, {
        body: { username, password }
    });
}

export interface TokenAuthorizationResult {
    expiresInMilliseconds: number;
    token: string;
}