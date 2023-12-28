import { authenticationService } from "../authorization/authenticationService";
import { ApiError } from "./apiErrors";
import { Platform } from "react-native";

declare const __API_BASE_URL__: string;

const baseApiRoute = Platform.OS === 'ios'
    ? `http://localhost:5000/api/`
    : `http://10.0.2.2:5000/api/`;

interface Options {
    silent?: boolean;
    blob?: boolean;
}

export interface RequestOptions extends Options {
    body?: unknown;
    params?: unknown;
}

export const getRequestUrl = (path: string) => `${baseApiRoute}${path}`;

export const httpGet = async <T>(url: string, options?: RequestOptions) => {
    return await request<T>(url, "GET", options);
};

export const httpPost = async <T>(url: string, options?: RequestOptions) => {
    return await request<T>(url, "POST", options);
};

export const httpPut = async <T>(url: string, options?: RequestOptions) => {
    return await request<T>(url, "PUT", options);
};

export const httpDelete = async (url: string, options?: RequestOptions) => {
    await request(url, "DELETE", options);
};

export const getAuthHeader = () => ({
    "Authorization": authenticationService.getAuthHeader()!  
});

const request = async <T>(
    url: string,
    method: "GET" | "POST" | "DELETE" | "PUT",
    options?: RequestOptions,
) => {
    let response: Response;

    try {
        toggleLoading(true);
        response = await fetch(getRequestUrl(url), {
            body: getBody(options),
            credentials: 'include',
            headers: {
                Accept: "application/json",
                ...getContentTypeHeader(options),
                // @ts-ignore
                "Authorization": authenticationService.getAuthHeader(),
            },
            method,
        });
    } catch (error) {
        // TODO client logging https://github.com/arublevsky/SoundMastery/issues/24
        console.error(error);
        throw error;
    } finally {
        toggleLoading(false, options?.silent);
    }

    return handleResponse<T>(response, options);
};

const getBody = (options?: RequestOptions) => {
    if (!options) {
        return undefined;
    }
    if (options.body === null || typeof options.body !== "object" || options.body instanceof FormData) {
        return options.body as FormData;
    }
    return JSON.stringify(options.body);
};

const getContentTypeHeader = (options?: RequestOptions) => {
    if (options === null || options === undefined) {
        return { "Content-Type": "text/plain" };
    }
    if (options.body instanceof FormData) {
        return {};
    }
    return {
        "Content-Type": typeof options.body === "object" ? "application/json" : "text/plain",
    };
};

const handleResponse = async <T>(response: Response, options: RequestOptions | undefined): Promise<T> => {
    if (!response.ok) {
        // TODO client logging https://github.com/arublevsky/SoundMastery/issues/24
        processFailedResponse(response);
    }

    if (options?.blob) {
        const blob = await response.blob()
        return { file: blob } as T;
    }

    const text = await response.text();
    return text ? JSON.parse(text) : {};
};

const processFailedResponse = (response: Response) => {
    if (response.status === 401) {
        authenticationService.logout();
        return;
    }

    throw new ApiError(response, '');
};

// TODO implement loading animation on requests
// https://github.com/arublevsky/SoundMastery/issues/25
const toggleLoading = (show: boolean, silent = false) => {
    if (silent) {
        return;
    }

    if (show) {
        // loading.show();
    } else {
        // loading.hide();
    }
};
