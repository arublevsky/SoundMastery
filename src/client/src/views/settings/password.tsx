import React, { useState } from 'react';
import clsx from 'clsx';
import {
    Box,
    Button,
    Card,
    CardContent,
    CardHeader,
    Divider,
    TextField
} from '@mui/material';
import { makeStyles } from '@mui/styles';

const useStyles = makeStyles(({ root: {} }));

interface Props {
    className?: string;
}

const Password = ({ className }: Props) => {
    const classes = useStyles();
    const [values, setValues] = useState({
        password: '',
        confirm: ''
    });

    const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setValues({
            ...values,
            [event.target.name]: event.target.value
        });
    };

    return (
        <form className={clsx(classes.root, className)}>
            <Card>
                <CardHeader subheader="Update password" title="Password" />
                <Divider />
                <CardContent>
                    <TextField
                        fullWidth
                        label="Password"
                        margin="normal"
                        name="password"
                        onChange={handleChange}
                        type="password"
                        value={values.password}
                        variant="outlined"
                    />
                    <TextField
                        fullWidth
                        label="Confirm password"
                        margin="normal"
                        name="confirm"
                        onChange={handleChange}
                        type="password"
                        value={values.confirm}
                        variant="outlined"
                    />
                </CardContent>
                <Divider />
                <Box display="flex" justifyContent="flex-end" p={2}>
                    <Button color="primary" variant="contained">
                        Update
                    </Button>
                </Box>
            </Card>
        </form>
    );
};

export default Password;
