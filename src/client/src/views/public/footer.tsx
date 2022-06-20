import React from 'react';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import { Footer } from './content';
import Link from '@mui/material/Link';

interface Props {
    footers: Footer[];
}

const Footer = ({ footers }: Props) => (
    <>
        {footers.map((footer) => (
            <Grid item xs={6} sm={3} key={footer.title}>
                <Typography variant="h6" color="textPrimary" gutterBottom>
                    {footer.title}
                </Typography>
                <ul>
                    {footer.description.map((item) => (
                        <li key={item}>
                            <Link href="#" variant="subtitle1" color="textSecondary">
                                {item}
                            </Link>
                        </li>
                    ))}
                </ul>
            </Grid>)
        )}
    </>
);

export default Footer;