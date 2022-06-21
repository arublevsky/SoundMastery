import React, { useState } from 'react';
import {
    Box,
    Container
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import Page from './../../components/page';
import Results from './results';
import Toolbar from './toolbar';
import data from './data';

const useStyles = makeStyles((theme) => ({
    root: {
        backgroundColor: theme.palette.background.default,
        minHeight: '100%',
        paddingBottom: theme.spacing(3),
        paddingTop: theme.spacing(3)
    }
}));

const CustomerListView = () => {
    const classes = useStyles();
    const [customers] = useState(data);

    return (
        <Page className={classes.root} title="Customers">
            <Container maxWidth={false}>
                <Toolbar />
                <Box mt={3}>
                    <Results customers={customers} />
                </Box>
            </Container>
        </Page>
    );
};

export default CustomerListView;
