import { LOGIN_USER, LOGOUT_USER } from "../actionTypes";

export interface UserContext {
    isLoggedIn: boolean;
    login: string;
}

export interface LoginAction {
    type: typeof LOGIN_USER;
    email: string;
    password: string;
}

export interface LogoutAction {
    type: typeof LOGOUT_USER;
}

export type UserActionsType = LogoutAction | LoginAction;
export type RootAction = UserActionsType;