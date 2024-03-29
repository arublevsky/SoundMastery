import React from 'react';
import {
    Container,
    Grid
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import Page from '../../components/page';
import Budget from './budget';
import LatestOrders from './latestOrders';
import LatestProducts from './latestProducts';
import Sales from './sales';
import TasksProgress from './tasksProgress';
import TotalCustomers from './totalCustomers';
import TotalProfit from './totalProfit';
import TrafficByDevice from './trafficByDevice';

const useStyles = makeStyles((theme) => ({
    root: {
        backgroundColor: theme.palette.background.default,
        minHeight: '100%',
        paddingBottom: theme.spacing(3),
        paddingTop: theme.spacing(3)
    }
}));

const Dashboard = () => {
    const classes = useStyles();

    return (
        <Page className={classes.root} title="Dashboard">
            <Container maxWidth={false}>
                <Grid container spacing={3}>
                    <Grid item lg={3} sm={6} xl={3} xs={12}>
                        <Budget />
                    </Grid>
                    <Grid item lg={3} sm={6} xl={3} xs={12}>
                        <TotalCustomers />
                    </Grid>
                    <Grid item lg={3} sm={6} xl={3} xs={12}>
                        <TasksProgress />
                    </Grid>
                    <Grid item lg={3} sm={6} xl={3} xs={12}>
                        <TotalProfit />
                    </Grid>
                    <Grid item lg={8} md={12} xl={9} xs={12}>
                        <Sales />
                    </Grid>
                    <Grid item lg={4} md={6} xl={3} xs={12}>
                        <TrafficByDevice />
                    </Grid>
                    <Grid item lg={4} md={6} xl={3} xs={12}>
                        <LatestProducts />
                    </Grid>
                    <Grid item lg={8} md={12} xl={9} xs={12}>
                        <LatestOrders />
                    </Grid>
                </Grid>
            </Container>
        </Page>
    );
};

export default Dashboard;
