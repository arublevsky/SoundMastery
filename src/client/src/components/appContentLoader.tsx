import React from "react";
import { makeStyles } from '@mui/styles';
import ContentLoader from 'react-content-loader';

const useStyles = makeStyles(() => ({
    loader: {
        flex: 1,
        alignItems: 'center',
        justifyContent: 'center'
    }
}));


const AppContentLoader = () => {
    const classes = useStyles();

    return (
        <div className={classes.loader}>
            <ContentLoader viewBox="0 0 400 160" backgroundColor="transparent">
                <circle cx="150" cy="86" r="4" />
                <circle cx="194" cy="86" r="4" />
                <circle cx="238" cy="86" r="4" />
            </ContentLoader>
        </div>
    );
};

export default AppContentLoader;