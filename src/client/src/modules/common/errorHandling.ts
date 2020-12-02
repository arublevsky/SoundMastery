import { ApiError } from "./apiErrors";

export interface ApplicationError extends IdentityError {
}

interface IdentityError {
    code: string;
    description: string;
}

export interface ValidationErrors {
    [key: string]: string[];
}

export const parseErrors = async (error: ApiError, showFallback = true) => {
    try {
        const json = error.response.body ? await error.response.text() : error.message;
        const response = JSON.parse(json);

        if (response && response.length && isIdentityError(response[0])) {
            return response as IdentityError[];
        }

        if (response && response.errors && isValidationError(response.errors)) {
            // validation errors should be prevented by the client side validation
            const properties = Object.keys(response.errors).join(", ");
            return [{ code: "ValidationError", description: `Model validation error occurred: ${properties}` }];
        }
    } catch {
        // do nothing
    }

    return showFallback ? [{ code: "Unknown error", description: "Unknown error" }] : [];
};

const isIdentityError = (error: unknown): error is IdentityError => {
    const casted = error as IdentityError;
    return !!(casted && casted.code && casted.description);
};

const isValidationError = (error: unknown) =>
    typeof error === 'object' && Object.values(error).every((value) => value.length > 0);
