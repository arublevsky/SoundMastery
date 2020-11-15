import { authorizationService } from "../authorization/authorizationService";
import { ApiError } from "./apiErrors";

const baseApiRoute = "https://localhost:5001/api/";

interface Options {
    silent?: boolean;
}

export interface RequestOptions extends Options {
    body?: unknown;
    params?: unknown;
}

export const httpGet = async <T extends unknown>(url: string, options?: RequestOptions) => {
    return await request<T>(url, "GET", options);
};

export const httpPost = async <T extends unknown>(url: string, options?: RequestOptions) => {
    return await request<T>(url, "POST", options);
};

export const httpPut = async <T extends unknown>(url: string, options?: RequestOptions) => {
    return await request<T>(url, "PUT", options);
};

export const httpDelete = async (url: string, options?: RequestOptions) => {
    await request(url, "DELETE", options);
};

const request = async <T extends unknown>(
    url: string,
    method: "GET" | "POST" | "DELETE" | "PUT",
    options?: RequestOptions,
) => {
    let response: Response;
    try {
        toggleLoading(true);
        response = await fetch(baseApiRoute + url, {
            body: getBody(options),
            credentials: 'include',
            headers: {
                Accept: "application/json",
                ...getContentTypeHeader(options),
                "Authorization": authorizationService.getAuthHeader(),
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

    return handleResponse<T>(response);
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

const handleResponse = async <T>(response: Response): Promise<T> => {
    const text = await response.text();
    if (!response.ok) {
        // TODO client logging https://github.com/arublevsky/SoundMastery/issues/24
        processFailedResponse(response);
    }

    return text ? JSON.parse(text) : {};
};

const processFailedResponse = (response: Response) => {
    // TODO send data from server to enrich UI errors
    // const errorCode = response.headers.get(apiConstants.headers.errorCode) as string;
    // const errorId = response.headers.get(apiConstants.headers.errorId) as string;
    // const payloadError = response.headers.get(apiConstants.headers.payloadError);
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
