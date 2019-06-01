import { authState } from "../../state/reducers/users";

const baseApiRoute = "http://localhost:5000/api/account";

export const login = async (username: string, password: string) => {
    const request = { username, password };
    return await postData<TokenAuthorizationResult>(`${baseApiRoute}/login`, request);
}

export const refreshToken = async () => {
    return await postData<TokenAuthorizationResult>(`${baseApiRoute}/refresh-token`);
};

async function postData<TResult>(url: string, data: any = {}) {
    const authHeader = authState.getAuthHeader();
    const response = await fetch(url, {
        method: 'POST',
        cache: 'no-cache',
        headers: {
            "Accept": "application/json",
            'Content-Type': 'application/json',
            "Authorization": authHeader,
        },
        body: JSON.stringify(data),
    });

    return handleResponse(response) as Promise<TResult>;
}

const handleResponse = async <T>(response: Response): Promise<T> => {
    if (!response.ok) {
        processFailedResponse(response);
    } else {
        processSuccessResponse(response);
    }
    const text = await response.text();
    return text ? JSON.parse(text) : {};
};

const processSuccessResponse = (response: Response) => {
};

const processFailedResponse = (response: Response) => {
};

export interface TokenAuthorizationResult {
    expiresInMilliseconds: number;
    token: string;
}