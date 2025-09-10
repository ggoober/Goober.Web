const path = require('path');
const {CleanWebpackPlugin} = require('clean-webpack-plugin');
const outPath = './wwwroot/assets';
const CopyWebpackPlugin = require('copy-webpack-plugin');
const AngularWebpackPlugin = require('@ngtools/webpack').AngularWebpackPlugin;
const fs = require('fs');
const linkerPlugin = require('@angular/compiler-cli/linker/babel');


// COPY_WEBPACK_PLUGIN_CONFIGURATIONS ( @indusoft/* )
const NODE_MODULES_PATH = path.join(__dirname, 'node_modules/@indusoft');
const files = fs.readdirSync(NODE_MODULES_PATH);
let arrayPathToAssets = [
    { from: '_src/grid-view-page/assets', to: './' },
    { from: '_src/multiselectPage/assets', to: './' },
    { from: '_src/external-js-page/assets', to: './' }
];
files.forEach((file) => {
    const assetsDir = path.join(NODE_MODULES_PATH, file, 'assets');
    console.log(file + " is directory =  " + fs.existsSync(assetsDir).toString());
    if (fs.existsSync(assetsDir)) {
        const relativePathToAssetDir = `node_modules/@indusoft/${file}/assets`
        arrayPathToAssets.push({ from: relativePathToAssetDir, to: "./" })
    }
});
console.log('Assets folders which will be loaded');
console.log(arrayPathToAssets);


module.exports = {
    mode: 'development',
    entry: {
        'header': path.resolve(__dirname, './_src/header/header.main.ts'),
        'homePage': path.resolve(__dirname, './_src/homePage/app/index.ts'),
        'helpPage': path.resolve(__dirname, './_src/helpPage/app/index.ts'),
        'dateTimePickerPage': path.resolve(__dirname, './_src/dateTimePickerPage/app/index.ts'),
        'multiselectPage': path.resolve(__dirname, './_src/multiselectPage/app/index.ts'),
        'overlayPage': path.resolve(__dirname, './_src/overlayPage/app/index.ts'),
        'progressPage': path.resolve(__dirname, './_src/progressPage/app/index.ts'),
        'toastPage': path.resolve(__dirname, './_src/toastPage/app/index.ts'),
        'treeViewPage': path.resolve(__dirname, './_src/treeViewPage/app/index.ts'),
        'grid-view-page': path.resolve(__dirname, './_src/grid-view-page/index.ts'),
        'external-js-page': path.resolve(__dirname, './_src/external-js-page/index.ts'),
        'formPage': path.resolve(__dirname, './_src/formPage/app/index.ts'),
        'indexer-db-demo-page': path.resolve(__dirname, './_src/indexerDbPage/index.ts'),
        'formWithTreePage': path.resolve(__dirname, './_src/formWithTreePage/app/index.ts'),
        'dropdown-button-page': path.resolve(__dirname, './_src/dropdown-button-page/index.ts'),
        'toggle-button-group-page': path.resolve(__dirname, './_src/toggle-button-group-page/app/index.ts'),
        'grid-view-editor-page': path.resolve(__dirname, './_src/grid-view-editor-page/index.ts'),
        'accordion-page': path.resolve(__dirname, './_src/accordion/index.ts'),
        'chips-input-page': path.resolve(__dirname, './_src/chipsInput/index.ts'),
        'base-button-page': path.resolve(__dirname, './_src/baseButton/index.ts'),
        'confirm-dialog-page': path.resolve(__dirname, './_src/confirmDialog/index.ts'),
        'base-button-page': path.resolve(__dirname, './_src/baseButton/index.ts'),
        'color-picker-page': path.resolve(__dirname, './_src/colorPicker/index.ts'),
        'tree-input-page': path.resolve(__dirname, './_src/treeInputPage/index.ts'),
        'switch-page': path.resolve(__dirname, './_src/switchPage/index.ts'),
        'numeric-input-page': path.resolve(__dirname, './_src/numericInputPage/index.ts'),
        'context-menu-page': path.resolve(__dirname, './_src/contextMenuPage/index.ts'),
        'scroll-up-page': path.resolve(__dirname, './_src/scrollUpPage/index.ts'),
        'tooltip-page': path.resolve(__dirname, './_src/tooltipPage/index.ts')
    },
    output: {
        path: path.resolve(__dirname, outPath),
        filename: '[name].js',
        publicPath: ''
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                loader: '@ngtools/webpack'
            },
            {
                test: /\.(png|jpe?g|gif|svg|ico)$/,
                loader: 'file-loader',
                options: {
                    name: 'img/[name].[hash].[ext]',
                }
            },
            {
                test: /\.(woff|woff2|ttf|eot)$/,
                loader: 'file-loader',
                options: {
                    name: 'fonts/[name].[hash].[ext]',
                }
            },
            {
                test: /\.css$/,
                include: path.resolve(__dirname, '_src/'),
                use: [
                    'raw-loader',
                ]
            },
            {
                test: /\.scss$/,
                include: path.resolve(__dirname, '_src/'),
                use:
                    [
                        'raw-loader',
                        'sass-loader',
                    ]
            },
            {
                test: /\.html$/,
                use: 'raw-loader'
            },
            {
                test: /\.m?js$/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        plugins: [linkerPlugin],
                        compact: false,
                        cacheDirectory: true,
                    }
                }
            }
        ],
    },
    resolve: {
        extensions: ['.ts', '.js']
    },
    plugins: [
        new CleanWebpackPlugin({
            cleanOnceBeforeBuildPatterns: [path.resolve(process.cwd(), outPath)]
        }),
        new CopyWebpackPlugin({
            patterns: [
                ...
                arrayPathToAssets
            ]
        }),
        new AngularWebpackPlugin({
            tsconfig: './tsconfig.json',
            sourceMap: true,
            strictInjectionParameters: true,
            enableIvy: false
        })
    ],
    devtool: 'source-map'
};
