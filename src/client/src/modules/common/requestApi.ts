import { authService } from "../authorization/authtorizationService";

const baseApiRoute = "http://localhost:5000/api/"

interface Options {
    silent?: boolean;
}

export interface RequestOptions extends Options {
    body?: unknown;
    params?: unknown;
}

export const httpGet = async <Data extends {}>(url: string, options?: RequestOptions) => {
    return await request<Data>(url, "GET", options);
};

export const httpPost = async <Data extends {}>(url: string, options?: RequestOptions) => {
    return await request<Data>(url, "POST", options);
};

export const httpPut = async <Data extends {}>(url: string, options?: RequestOptions) => {
    return await request<Data>(url, "PUT", options);
};

export const httpDelete = async (url: string, options?: RequestOptions) => {
    await request(url, "DELETE", options);
};

const request = async <Data extends {}>(
    url: string,
    method: "GET" | "POST" | "DELETE" | "PUT",
    options?: RequestOptions,
) => {
    let response: Response;
    try {
        toggleLoading(true);
        response = await fetch(baseApiRoute + url, {
            body: getBody(options),

            headers: {
                Accept: "application/json",
                ...getContentTypeHeader(options),
                "Authorization": authService.getAuthHeader(),
                // 'Access-Control-Allow-Origin': "http://localhost:9100",
            },
            method,
        });
    } catch (error) {
        // TODO client logging
        throw error;
    } finally {
        toggleLoading(false, options?.silent);
    }

    return handleResponse<Data>(response);
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

const getContentTypeHeader = (options?: RequestOptions): { "Content-Type": string } | {} => {
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
    return text ? JSON.parse(text) : {};
};

// TODO implement loading animation on requests
const toggleLoading = (show: boolean, silent: boolean = false) => {
    if (silent) {
        return;
    }

    if (show) {
        // loading.show();
    } else {
        // loading.hide();
    }
}