import React from 'react';
import * as Yup from 'yup';
import { Formik } from 'formik';
import { UserProfile } from './../../modules/profile/profileApi';
import {
    Box,
    Button,
    Card,
    CardContent,
    CardHeader,
    Divider,
    Grid,
    TextField,
} from '@mui/material';

export interface ProfileDetailsFormData {
    firstName: string;
    lastName: string;
    phoneNumber: string;
    email: string;
}

interface Props {
    userProfile: UserProfile;
    handleProfileSave: (formData: ProfileDetailsFormData) => Promise<void>;
}

export const ProfileDetailsForm = ({ handleProfileSave, userProfile }: Props) => (
    <Formik<ProfileDetailsFormData>
        initialValues={{
            firstName: userProfile.firstName,
            lastName: userProfile.lastName,
            phoneNumber: userProfile.phoneNumber || '',
            email: userProfile.email,
        }}
        validationSchema={
            Yup.object().shape({
                firstName: Yup.string().max(255).required('First name is required'),
                lastName: Yup.string().max(255).required('Last name is required'),
                phoneNumber: Yup.string().max(25).notRequired(),
            })
        }
        onSubmit={(data) => handleProfileSave(data)}
    >
        {({
            errors,
            handleBlur,
            handleChange,
            handleSubmit,
            isSubmitting,
            touched,
            values
        }) => (
            <form onSubmit={handleSubmit}>
                <Card>
                    <CardHeader subheader="The information can be edited" title="Profile" />
                    <Divider />
                    <CardContent>
                        <Grid container spacing={3}>
                            <Grid item md={6} xs={12}>
                                <TextField
                                    error={Boolean(touched.firstName && errors.firstName)}
                                    fullWidth
                                    helperText={touched.firstName && errors.firstName}
                                    label="First name"
                                    name="firstName"
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    required
                                    value={values.firstName}
                                    variant="outlined"
                                />
                            </Grid>
                            <Grid item md={6} xs={12}>
                                <TextField
                                    error={Boolean(touched.firstName && errors.firstName)}
                                    fullWidth
                                    helperText={touched.firstName && errors.firstName}
                                    label="Last name"
                                    name="lastName"
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    required
                                    value={values.lastName}
                                    variant="outlined"
                                />
                            </Grid>
                            <Grid item md={6} xs={12}>
                                <TextField
                                    fullWidth
                                    label="Email Address"
                                    name="email"
                                    required
                                    value={userProfile.email}
                                    onBlur={handleBlur}
                                    variant="outlined"
                                    disabled={true}
                                />
                            </Grid>
                            <Grid item md={6} xs={12}>
                                <TextField
                                    fullWidth
                                    label="Phone Number"
                                    name="phoneNumber"
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    value={values.phoneNumber}
                                    variant="outlined"
                                />
                            </Grid>
                        </Grid>
                    </CardContent>
                    <Divider />
                    <Box display="flex" justifyContent="flex-end" p={2}>
                        <Button
                            color="primary"
                            disabled={isSubmitting}
                            fullWidth
                            size="large"
                            type="submit"
                            variant="contained"
                        >
                            Save details
                        </Button>
                    </Box>
                </Card>
            </form>
        )}
    </Formik>);
