import React from 'react';
import clsx from 'clsx';
import moment from 'moment';
import PerfectScrollbar from 'react-perfect-scrollbar';
import {
    Box,
    Button,
    Card,
    CardHeader,
    Chip,
    Divider,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    TableSortLabel,
    Tooltip
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import ArrowRightIcon from '@mui/icons-material/ArrowRight';
import { orders } from './data/orders';


const useStyles = makeStyles(() => ({
    root: {},
    actions: {
        justifyContent: 'flex-end'
    }
}));

interface Props {
    className?: string;
}

const LatestOrders = ({ className }: Props) => {
    const classes = useStyles();

    const items = orders.map((order) => (
        <TableRow hover key={order.id}>
            <TableCell>
                {order.ref}
            </TableCell>
            <TableCell>
                {order.customer.name}
            </TableCell>
            <TableCell>
                {moment(order.createdAt).format('DD/MM/YYYY')}
            </TableCell>
            <TableCell>
                <Chip
                    color="primary"
                    label={order.status}
                    size="small"
                />
            </TableCell>
        </TableRow>
    ));

    return (
        <Card className={clsx(classes.root, className)}>
            <CardHeader title="Latest Orders" />
            <Divider />
            <PerfectScrollbar>
                <Box minWidth={800}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Order Ref</TableCell>
                                <TableCell>Customer</TableCell>
                                <TableCell sortDirection="desc">
                                    <Tooltip enterDelay={300} title="Sort">
                                        <TableSortLabel active direction="desc">
                                            Date
                                        </TableSortLabel>
                                    </Tooltip>
                                </TableCell>
                                <TableCell>
                                    Status
                                </TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {items}
                        </TableBody>
                    </Table>
                </Box>
            </PerfectScrollbar>
            <Box display="flex" justifyContent="flex-end" p={2}>
                <Button color="primary" endIcon={<ArrowRightIcon />} size="small" variant="text">
                    View all
                </Button>
            </Box>
        </Card>
    );
};

export default LatestOrders;
