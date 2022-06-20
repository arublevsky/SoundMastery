import React from 'react';
import {
    Container,
    Grid
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import Page from '../../components/page';
import Profile from './profile';
import ProfileDetails from './profileDetails';

const useStyles = makeStyles((theme) => ({
    root: {
        backgroundColor: theme.palette.background.default,
        minHeight: '100%',
        paddingBottom: theme.spacing(3),
        paddingTop: theme.spacing(3)
    }
}));

const Account = () => {
    const classes = useStyles();

    return (
        <Page className={classes.root} title="Account">
            <Container maxWidth="lg">
                <Grid container spacing={3}>
                    <Grid item lg={4} md={6} xs={12}>
                        <Profile />
                    </Grid>
                    <Grid item lg={8} md={6} xs={12}>
                        <ProfileDetails />
                    </Grid>
                </Grid>
            </Container>
        </Page>
    );
};

export default Account;
