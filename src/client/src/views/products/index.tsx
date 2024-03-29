import React from 'react';
import {
    Box,
    Container,
    Grid,
    Pagination
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import Page from '../../components/page';
import Toolbar from './toolbar';
import ProductCard from './productCard';
import products from './data';

const useStyles = makeStyles((theme) => ({
    root: {
        backgroundColor: theme.palette.background.default,
        minHeight: '100%',
        paddingBottom: theme.spacing(3),
        paddingTop: theme.spacing(3)
    },
    productCard: {
        height: '100%'
    }
}));

const ProductList = () => {
    const classes = useStyles();

    const items = products.map((product) => (
        <Grid item key={product.id} lg={4} md={6} xs={12}>
            <ProductCard className={classes.productCard} product={product} />
        </Grid>
    ));

    return (
        <Page className={classes.root} title="Products">
            <Container maxWidth={false}>
                <Toolbar />
                <Box mt={3}>
                    <Grid container spacing={3}>
                        {items}
                    </Grid>
                </Box>
                <Box mt={3} display="flex" justifyContent="center">
                    <Pagination color="primary" count={3} size="small" />
                </Box>
            </Container>
        </Page>
    );
};

export default ProductList;
