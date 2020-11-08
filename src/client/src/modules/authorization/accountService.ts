import { register } from "./authorizationApi";

export interface RegisterUserModel {
    email: string;
    firstName: string;
    lastName: string;
    password: string;
}

export const registerUser = async (model: RegisterUserModel) => {
    return register(model);
};
