import React, { useState } from 'react';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import clsx from 'clsx';
import {
    AppBar,
    Badge,
    Box,
    Hidden,
    IconButton,
    Toolbar,
    makeStyles
} from '@material-ui/core';
import MenuIcon from '@material-ui/icons/Menu';
import NotificationsIcon from '@material-ui/icons/NotificationsOutlined';
import InputIcon from '@material-ui/icons/Input';
import Logo from '../../components/logo';
import { useAuthContext } from '../../modules/authorization/context';

const useStyles = makeStyles(() => ({
    root: {},
    avatar: {
        width: 60,
        height: 60,
    }
}));

interface Props {
    className?: string;
    onMobileNavOpen: () => void;
}

const TopBar = ({ className, onMobileNavOpen }: Props) => {
    const classes = useStyles();
    const [notifications] = useState([]);
    const navigate = useNavigate();
    const { onLoggedOut } = useAuthContext();

    const handleLogout = () => {
        onLoggedOut();
        navigate("/login");
    };

    return (
        <AppBar className={clsx(classes.root, className)} elevation={0}>
            <Toolbar>
                <RouterLink to="/">
                    <Logo />
                </RouterLink>
                <Box flexGrow={1} />
                <Hidden mdDown>
                    <IconButton color="inherit">
                        <Badge badgeContent={notifications.length} color="primary" variant="dot">
                            <NotificationsIcon />
                        </Badge>
                    </IconButton>
                    <IconButton color="inherit" onClick={handleLogout}>
                        <InputIcon />
                    </IconButton>
                </Hidden>
                <Hidden lgUp>
                    <IconButton color="inherit" onClick={onMobileNavOpen}>
                        <MenuIcon />
                    </IconButton>
                </Hidden>
            </Toolbar>
        </AppBar>
    );
};

export default TopBar;
