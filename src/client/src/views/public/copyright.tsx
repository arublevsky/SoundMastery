import React from 'react';
import Typography from '@mui/material/Typography';
import Link from '@mui/material/Link';

const Copyright = () => (
    <Typography variant="body2" color="textSecondary" align="center">
        {'Copyright © '}
        <Link color="inherit" href="https://material-ui.com/">
            Sound Mastery
        </Link>{' '}
        {new Date().getFullYear()}
        {'.'}
    </Typography>
);

export default Copyright;
