import React from 'react';
import { Helmet } from 'react-helmet';

interface Props {
    children: React.ReactElement;
    title?: string;
    className: string;
}

const Page = ({ children, title }: Props) => (
    <div>
        <Helmet>
            <title>{title || ''}</title>
        </Helmet>
        {children}
    </div>
);

export default Page;
