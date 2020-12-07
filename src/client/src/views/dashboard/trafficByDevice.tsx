import React from 'react';
import clsx from 'clsx';
import { Doughnut } from 'react-chartjs-2';
import {
    Box,
    Card,
    CardContent,
    CardHeader,
    Divider,
    Typography,
    makeStyles,
    useTheme
} from '@material-ui/core';
import { devices, getTrafficOptions, trafficData } from './data/traffic';

const useStyles = makeStyles(() => ({ root: { height: '100%' } }));

interface Props {
    className?: string;
}

const TrafficByDevice = ({ className }: Props) => {
    const classes = useStyles();
    const theme = useTheme();

    const items = devices.map(({ color, icon: Icon, title, value }) => (
        <Box key={title} p={1} textAlign="center">
            <Icon color="action" />
            <Typography color="textPrimary" variant="body1">
                {title}
            </Typography>
            <Typography style={{ color }} variant="h2">
                {value}%
            </Typography>
        </Box>
    ));

    return (
        <Card className={clsx(classes.root, className)}>
            <CardHeader title="Traffic by Device" />
            <Divider />
            <CardContent>
                <Box height={300} position="relative">
                    <Doughnut data={trafficData} options={getTrafficOptions(theme)} />
                </Box>
                <Box display="flex" justifyContent="center" mt={2}>
                    {items}
                </Box>
            </CardContent>
        </Card>
    );
};

export default TrafficByDevice;
