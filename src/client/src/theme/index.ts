import { createMuiTheme } from '@mui/material';
import { indigo, common, blueGrey } from '@mui/material/colors';
import shadows from './shadows';
import typography from './typography';

const theme = createMuiTheme({
    palette: {
        background: {
            default: common.white,
            paper: common.white
        },
        primary: {
            main: indigo[500]
        },
        secondary: {
            main: indigo[500]
        },
        text: {
            primary: blueGrey[900],
            secondary: blueGrey[600]
        }
    },
    shadows,
    typography
});

export default theme;
