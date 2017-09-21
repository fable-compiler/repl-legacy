import * as path from 'path';
import fable from 'rollup-plugin-fable';

function resolve(filePath) {
  return path.join(__dirname, filePath)
}

// var babelOptions = {
//   presets: [["es2015", {"modules": false}]]
// };

var fableOptions = {
  // babel: babelOptions,
  fableCore: resolve("../Fable/build/fable-core"),
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

export default {
  input: resolve('./src/Fable.REPL/Fable.REPL.fsproj'),
  output: {
    file: resolve('./public/fable-repl.js'),
    format: 'iife', // 'amd', 'cjs', 'es', 'iife', 'umd'
  },
  name: 'FableREPL',
  plugins: [
    fable(fableOptions),
  ],
};
