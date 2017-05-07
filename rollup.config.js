import fable from 'rollup-plugin-fable';
var path = require('path');

function resolve(filePath) {
  return path.resolve(__dirname, filePath)
}

// var babelOptions = {
//   presets: [["es2015", {"modules": false}]]
// };

var fableOptions = {
  //babel: babelOptions,
  //plugins: [],
  //define: []
};

export default {
  entry: resolve('./fable-repl.fsproj'),
  dest: resolve('./out/bundle.js'),
  format: 'iife', // 'amd', 'cjs', 'es', 'iife', 'umd'
  moduleName: 'Bundle',
  //sourceMap: 'inline',
  plugins: [
    fable(fableOptions),
  ],
};
