import React from 'react';
import clsx from 'clsx';
import {
    Avatar,
    Box,
    Card,
    CardContent,
    Grid,
    LinearProgress,
    Typography,
    colors
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import InsertChartIcon from '@mui/icons-material/InsertChartOutlined';

const useStyles = makeStyles(() => ({
    root: {
        height: '100%'
    },
    avatar: {
        backgroundColor: colors.orange[600],
        height: 56,
        width: 56
    }
}));

interface Props {
    className?: string;
}

const TasksProgress = ({ className }: Props) => {
    const classes = useStyles();

    return (
        <Card className={clsx(classes.root, className)}>
            <CardContent>
                <Grid container spacing={3}>
                    <Grid item>
                        <Typography color="textSecondary" gutterBottom variant="h6">
                            TASKS PROGRESS
                        </Typography>
                        <Typography color="textPrimary" variant="h3">
                            75.5%
                        </Typography>
                    </Grid>
                    <Grid item>
                        <Avatar className={classes.avatar}>
                            <InsertChartIcon />
                        </Avatar>
                    </Grid>
                </Grid>
                <Box mt={3}>
                    <LinearProgress value={75.5} variant="determinate" />
                </Box>
            </CardContent>
        </Card>
    );
};

export default TasksProgress;
