import { v4 as uuid } from 'uuid';
import moment from 'moment';

export const products = [
    {
        id: uuid(),
        name: 'Dropbox',
        imageUrl: '/static/images/products/product_1.png',
        updatedAt: moment().subtract(2, 'hours')
    },
    {
        id: uuid(),
        name: 'Medium Corporation',
        imageUrl: '/static/images/products/product_2.png',
        updatedAt: moment().subtract(2, 'hours')
    },
    {
        id: uuid(),
        name: 'Slack',
        imageUrl: '/static/images/products/product_3.png',
        updatedAt: moment().subtract(3, 'hours')
    },
    {
        id: uuid(),
        name: 'Lyft',
        imageUrl: '/static/images/products/product_4.png',
        updatedAt: moment().subtract(5, 'hours')
    },
    {
        id: uuid(),
        name: 'GitHub',
        imageUrl: '/static/images/products/product_5.png',
        updatedAt: moment().subtract(9, 'hours')
    }
];