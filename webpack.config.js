var path = require("path");
var webpack = require("webpack");
var fableUtils = require("fable-utils");

function resolve(filePath) {
  return path.join(__dirname, filePath)
}

var babelOptions = fableUtils.resolveBabelOptions({
  // presets: [["es2015", { "modules": false }]],
  // plugins: ["transform-runtime"]
});

var isProduction = process.argv.indexOf("-p") >= 0;
console.log("Bundling for " + (isProduction ? "production" : "development") + "...");

function getWebpackConfig(entry, filename, library) {
  return {
    devtool: "source-map",
    entry: resolve(entry),
    output: {
      filename: "js/" + filename + '.js',
      path: resolve('./public'),
      library: library,
      libraryTarget: "amd"
    },
    resolve: {
      modules: [resolve("./node_modules/")]
    },
    devServer: {
      contentBase: resolve('public'),
      port: 8080
    },
    externals: {
      "monaco": { root: "monaco" },
      "editor": "editor",
      "fable-repl": "fable-repl",
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

module.exports = [
  getWebpackConfig('./src/Monaco/Monaco.fsproj', 'editor', 'editor'),
  getWebpackConfig('./src/App/App.fsproj', 'app'),
]
