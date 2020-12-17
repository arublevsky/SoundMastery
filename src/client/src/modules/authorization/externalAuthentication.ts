import { ExternalAuthenticationResult, ExternalAuthProviderType, acquireTwitterRequestToken } from './accountApi';
import { PublicClientApplication, Configuration } from "@azure/msal-browser";

declare const __CLIENT_APP_BASE_URL__: string;
declare const __FACEBOOK_APP_ID__: string;
declare const __GOOGLE_CLIENT_ID__: string;
declare const __MICROSOFT_CLIENT_ID__: string;

const msalConfig: Configuration = {
    auth: {
        clientId: __MICROSOFT_CLIENT_ID__,
        redirectUri: __CLIENT_APP_BASE_URL__,
    }
};

/** 
 * Facebook login handler.
 */
export const facebookLogin = async (onSuccess: (result: ExternalAuthenticationResult) => void) => {
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

/** 
 * Google login handler.
 */
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

/** 
 * Microsoft login handler.
 */
export const microsoftLogin = async (onSuccess: (result: ExternalAuthenticationResult) => void) => {
    const client = new PublicClientApplication(msalConfig);
    const response = await client.loginPopup({ scopes: ["user.read"] });

    onSuccess({
        token: response.accessToken,
        type: ExternalAuthProviderType.Microsoft,
    });
};

/** 
 * Twitter login handler.
 */
export const twitterLogin = async () => {
    const token = await acquireTwitterRequestToken();

    // Sign-in process will continue on the specific callback URL, 
    // which is set by the server due to Twitter API CORS limitation.
    // Opening in pop-up is possible but requires extra logic that tracks popup state
    // and reports message back to opener, see https://github.com/alexandrtovmach/react-twitter-login
    window.location.replace(`https://api.twitter.com/oauth/authorize?oauth_token=${token}`);
};

export const isTwitterRedirectUrl = () => {
    const params = new URLSearchParams(window.location.search);
    const token = params.get('oauth_token');
    const verifier = params.get('oauth_verifier');
    const requestId = params.get('tweetinvi_auth_request_id');

    return token && verifier && requestId;
};

export const initExternalProviders = () => {
    gapi.load('auth2', () => gapi.auth2.init({ client_id: __GOOGLE_CLIENT_ID__ }));
    FB.init({ appId: __FACEBOOK_APP_ID__, version: 'v9.0' });
};

export const logoutExternal = () => {
    // Facebook
    FB.getLoginStatus((response) => {
        if (response?.authResponse?.accessToken) {
            FB.logout();
        }
    });

    // Google
    const auth2 = gapi.auth2?.getAuthInstance();
    if (auth2 && auth2.isSignedIn.get()) {
        auth2.signOut();
    }


    // Microsoft
    // To perform logout MS requires:
    // 1. account info to be stored somewhere (required request parameter for acquireTokenSilent)
    // 2. action confirmation by a user on the redirect page (no popup option available) 
    // https://github.com/AzureAD/microsoft-authentication-library-for-js/issues/2563
    // As we don't store access tokens and other sensitive info in the app
    // there is no security risk of not logging out here, so let's skip it for now
    //
    // Example logout flow
    // const client = new PublicClientApplication(msalConfig);
    // const response = await client.acquireTokenSilent({ account: storedAccountInfo });
    // if (response != null) client.logout({ account: response.account });
};
