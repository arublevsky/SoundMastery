import { TokenAuthorizationResult } from './authorizationApi';

export const facebookLogin = (handleToken: (token: TokenAuthorizationResult) => void) => {
    FB.getLoginStatus((response) => {
        if (response?.authResponse?.accessToken) {
            handleToken({
                expiresInMilliseconds: response.authResponse.expiresIn,
                token: response.authResponse.accessToken,
                isExternal: true,
            });
        } else {
            performLogin(handleToken);
        }
    });
};

const performLogin = (handleToken: (token: TokenAuthorizationResult) => void) => {
    FB.login((response) => {
        if (response?.authResponse?.accessToken) {
            handleToken({
                expiresInMilliseconds: response.authResponse.expiresIn,
                token: response.authResponse.accessToken,
                isExternal: true,
            });
        }
    }, { scope: 'email' });
};
