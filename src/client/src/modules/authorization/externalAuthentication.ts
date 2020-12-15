import { ExternalAuthenticationResult, ExternalAuthProviderType } from './authorizationApi';

declare const __FACEBOOK_APP_ID__: string;
declare const __GOOGLE_CLIENT_ID__: string;

export const initExternalProviders = () => {
    gapi.load('auth2', () => {
        gapi.auth2.init({ client_id: __GOOGLE_CLIENT_ID__ });
    });

    FB.init({ appId: __FACEBOOK_APP_ID__, xfbml: true, version: 'v9.0' });
    FB.AppEvents.logPageView();
};

export const facebookLogin = (onSuccess: (result: ExternalAuthenticationResult) => void) => {
    FB.getLoginStatus((response) => {
        if (response?.authResponse?.accessToken) {
            onSuccess({
                token: response.authResponse.accessToken,
                type: ExternalAuthProviderType.Facebook,
            });
        } else {
            FB.login((response) => {
                if (response?.authResponse?.accessToken) {
                    onSuccess({
                        token: response.authResponse.accessToken,
                        type: ExternalAuthProviderType.Facebook,
                    });
                }
            }, { scope: 'email' });
        }
    });
};

export const googleLogin = async (onSuccess: (result: ExternalAuthenticationResult) => void) => {
    const auth2 = gapi.auth2?.getAuthInstance();
    if (auth2.isSignedIn.get()) {
        onSuccess({
            token: auth2.currentUser.get().getAuthResponse().id_token,
            type: ExternalAuthProviderType.Google,
        });
    } else {
        const user = await auth2.signIn();
        onSuccess({
            token: user.getAuthResponse().id_token,
            type: ExternalAuthProviderType.Google,
        });
    }
};

export const logoutExternal = () => {
    FB.getLoginStatus((response) => {
        if (response?.authResponse?.accessToken) {
            FB.logout();
        }
    });

    const auth2 = gapi.auth2?.getAuthInstance();
    if (auth2 && auth2.isSignedIn.get()) {
        auth2.signOut();
    }
};
