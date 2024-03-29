import React from 'react';
import clsx from 'clsx';
import {
    Avatar,
    Box,
    Card,
    CardContent,
    Grid,
    Typography,
    colors
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';
import PeopleIcon from '@mui/icons-material/PeopleOutlined';

const useStyles = makeStyles((theme) => ({
    root: {
        height: '100%'
    },
    avatar: {
        backgroundColor: colors.green[600],
        height: 56,
        width: 56
    },
    differenceIcon: {
        color: colors.green[900]
    },
    differenceValue: {
        color: colors.green[900],
        marginRight: theme.spacing(1)
    }
}));

interface Props {
    className?: string;
}

const TotalCustomers = ({ className }: Props) => {
    const classes = useStyles();

    return (
        <Card className={clsx(classes.root, className)}>
            <CardContent>
                <Grid container spacing={3}>
                    <Grid item>
                        <Typography color="textSecondary" gutterBottom variant="h6">
                            TOTAL CUSTOMERS
                        </Typography>
                        <Typography color="textPrimary" variant="h3">
                            1,600
                        </Typography>
                    </Grid>
                    <Grid item>
                        <Avatar className={classes.avatar}>
                            <PeopleIcon />
                        </Avatar>
                    </Grid>
                </Grid>
                <Box mt={2} display="flex" alignItems="center">
                    <ArrowUpwardIcon className={classes.differenceIcon} />
                    <Typography className={classes.differenceValue} variant="body2">
                        16%
                    </Typography>
                    <Typography color="textSecondary" variant="caption">
                        Since last month
                    </Typography>
                </Box>
            </CardContent>
        </Card>
    );
};

export default TotalCustomers;
