import React from 'react';

const Logo = (props: unknown) => {
    return (
        <img
            alt="Logo"
            src="/static/logo.svg"
            {...props}
        />
    );
};

export default Logo;
