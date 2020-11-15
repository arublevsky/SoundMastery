const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');
const path = require('path');
const fs = require('fs');

module.exports = merge(common, {
    mode: 'development',
    devtool: 'inline-source-map',
    devServer: {
        contentBase: path.join(__dirname, 'dist'),
        compress: true,
        historyApiFallback: true,
        port: 9000,
        hot: true,
        https: true,
        key: fs.readFileSync(path.resolve(__dirname, './../../tools/docker/ssl/client/private.key')),
        cert: fs.readFileSync(path.resolve(__dirname, './../../tools/docker/ssl/client/private.crt')),
        ca: fs.readFileSync(path.resolve(__dirname, './../../tools/docker/ssl/client/private.pem')),
    },
});