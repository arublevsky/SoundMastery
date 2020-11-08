import { Alert, AlertTitle } from '@material-ui/lab';
import React from 'react';
import { ApplicationError } from './../../modules/common/errorHandling';

interface Props {
    title: string;
    show: boolean;
    errors: ApplicationError[];
}

export const ErrorAlert = ({ show, title, errors }: Props) => (
    <>
        {show &&
            <Alert severity="error">
                <AlertTitle>{title}</AlertTitle>
                {errors.length > 0 &&
                    <ul>
                        {errors.map((e) => (<li key={e.code}>{e.description}</li>))}
                    </ul>}
            </Alert>
        }
    </>
);

