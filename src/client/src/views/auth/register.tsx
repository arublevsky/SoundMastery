import React from 'react';
import { useNavigate } from 'react-router-dom';
import Page from '../../components/page';
import {
    Box,
    Container
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import RegisterForm, { RegisterFormData } from './registerForm';
import { registerUser } from '../../modules/authorization/accountApi';
import { useErrorHandling } from '../errors/useErrorHandling';
import { ErrorAlert } from '../errors/errorAlert';
import { useAuthContext } from '../../modules/authorization/context';

const useStyles = makeStyles((theme) => ({
    root: {
        backgroundColor: theme.palette.background.default,
        height: '100%',
        paddingBottom: theme.spacing(3),
        paddingTop: theme.spacing(3)
    }
}));

const RegisterView = () => {
    const classes = useStyles();
    const navigate = useNavigate();
    const { onLoggedIn } = useAuthContext();

    const [showError, _, errors, asyncHandler] = useErrorHandling();

    const handleRegister = async (data: RegisterFormData) => {
        await asyncHandler(async () => {
            const result = await registerUser(data);
            await onLoggedIn(result);
            navigate('/admin/dashboard');
        });
    };

    return (
        <Page className={classes.root} title="Register">
            <Box display="flex" flexDirection="column" height="100%" justifyContent="center">
                <Container maxWidth="sm">
                    <ErrorAlert show={showError} errors={errors} title={"Registration failed"} />
                    <RegisterForm handleRegister={handleRegister} />
                </Container>
            </Box>
        </Page>
    );
};

export default RegisterView;
