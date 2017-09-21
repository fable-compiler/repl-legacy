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
      filename: filename + '.js',
      path: resolve('./public'),
      publicPath: 'public',
      library: library
    },
    resolve: {
      modules: [resolve("./node_modules/")]
    },
    devServer: {
      // If we use public we won't have access to node_modules
      contentBase: resolve('.'),
      port: 8080
    },
    externals: {
      "monaco": "var monaco",
      "editor": "var editor",
      "FableREPL": "var FableREPL",
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
          test: /\.sass$/,
          use: [
              "style-loader",
              "css-loader",
              "sass-loader"
          ]
      }      
      ]
    }
  };
}

module.exports = [
  getWebpackConfig('./src/App/App.fsproj', 'bundle'),
  getWebpackConfig('./src/Monaco/Monaco.fsproj', 'editor', 'editor')
]