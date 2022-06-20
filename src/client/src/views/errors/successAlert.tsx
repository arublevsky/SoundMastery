import { Alert, AlertTitle } from '@mui/material';
import React from 'react';

interface Props {
    title: string;
    details?: string;
    show: boolean;
}

export const SuccessAlert = ({ show, title, details }: Props) => (
    <>
        {show &&
            <Alert severity="success">
                <AlertTitle>{title}</AlertTitle>
                {details}
            </Alert>
        }
    </>
);
