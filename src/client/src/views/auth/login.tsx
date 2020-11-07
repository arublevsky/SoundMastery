import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Box,
    Container,
    makeStyles
} from '@material-ui/core';
import Page from '../../components/page';
import LoginForm from './loginForm';
import { login } from '../../modules/authorization/authorizationApi';
import { useAuthContext } from '../../modules/authorization/context';
import { Alert, AlertTitle } from '@material-ui/lab';

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
    const [error, setError] = useState(false);

    const handleFormLogin = async (email: string, password: string) => {
        try {
            const result = await login(email, password);
            onLoggedIn(result);
            navigate("/admin/dashboard");
        } catch (error) {
            // https://github.com/arublevsky/SoundMastery/issues/24
            setError(true);
        }
    };

    const handleFacebookLogin = () => Promise.resolve();
    const handleGoogleLogin = () => Promise.resolve();

    return (
        <Page className={classes.root} title="Login">
            <Box display="flex" flexDirection="column" height="100%" justifyContent="center">
                <Container maxWidth="sm">
                    {error &&
                        <Alert severity="error">
                            <AlertTitle>Login attempt failed</AlertTitle>
                                Invalid credentials, please try again.
                        </Alert>}
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
