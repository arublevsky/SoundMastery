import React from 'react';
import clsx from 'clsx';
import {
    Avatar,
    Box,
    Card,
    CardContent,
    Divider,
    Grid,
    Typography
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import GetAppIcon from '@mui/icons-material/GetApp';

const useStyles = makeStyles((theme) => ({
    root: {
        display: 'flex',
        flexDirection: 'column'
    },
    statsItem: {
        alignItems: 'center',
        display: 'flex'
    },
    statsIcon: {
        marginRight: theme.spacing(1)
    }
}));

interface Props {
    className?: string;
    product: {
        id: string;
        createdAt: string;
        description: string;
        media: string;
        title: string;
        totalDownloads: string;
    }
}

const ProductCard = ({ className, product }: Props) => {
    const classes = useStyles();

    return (
        <Card className={clsx(classes.root, className)}>
            <CardContent>
                <Box display="flex" justifyContent="center" mb={3}>
                    <Avatar alt="Product" src={product.media} variant="square" />
                </Box>
                <Typography align="center" color="textPrimary" gutterBottom variant="h4">
                    {product.title}
                </Typography>
                <Typography align="center" color="textPrimary" variant="body1">
                    {product.description}
                </Typography>
            </CardContent>
            <Box flexGrow={1} />
            <Divider />
            <Box p={2}>
                <Grid container spacing={2}>
                    <Grid className={classes.statsItem} item>
                        <AccessTimeIcon className={classes.statsIcon} color="action" />
                        <Typography color="textSecondary" display="inline" variant="body2">
                            Updated 2hr ago
                        </Typography>
                    </Grid>
                    <Grid className={classes.statsItem} item>
                        <GetAppIcon className={classes.statsIcon} color="action" />
                        <Typography color="textSecondary" display="inline" variant="body2">
                            {product.totalDownloads}
                            {' '}
                            Downloads
                        </Typography>
                    </Grid>
                </Grid>
            </Box>
        </Card>
    );
};

export default ProductCard;
