const webpack = require('webpack');
const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = merge(common, {
    mode: 'production',
    plugins: [
        new webpack.DefinePlugin({
            // when running in docker locally, replace with dev URL
            // to improve try use dockerfile ARG API_URL="http://something.com" ?
            __API_BASE_URL__: JSON.stringify("https://soundmastery.azurewebsites.net")
        })
    ]
});