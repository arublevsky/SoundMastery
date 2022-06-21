import { useState } from "react";
import { ApiError } from "../../modules/common/apiErrors";
import { ApplicationError, parseErrors } from "../../modules/common/errorHandling";

type ReturnType = [
    boolean,
    boolean,
    ApplicationError[],
    <T extends void>(action: () => Promise<T>) => Promise<T>,
    <T extends void>(action: () => T) => void,
];

export const useErrorHandling = (showFallback = true): ReturnType => {
    const [showSuccess, setShowSuccess] = useState(false);
    const [showError, setShowError] = useState(false);
    const [errors, setErrors] = useState<ApplicationError[]>(null);

    const handleError = async (error: ApiError) => {
        const errors = await parseErrors(error, showFallback);
        setErrors(errors);
        setShowError(true);
    };

    const handler = <T extends void>(action: () => T) => {
        try {
            const result = action();
            setShowSuccess(true);
            return result;
        } catch (e) {
            handleError(e);
        }
    };

    const asyncHandler = async <T extends void>(action: () => Promise<T>) => {
        try {
            const result = await action();
            setShowSuccess(true);
            return result;
        } catch (e) {
            handleError(e);
        }
    };

    return [showError, showSuccess, errors, asyncHandler, handler];
};
