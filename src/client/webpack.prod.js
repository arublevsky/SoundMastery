const webpack = require('webpack');
const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = merge(common, {
    mode: 'production',
    plugins: [
        new webpack.DefinePlugin({
            // when running in k8s locally, replace both URLs with http://soundmastery-client.local 
            // TODO: to improve try use dockerfile ARG API_URL="http://something.com" 
            // https://github.com/arublevsky/SoundMastery/issues/86
            __API_BASE_URL__: JSON.stringify("https://soundmastery.azurewebsites.net"),
            __CLIENT_APP_BASE_URL__: JSON.stringify("https://soundmastery-client.azurewebsites.net"),
            __FACEBOOK_APP_ID__: JSON.stringify("186246619844260"),
            __GOOGLE_CLIENT_ID__: JSON.stringify("48645631284-1m423c8ltmcg6cn6npi8bggmg172p215.apps.googleusercontent.com"),
            __MICROSOFT_CLIENT_ID__: JSON.stringify("19d8b35d-a98a-4a87-9670-4e59d0686005"),
        })
    ]
});