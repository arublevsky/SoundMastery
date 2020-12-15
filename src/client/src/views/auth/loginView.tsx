import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Container, makeStyles } from '@material-ui/core';
import Page from '../../components/page';
import LoginForm from './loginForm';
import { login } from '../../modules/authorization/authorizationApi';
import { useAuthContext } from '../../modules/authorization/context';
import { useErrorHandling } from '../errors/useErrorHandling';
import { ErrorAlert } from '../errors/errorAlert';
import { facebookLogin, googleLogin, initExternalProviders } from '../../modules/authorization/externalAuthentication';

const useStyles = makeStyles((theme) => ({
    root: {
        backgroundColor: theme.palette.background.default,
        height: '100%',
        paddingBottom: theme.spacing(3),
        paddingTop: theme.spacing(3)
    }
}));

const LoginView = () => {
    const classes = useStyles();
    const navigate = useNavigate();
    const { onLoggedIn } = useAuthContext();
    const [showError, _, errors, asyncHandler] = useErrorHandling(false);

    useEffect(() => {
        initExternalProviders();
    }, []);

    const handleFormLogin = async (email: string, password: string) => {
        await asyncHandler(async () => {
            const result = await login(email, password);
            await onLoggedIn(result);
            navigate("/admin/dashboard");
        });
    };

    const handleFacebookLogin = () => {
        facebookLogin((result) => {
            asyncHandler(async () => {
                await onLoggedIn(result);
                navigate("/admin/dashboard");
            });
        });
    };

    const handleGoogleLogin = async () => {
        await googleLogin((result) => {
            asyncHandler(async () => {
                await onLoggedIn(result);
                navigate("/admin/dashboard");
            });
        });
    };

    return (
        <Page className={classes.root} title="Login">
            <Box display="flex" flexDirection="column" height="100%" justifyContent="center">
                <Container maxWidth="sm">
                    <ErrorAlert show={showError} errors={errors} title={"Login failed"} />
                    <LoginForm
                        handleFormLogin={handleFormLogin}
                        handleFacebookLogin={handleFacebookLogin}
                        handleGoogleLogin={handleGoogleLogin}
                    />
                </Container>
            </Box>
        </Page>
    );
};

export default LoginView;
