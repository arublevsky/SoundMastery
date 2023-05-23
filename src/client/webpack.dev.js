const webpack = require('webpack');
const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');
const path = require('path');

module.exports = merge(common, {
    mode: 'development',
    devtool: 'inline-source-map',
    devServer: {
        static: {
            directory: path.resolve(__dirname, "dist"),
        },
        compress: true,
        historyApiFallback: true,
        port: 9000,
        hot: true,
    },
    plugins: [
        new webpack.DefinePlugin({
            __API_BASE_URL__: JSON.stringify("http://localhost:5000"),
            __CLIENT_APP_BASE_URL__: JSON.stringify("http://localhost:9000"),
            __FACEBOOK_APP_ID__: JSON.stringify("186246619844260"),
            __GOOGLE_CLIENT_ID__: JSON.stringify("48645631284-1m423c8ltmcg6cn6npi8bggmg172p215.apps.googleusercontent.com"),
            __MICROSOFT_CLIENT_ID__: JSON.stringify("19d8b35d-a98a-4a87-9670-4e59d0686005"),
        })
    ]
});