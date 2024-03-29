import React from 'react';
import AppBar from '@mui/material/AppBar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import Grid from '@mui/material/Grid';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Link from '@mui/material/Link';
import { Link as RouterLink } from 'react-router-dom';
import { makeStyles } from '@mui/styles';
import Container from '@mui/material/Container';
import Box from '@mui/material/Box';
import { useNavigate } from 'react-router-dom';
import { footers, tiers } from './content';
import Tiers from './tiers';
import Footer from './footer';
import Copyright from './copyright';
import { useAuthContext } from '../../modules/authorization/context';

const useStyles = makeStyles((theme) => ({
    '@global': {
        ul: {
            margin: 0,
            padding: 0,
            listStyle: 'none',
        },
    },
    appBar: {
        borderBottom: `1px solid ${theme.palette.divider}`,
    },
    toolbar: {
        flexWrap: 'wrap',
    },
    toolbarTitle: {
        flexGrow: 1,
    },
    link: {
        margin: theme.spacing(1, 1.5),
    },
    heroContent: {
        padding: theme.spacing(8, 0, 6),
    },
    footer: {
        borderTop: `1px solid ${theme.palette.divider}`,
        marginTop: theme.spacing(8),
        paddingTop: theme.spacing(3),
        paddingBottom: theme.spacing(3),
        [theme.breakpoints.up('sm')]: {
            paddingTop: theme.spacing(6),
            paddingBottom: theme.spacing(6),
        },
    },
}));

const Public = () => {
    const classes = useStyles();
    const navigate = useNavigate();
    const { isAuthenticated, onLoggedOut } = useAuthContext();

    const handleLogout = () => {
        onLoggedOut();
        navigate("/");
    };

    const handleLogin = () => {
        navigate("/login");
    };

    return (
        <React.Fragment>
            <CssBaseline />
            <AppBar position="static" color="default" elevation={0} className={classes.appBar}>
                <Toolbar className={classes.toolbar}>
                    <Typography variant="h6" color="inherit" noWrap className={classes.toolbarTitle}>
                        Company name
                    </Typography>
                    <nav>
                        <Link variant="button" color="textPrimary" href="#" className={classes.link}>
                            Features
                        </Link>
                        <Link variant="button" color="textPrimary" href="#" className={classes.link}>
                            Enterprise
                        </Link>
                        {isAuthenticated &&
                            <Link component={RouterLink} to="/admin/dashboard" variant="button" color="textPrimary" className={classes.link}>
                                Administration
                            </Link>}
                    </nav>
                    {!isAuthenticated &&
                        <Button color="primary" className={classes.link} onClick={handleLogin}>
                            Login
                        </Button>}
                    {isAuthenticated &&
                        <Button color="primary" className={classes.link} onClick={handleLogout}>
                            Logout
                        </Button>}
                </Toolbar>
            </AppBar>
            <Container maxWidth="sm" component="main" className={classes.heroContent}>
                <Typography component="h1" variant="h2" align="center" color="textPrimary" gutterBottom>
                    Pricing
                </Typography>
                <Typography variant="h5" align="center" color="textSecondary" component="p">
                    Quickly build an effective pricing table for your potential customers with this layout.
                    It&apos;s built with default Material-UI components with little customization.
                </Typography>
            </Container>
            <Container maxWidth="md" component="main">
                <Grid container spacing={5} alignItems="flex-end">
                    <Tiers tiers={tiers} />
                </Grid>
            </Container>
            <Container maxWidth="md" component="footer" className={classes.footer}>
                <Grid container spacing={4}>
                    <Footer footers={footers} />
                </Grid>
                <Box mt={5}>
                    <Copyright />
                </Box>
            </Container>
        </React.Fragment>
    );
};

export default Public;