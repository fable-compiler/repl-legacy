var path = require('path');

function resolve(filePath) {
  return path.resolve(__dirname, filePath)
}

var babelOptions = {
  presets: [["es2015", {"modules": false}]]
};

var fableOptions = {
  // babel: babelOptions,
  //plugins: [],
  define: [
    "COMPILER_PUBLIC_API",
    "FX_NO_CORHOST_SIGNER",
    "FX_NO_LINKEDRESOURCES",
    "FX_NO_PDB_READER",
    "FX_NO_PDB_WRITER",
    "FX_NO_WEAKTABLE",
    "FX_REDUCED_EXCEPTIONS",
    "NO_COMPILER_BACKEND",
    "NO_INLINE_IL_PARSER"
  ]
};

module.exports = {
  //devtool: "source-map",
  entry: resolve('./fable-repl.fsproj'),
  output: {
    filename: 'bundle.js',
    path: resolve('./out'),
  },
  externals: {
    monaco: "monaco"
  },
  module: {
    rules: [
      {
        test: /\.fs(x|proj)?$/,
        use: {
          loader: "fable-loader",
          options: fableOptions
        }
      },
      // {
      //   test: /\.js$/,
      //   exclude: /node_modules\/(?!fable)/,
      //   use: {
      //     loader: "babel-loader",
      //     options: babelOptions
      //   },
      // }
    ]
  },
};
