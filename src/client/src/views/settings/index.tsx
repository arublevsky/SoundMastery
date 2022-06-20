import React from 'react';
import {
    Box,
    Container
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import Page from '../../components/page';
import Notifications from './notifications';
import Password from './password';

const useStyles = makeStyles((theme) => ({
    root: {
        backgroundColor: theme.palette.background.default,
        minHeight: '100%',
        paddingBottom: theme.spacing(3),
        paddingTop: theme.spacing(3)
    }
}));

const SettingsView = () => {
    const classes = useStyles();

    return (
        <Page className={classes.root} title="Settings">
            <Container maxWidth="lg">
                <Notifications />
                <Box mt={3}>
                    <Password />
                </Box>
            </Container>
        </Page>
    );
};

export default SettingsView;
