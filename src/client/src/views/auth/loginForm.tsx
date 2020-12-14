import React from 'react';
import { Link as RouterLink } from 'react-router-dom';
import * as Yup from 'yup';
import { Formik } from 'formik';
import {
    Box,
    Button,
    Grid,
    Link,
    TextField,
    Typography
} from '@material-ui/core';
import Facebook from '../../icons/facebook';
import Google from '../../icons/google';

interface FormProps {
    email: string;
    password: string;
}

interface Props {
    handleFormLogin: (email: string, password: string) => Promise<void>;
    handleFacebookLogin: () => void;
    handleGoogleLogin: () => Promise<void>;
}

const LoginForm = ({ handleFormLogin, handleFacebookLogin, handleGoogleLogin }: Props) => (
    <Formik<FormProps>
        initialValues={{
            email: 'admin@gmail.com',
            password: 'UserPass123'
        }}
        onSubmit={(values) => handleFormLogin(values.email, values.password)}
        validationSchema={Yup.object().shape({
            email: Yup.string().email('Must be a valid email').max(255).required('Email is required'),
            password: Yup.string().max(255).required('Password is required')
        })}
    >
        {({
            errors,
            handleBlur,
            handleChange,
            handleSubmit,
            isSubmitting,
            touched,
            values,
        }) => (
                <form onSubmit={handleSubmit}>
                    <Box mb={3}>
                        <Typography color="textPrimary" variant="h2">
                            Sign in
                        </Typography>
                        <Typography color="textSecondary" gutterBottom variant="body2">
                            Sign in on the internal platform
                        </Typography>
                    </Box>
                    <Grid container spacing={3}>
                        <Grid item xs={12} md={6}>
                            <Button
                                color="primary"
                                fullWidth
                                startIcon={<Facebook />}
                                onClick={handleFacebookLogin}
                                size="large"
                                variant="contained"
                            >
                                Login with Facebook
                    </Button>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <Button
                                fullWidth
                                startIcon={<Google />}
                                onClick={handleGoogleLogin}
                                size="large"
                                variant="contained"
                            >
                                Login with Google
                            </Button>
                        </Grid>
                    </Grid>
                    <Box mt={3} mb={1}>
                        <Typography align="center" color="textSecondary" variant="body1">
                            or login with email address
                        </Typography>
                    </Box>
                    <TextField
                        error={Boolean(touched.email && errors.email)}
                        fullWidth
                        helperText={touched.email && errors.email}
                        label="Email Address"
                        margin="normal"
                        name="email"
                        onBlur={handleBlur}
                        onChange={handleChange}
                        type="email"
                        value={values.email}
                        variant="outlined"
                    />
                    <TextField
                        error={Boolean(touched.password && errors.password)}
                        fullWidth
                        helperText={touched.password && errors.password}
                        label="Password"
                        margin="normal"
                        name="password"
                        onBlur={handleBlur}
                        onChange={handleChange}
                        type="password"
                        value={values.password}
                        variant="outlined"
                    />
                    <Box my={2}>
                        <Button
                            color="primary"
                            disabled={isSubmitting}
                            fullWidth
                            size="large"
                            type="submit"
                            variant="contained"
                        >
                            Sign in now
                        </Button>
                    </Box>
                    <Typography color="textSecondary" variant="body1">
                        Don&apos;t have an account?{' '}
                        <Link component={RouterLink} to="/register" variant="h6">
                            Sign up
                        </Link>
                    </Typography>
                </form>
            )}
    </Formik>);

export default LoginForm;