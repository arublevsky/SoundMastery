import React from 'react';
import { Link as RouterLink } from 'react-router-dom';
import clsx from 'clsx';
import Logo from '../../components/logo';
import {
    AppBar,
    Toolbar,
    makeStyles
} from '@material-ui/core';

const useStyles = makeStyles(({
    root: {},
    toolbar: {
        height: 64
    }
}));

interface Props {
    className?: string;
}

const TopBar = ({ className }: Props) => {
    const classes = useStyles();

    return (
        <AppBar className={clsx(classes.root, className)} elevation={0}>
            <Toolbar className={classes.toolbar}>
                <RouterLink to="/">
                    <Logo />
                </RouterLink>
            </Toolbar>
        </AppBar>
    );
};

export default TopBar;
