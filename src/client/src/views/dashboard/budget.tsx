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
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';
import MoneyIcon from '@mui/icons-material/Money';

const useStyles = makeStyles((theme) => ({
    root: {
        height: '100%'
    },
    avatar: {
        backgroundColor: colors.red[600],
        height: 56,
        width: 56
    },
    differenceIcon: {
        color: colors.red[900]
    },
    differenceValue: {
        color: colors.red[900],
        marginRight: theme.spacing(1)
    }
}));

interface Props {
    className?: string;
}

const Budget = ({ className }: Props) => {
    const classes = useStyles();

    return (
        <Card className={clsx(classes.root, className)}>
            <CardContent>
                <Grid container spacing={3}>
                    <Grid item>
                        <Typography color="textSecondary" gutterBottom variant="h6">
                            BUDGET
                        </Typography>
                        <Typography color="textPrimary" variant="h3">
                            $24,000
                        </Typography>
                    </Grid>
                    <Grid item>
                        <Avatar className={classes.avatar}>
                            <MoneyIcon />
                        </Avatar>
                    </Grid>
                </Grid>
                <Box mt={2} display="flex" alignItems="center">
                    <ArrowDownwardIcon className={classes.differenceIcon} />
                    <Typography className={classes.differenceValue} variant="body2">
                        12%
                    </Typography>
                    <Typography color="textSecondary" variant="caption">
                        Since last month
                    </Typography>
                </Box>
            </CardContent>
        </Card>
    );
};

export default Budget;
