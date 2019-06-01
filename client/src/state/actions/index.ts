import { LOGIN_USER, LOGOUT_USER } from '../actionTypes';
import { LoginAction, LogoutAction } from '../types';
import { login, TokenAuthorizationResult } from '../../modules/authorization/authorizationApi';
import { Dispatch } from 'redux';

export const loginUserAsync = (email: string, password: string) =>
    async (dispatch: Dispatch<LoginAction>) => {
        const result = await login(email, password);
        return dispatch(loginUser(result));
    };

export const loginUser = (result: TokenAuthorizationResult): LoginAction => {
    return {
        type: LOGIN_USER,
        result: result,
    }
}

export const logoutUser = (): LogoutAction => {
    return {
        type: LOGOUT_USER,
    }
}