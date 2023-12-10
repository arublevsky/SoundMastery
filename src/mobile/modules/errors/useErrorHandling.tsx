import { useState } from "react";
import { ApiError } from "../common/apiErrors";
import { ApplicationError, parseErrors } from "../common/errorHandling";

type ReturnType = [
    ApplicationError[],
    <T extends void>(action: () => Promise<T>) => Promise<T | null>,
    () => void,
    <T extends void>(action: () => T | null) => void,
];

export const useErrorHandling = (): ReturnType => {
    const [errors, setErrors] = useState<ApplicationError[]>([]);

    const handleError = async (error: ApiError | unknown) => {
        const errors = await parseErrors(error);
        setErrors(errors);
    };

    const clearErrors = () => {
        setErrors([]);
    };

    const handler = async <T extends void>(action: () => T) => {
        try {
            return action();
        } catch (e: unknown) {
            await handleError(e);
            return null;
        }
    };

    const asyncHandler = async <T extends void>(action: () => Promise<T>) => {
        try {
            return await action();
        } catch (e: unknown) {
            await handleError(e);
            return null;
        }
    };

    return [errors, asyncHandler, clearErrors, handler];
};
