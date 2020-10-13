import { UserActionsType } from "../../types";
import { LOGIN_USER, LOGOUT_USER } from "../../actionTypes";
import { AuthorizationState } from "./authtorizationState";

export interface UserState {
    isLoggedIn: () => boolean;
}

const _state = {
    isLoggedIn: () => false,
}

export const authState = new AuthorizationState();

export const userReducer = (state: UserState = _state, action: UserActionsType) => {
    switch (action.type) {
        case LOGIN_USER:
            authState.login(action.result);
            return Object.assign({}, state, {
                isLoggedIn: authState.isAuthenticated,
            });
        case LOGOUT_USER:
            authState.logout();
            return Object.assign({}, state, { isLoggedIn: authState.isAuthenticated })
        default:
            return state;
    }
}