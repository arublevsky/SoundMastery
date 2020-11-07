import React from 'react';
import clsx from 'clsx';
import {
    Box,
    Button,
    Card,
    CardHeader,
    Divider,
    IconButton,
    List,
    ListItem,
    ListItemAvatar,
    ListItemText,
    makeStyles
} from '@material-ui/core';
import MoreVertIcon from '@material-ui/icons/MoreVert';
import ArrowRightIcon from '@material-ui/icons/ArrowRight';
import { products } from './data/products';


const useStyles = makeStyles(({
    root: {
        height: '100%'
    },
    image: {
        height: 48,
        width: 48
    }
}));

interface Props {
    className?: string;
}

const LatestProducts = ({ className }: Props) => {
    const classes = useStyles();

    const items = products.map((product, i) => (
        <ListItem divider={i < products.length - 1} key={product.id}>
            <ListItemAvatar>
                <img
                    alt="Product"
                    className={classes.image}
                    src={product.imageUrl}
                />
            </ListItemAvatar>
            <ListItemText
                primary={product.name}
                secondary={`Updated ${product.updatedAt.fromNow()}`}
            />
            <IconButton edge="end" size="small">
                <MoreVertIcon />
            </IconButton>
        </ListItem>
    ));

    return (
        <Card className={clsx(classes.root, className)}>
            <CardHeader subtitle={`${products.length} in total`} title="Latest Products" />
            <Divider />
            <List>
                {items}
            </List>
            <Divider />
            <Box display="flex" justifyContent="flex-end" p={2}>
                <Button color="primary" endIcon={<ArrowRightIcon />} size="small" variant="text">
                    View all
                </Button>
            </Box>
        </Card>
    );
};

export default LatestProducts;
