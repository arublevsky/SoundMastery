import React from 'react';
import clsx from 'clsx';
import { Bar } from 'react-chartjs-2';
import {
    Box,
    Button,
    Card,
    CardContent,
    CardHeader,
    Divider,
    useTheme
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';
import ArrowRightIcon from '@mui/icons-material/ArrowRight';
import { getSalesOptions, salesData } from './data/sales';

const useStyles = makeStyles(() => ({ root: {} }));

interface Props {
    className?: string;
}

const Sales = ({ className }: Props) => {
    const classes = useStyles();
    const theme = useTheme();
    const options = getSalesOptions(theme);

    return (
        <Card className={clsx(classes.root, className)}>
            <CardHeader
                action={(
                    <Button endIcon={<ArrowDropDownIcon />} size="small" variant="text">
                        Last 7 days
                    </Button>
                )}
                title="Latest Sales"
            />
            <Divider />
            <CardContent>
                <Box height={400} position="relative">
                    <Bar data={salesData} options={options} />
                </Box>
            </CardContent>
            <Divider />
            <Box display="flex" justifyContent="flex-end" p={2}>
                <Button color="primary" endIcon={<ArrowRightIcon />} size="small" variant="text">
                    Overview
                </Button>
            </Box>
        </Card>
    );
};

export default Sales;
