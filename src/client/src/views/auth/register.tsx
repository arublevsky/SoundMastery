import React from 'react';
import { useNavigate } from 'react-router-dom';
import Page from '../../components/page';
import {
    Box,
    Container,
    makeStyles
} from '@material-ui/core';
import RegisterForm from './registerForm';

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

    const handleRegister = () => {
        navigate('/admin/dashboard', { replace: true });
        return Promise.resolve();
    };

    return (
        <Page className={classes.root} title="Register">
            <Box
                display="flex" flexDirection="column" height="100%" justifyContent="center">
                <Container maxWidth="sm">
                    <RegisterForm handleRegister={handleRegister} />
                </Container>
            </Box>
        </Page>
    );
};

export default RegisterView;
