import React, { useEffect } from 'react';
import { Link as RouterLink, useLocation } from 'react-router-dom';
import {
    Avatar,
    Box,
    Button,
    Divider,
    Drawer,
    Hidden,
    List,
    Typography,
    makeStyles
} from '@material-ui/core';

import NavItem from './navItem';
import navItems from './navItems';
import { useAuthContext } from '../../../modules/authorization/context';

const currentUser = {
    avatar: '/static/images/avatars/avatar_6.png',
    jobTitle: 'Senior Developer',
    name: 'Katarina Smith'
};


const useStyles = makeStyles(() => ({
    mobileDrawer: {
        width: 256
    },
    desktopDrawer: {
        width: 256,
        top: 64,
        height: 'calc(100% - 64px)'
    },
    avatar: {
        cursor: 'pointer',
        width: 64,
        height: 64
    }
}));

interface Props {
    onMobileClose: () => void;
    openMobile: boolean;
}

const NavBar = ({ onMobileClose, openMobile }: Props) => {
    const classes = useStyles();
    const location = useLocation();
    const { userProfile } = useAuthContext();

    useEffect(() => {
        if (openMobile && onMobileClose) {
            onMobileClose();
        }
    }, [location.pathname]);

    const items = navItems.map((item) => (
        <NavItem href={item.href} key={item.title} title={item.title} icon={item.icon} />
    ));

    const content = (
        <Box height="100%" display="flex" flexDirection="column">
            <Box
                alignItems="center" display="flex" flexDirection="column" p={2}>
                <Avatar
                    className={classes.avatar}
                    component={RouterLink}
                    src={currentUser.avatar}
                    to="/admin/account"
                />
                <Typography className={classes.avatar} color="textPrimary" variant="h5">
                    {userProfile.firstName} {userProfile.lastName}
                </Typography>
                <Typography color="textSecondary" variant="body2">
                    {currentUser.jobTitle}
                </Typography>
            </Box>
            <Divider />
            <Box p={2}>
                <List>{items}</List>
            </Box>
            <Box flexGrow={1} />
            <Box p={2} m={2} bgcolor="background.dark">
                <Typography
                    align="center" gutterBottom variant="h4">
                    Need more?
                </Typography>
                <Typography align="center" variant="body2">
                    Some text
                </Typography>
                <Box display="flex" justifyContent="center" mt={2}>
                    <Button
                        color="primary" component="a" variant="contained">
                        See more
                    </Button>
                </Box>
            </Box>
        </Box>
    );

    return (
        <>
            <Hidden lgUp>
                <Drawer
                    anchor="left"
                    classes={{ paper: classes.mobileDrawer }}
                    onClose={onMobileClose}
                    open={openMobile}
                    variant="temporary"
                >
                    {content}
                </Drawer>
            </Hidden>
            <Hidden mdDown>
                <Drawer anchor="left" classes={{ paper: classes.desktopDrawer }} open variant="persistent">
                    {content}
                </Drawer>
            </Hidden>
        </>
    );
};

export default NavBar;
