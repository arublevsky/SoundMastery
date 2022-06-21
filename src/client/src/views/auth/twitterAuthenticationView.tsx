import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Container } from '@mui/material';
import { makeStyles } from '@mui/styles';
import Page from '../../components/page';
import { useAuthContext } from '../../modules/authorization/context';
import { useErrorHandling } from '../errors/useErrorHandling';
import { ErrorAlert } from '../errors/errorAlert';
import { ExternalAuthProviderType } from '../../modules/authorization/accountApi';
import { isTwitterRedirectUrl } from '../../modules/authorization/externalAuthentication';

const useStyles = makeStyles(() => ({ root: {} }));

const TwitterAuthenticationView = () => {
    const classes = useStyles();
    const navigate = useNavigate();
    const { onLoggedIn } = useAuthContext();
    const [showError, _, errors, asyncHandler] = useErrorHandling();

    const handleTwitterRedirect = async () => {
        const queryString = window.location.search;

        asyncHandler(async () => {
            onLoggedIn({ token: queryString, type: ExternalAuthProviderType.Twitter })
                .then(() => navigate("/admin/dashboard"));
        });
    };

    useEffect(() => {
        if (isTwitterRedirectUrl()) {
            handleTwitterRedirect();
        }
    }, []);

    return (
        <Page className={classes.root} title="Login">
            <Box display="flex" flexDirection="column" height="100%" justifyContent="center">
                <Container maxWidth="sm">
                    <ErrorAlert show={showError} errors={errors} title={"Login failed"} />
                </Container>
            </Box>
        </Page>);
};

export default TwitterAuthenticationView;
