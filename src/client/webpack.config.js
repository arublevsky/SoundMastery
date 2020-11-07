const path = require('path');
const webpack = require('webpack');
const ESLintPlugin = require('eslint-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CopyPlugin = require('copy-webpack-plugin');

module.exports = {
    entry: './src/index.tsx',
    devtool: 'source-map',
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
        ],
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js'],
    },
    plugins: [
        new ESLintPlugin({ extensions: [".tsx", ".ts"] }),
        new webpack.HotModuleReplacementPlugin(),
        new HtmlWebpackPlugin({ template: 'src/index.html' }),
        new CopyPlugin({
            patterns: [
                { from: 'src/static', to: 'static' },
            ],
        }),
    ],
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'dist'),
        publicPath: '/'
    },
    devServer: {
        contentBase: path.join(__dirname, 'dist'),
        compress: true,
        historyApiFallback: true,
        port: 9000,
        hot: true,
    }
};
