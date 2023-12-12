import {ApiError} from "./apiErrors";

export interface ApplicationError extends IdentityError {
}

interface IdentityError {
    code: string;
    description: string;
}

const unknownError = {code: "Unknown error", description: "Unknown error"};

export const parseErrors = async (error: ApiError | unknown) => {
    try {
        if (!(error instanceof ApiError)) {
            return [unknownError];
        }

        const json = await error.response.text();
        const response = JSON.parse(json);

        if (response && response.length && isIdentityError(response[0])) {
            return response as IdentityError[];
        }

        if (response && response.errors && isValidationError(response.errors)) {
            // validation errors should be prevented by the client side validation
            const properties = Object.keys(response.errors).join(", ");
            return [{code: "ValidationError", description: `Model validation error: ${properties}`}];
        }

        if (error.response.status > 0) {
            return [{code: "StatusCodeError", description: `Invalid status code: ${error.response.status}`}];
        }
    } catch {
        // do nothing
    }

    return [unknownError];
};

const isIdentityError = (error: unknown): error is IdentityError => {
    const casted = error as IdentityError;
    return !!(casted && casted.code && casted.description);
};

const isValidationError = (error: any) =>
    typeof error === 'object' && Object.values(error).every((value) => Array.isArray(value));
