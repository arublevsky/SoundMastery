// eslint-disable-next-line no-unused-vars
import React from 'react';
import ReactDOM from 'react-dom';

const title = 'My M2222inimal React Webpack Babel Setup';

ReactDOM.render(
    <div>{title}</div>,
    document.getElementById('app'),
);

module.hot.accept();
