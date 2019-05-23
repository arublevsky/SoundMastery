import { LOGIN_USER, LOGOUT_USER } from '../actionTypes';
import { LoginAction, LogoutAction } from '../types';

export const loginUser = (email: string, password: string): LoginAction => {
    return {
        type: LOGIN_USER,
        email: email,
        password: password
    }
}

export const logoutUser = (): LogoutAction => {
    return {
        type: LOGOUT_USER,
    }
}