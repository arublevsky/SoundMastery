import React, { useState } from 'react';
import {
    Box,
    Container, Divider,
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import ReactPlayer from 'react-player';
import Webcam from "react-webcam";
import Page from '../../components/page';

const useStyles = makeStyles((theme) => ({
    root: {
        backgroundColor: theme.palette.background.default,
        minHeight: '100%',
        paddingBottom: theme.spacing(3),
        paddingTop: theme.spacing(3)
    },
    productCard: {
        height: '100%'
    }
}));

const VideoPage = () => {
    const classes = useStyles();
    const [mute, setMute] = useState(true);
    const [mirrored, setMirrored] = useState(false);

    return (
        <Page className={classes.root} title="Video">
            <Container maxWidth={false}>
                <Box display="flex" justifyContent="center" mt={2}>
                    <ReactPlayer url='https://www.youtube.com/watch?v=ysz5S6PUM-U' />
                </Box>

                <Divider />

                <Box display="flex" justifyContent="center" mt={2}>
                    <Webcam audio={!mute} mirrored={mirrored} />
                </Box>

                <Box display="flex" justifyContent="center" mt={2}>
                    <button onClick={() => setMute(!mute)}>Mute</button>
                    <button onClick={() => setMirrored(!mirrored)} >Mirror</button>
                </Box>
            </Container>
        </Page>
    );
};

export default VideoPage;