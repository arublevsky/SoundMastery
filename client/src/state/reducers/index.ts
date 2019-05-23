import { UserActionsType } from "../types";
import { LOGIN_USER, LOGOUT_USER } from "../actionTypes";

const _state = {
    isLoggedIn: false,
    login: ""
}

export const userReducer = (state = _state, action: UserActionsType) => {
    switch (action.type) {
        case LOGIN_USER:
            return Object.assign({}, state, {
                isLoggedIn: true,
                login: action.email,
            });
        case LOGOUT_USER:
            return Object.assign({}, state, { isLoggedIn: false })
        default:
            return state;
    }
}