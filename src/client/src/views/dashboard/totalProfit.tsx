import React from 'react';
import clsx from 'clsx';
import {
    Avatar,
    Card,
    CardContent,
    Grid,
    Typography,
    colors
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import AttachMoneyIcon from '@mui/icons-material/AttachMoney';

const useStyles = makeStyles(() => ({
    root: {
        height: '100%'
    },
    avatar: {
        backgroundColor: colors.indigo[600],
        height: 56,
        width: 56
    }
}));

interface Props {
    className?: string;
}

const TotalProfit = ({ className }: Props) => {
    const classes = useStyles();

    return (
        <Card className={clsx(classes.root, className)}>
            <CardContent>
                <Grid container spacing={3}>
                    <Grid item>
                        <Typography color="textSecondary" gutterBottom variant="h6">
                            TOTAL PROFIT
                        </Typography>
                        <Typography color="textPrimary" variant="h3">
                            $23,200
                        </Typography>
                    </Grid>
                    <Grid item>
                        <Avatar className={classes.avatar}>
                            <AttachMoneyIcon />
                        </Avatar>
                    </Grid>
                </Grid>
            </CardContent>
        </Card>
    );
};

export default TotalProfit;
