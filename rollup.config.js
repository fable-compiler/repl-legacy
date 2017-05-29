import fable from 'rollup-plugin-fable';

var path = require('path');

function resolve(filePath) {
  return path.resolve(__dirname, filePath)
}

// var babelOptions = {
//   presets: [["es2015", {"modules": false}]]
// };

var fableOptions = {
  // babel: babelOptions,
  fableCore: "../../../Fable/build/fable-core",
  define: [
    "COMPILER_SERVICE",
    "FX_NO_CORHOST_SIGNER",
    "FX_NO_LINKEDRESOURCES",
    "FX_NO_PDB_READER",
    "FX_NO_PDB_WRITER",
    "FX_NO_WEAKTABLE",
    "NO_COMPILER_BACKEND",
    "NO_INLINE_IL_PARSER",
    "TRACE"
  ]
};

export default {
  entry: resolve('./fable-repl.fsproj'),
  dest: resolve('./out/bundle.js'),
  format: 'iife', // 'amd', 'cjs', 'es', 'iife', 'umd'
  globals: { monaco: "monaco" },
  moduleName: 'Bundle',
  plugins: [
    fable(fableOptions),
  ],
};
