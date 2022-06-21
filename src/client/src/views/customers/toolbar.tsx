import React from 'react';
import clsx from 'clsx';
import {
    Box,
    Button,
    Card,
    CardContent,
    TextField,
    InputAdornment,
    SvgIcon
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import { Search as SearchIcon } from 'react-feather';

const useStyles = makeStyles((theme) => ({
    root: {},
    importButton: {
        marginRight: theme.spacing(1)
    },
    exportButton: {
        marginRight: theme.spacing(1)
    }
}));

interface Props {
    className?: string;
}

const Toolbar = ({ className }: Props) => {
    const classes = useStyles();

    return (
        <div className={clsx(classes.root, className)}>
            <Box display="flex" justifyContent="flex-end">
                <Button className={classes.importButton}>Import</Button>
                <Button className={classes.exportButton}>Export</Button>
                <Button color="primary" variant="contained">Add customer</Button>
            </Box>
            <Box mt={3}>
                <Card>
                    <CardContent>
                        <Box maxWidth={500}>
                            <TextField
                                fullWidth
                                InputProps={{
                                    startAdornment: (
                                        <InputAdornment position="start">
                                            <SvgIcon fontSize="small" color="action">
                                                <SearchIcon />
                                            </SvgIcon>
                                        </InputAdornment>
                                    )
                                }}
                                placeholder="Search customer"
                                variant="outlined"
                            />
                        </Box>
                    </CardContent>
                </Card>
            </Box>
        </div>
    );
};

export default Toolbar;
