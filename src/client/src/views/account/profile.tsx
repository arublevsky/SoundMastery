import React from 'react';
import clsx from 'clsx';
import moment from 'moment';
import { useAuthContext } from '../../modules/authorization/context';
import {
    Avatar,
    Box,
    Button,
    Card,
    CardActions,
    CardContent,
    Divider,
    Typography
} from '@mui/material';
import { makeStyles } from '@mui/styles';

const user = {
    avatar: '/static/images/avatars/avatar_6.png',
    city: 'Los Angeles',
    country: 'USA',
    jobTitle: 'Senior Developer',
    name: 'Katarina Smith',
    timezone: 'GTM-7'
};

const useStyles = makeStyles(() => ({
    root: {},
    avatar: {
        height: 100,
        width: 100
    }
}));

interface Props {
    className?: string;
}

const Profile = ({ className }: Props) => {
    const classes = useStyles();
    const { userProfile } = useAuthContext();

    return (
        <Card className={clsx(classes.root, className)}>
            <CardContent>
                <Box alignItems="center" display="flex" flexDirection="column">
                    <Avatar className={classes.avatar} src={user.avatar} />
                    <Typography color="textPrimary" gutterBottom variant="h3">
                        {userProfile.firstName} {userProfile.lastName}
                    </Typography>
                    <Typography color="textSecondary" variant="body1">
                        {`${user.city} ${user.country}`}
                    </Typography>
                    <Typography color="textSecondary" variant="body1">
                        {`${moment().format('hh:mm A')} ${user.timezone}`}
                    </Typography>
                </Box>
            </CardContent>
            <Divider />
            <CardActions>
                <Button color="primary" fullWidth variant="text">
                    Upload picture
                </Button>
            </CardActions>
        </Card>
    );
};

export default Profile;
