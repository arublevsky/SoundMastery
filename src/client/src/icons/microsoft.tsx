import React from 'react';
import { SvgIcon } from '@material-ui/core';

const Microsoft = (props: unknown) => {
    return (
        <SvgIcon {...props} viewBox="0 0 23 23">
            <path fill="#f3f3f3" d="M0 0h23v23H0z"></path>
            <path fill="#f35325" d="M1 1h10v10H1z"></path>
            <path fill="#81bc06" d="M12 1h10v10H12z"></path>
            <path fill="#05a6f0" d="M1 12h10v10H1z"></path>
            <path fill="#ffba08" d="M12 12h10v10H12z"></path>
        </SvgIcon>
    );
};

export default Microsoft;