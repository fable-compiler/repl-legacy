var path = require("path");
var webpack = require("webpack");
var fableUtils = require("fable-utils");

function resolve(filePath) {
  return path.join(__dirname, filePath)
}

var babelOptions = fableUtils.resolveBabelOptions({
//   presets: [["es2015", { "modules": false }]],
//   plugins: ["transform-runtime"]
});

module.exports = (env) => {
    const isProduction = env !== undefined && env.production;
    console.log("Bundling for " + (isProduction ? "production" : "development") + "...");

    return {
        devtool: "source-map",
        entry: resolve("./src/App/App.fsproj"),
        output: {
          filename: "js/app.js",
          path: resolve('./public')
        },
        resolve: {
          modules: [resolve("./node_modules/")]
        },
        devServer: {
          contentBase: resolve('public'),
          port: 8080
        },
        externals: {
          "monaco": "var monaco",
          "editor": "var editor",
          "fable-repl": "var Fable",
        },
        module: {
          rules: [
            {
              test: /\.fs(x|proj)?$/,
              use: {
                loader: "fable-loader",
                options: {
                  babel: babelOptions,
                  define: isProduction ? [] : ["DEBUG"]
                }
              }
            },
            {
              test: /\.js$/,
              exclude: /node_modules/,
              use: {
                loader: 'babel-loader',
                options: babelOptions
              },
            },
            {
              test: /\.json$/,
              loader: 'json-loader'
            },
            {
              test: /\.scss$/,
              use: [
                "style-loader",
                "css-loader",
                "sass-loader",
                "postcss-loader"
              ]
            },
            {
              test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|ttf|eot)(\?.*$|$)/,
              use: ["file-loader"]
            }
          ]
        }
      };
}
