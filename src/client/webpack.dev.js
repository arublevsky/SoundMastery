const webpack = require('webpack');
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
    plugins: [
        new webpack.DefinePlugin({
            __API_BASE_URL__: JSON.stringify("https://localhost:5001"),
            __FACEBOOK_APP_ID__: JSON.stringify("186246619844260"),
            __GOOGLE_CLIENT_ID__: JSON.stringify("48645631284-1m423c8ltmcg6cn6npi8bggmg172p215.apps.googleusercontent.com"),
            __MICROSOFT_CLIENT_ID__: JSON.stringify("19d8b35d-a98a-4a87-9670-4e59d0686005"),
        })
    ]
});