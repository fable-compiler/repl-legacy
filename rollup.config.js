import fable from 'rollup-plugin-fable';

// var babelOptions = {
//   presets: [["es2015", {"modules": false}]]
// };

var fableOptions = {
  // babel: babelOptions,
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
  input: './fable-repl.fsproj',
  output: {
    file: './out/bundle.js',
    format: 'iife', // 'amd', 'cjs', 'es', 'iife', 'umd'
  },
  globals: { monaco: "monaco" },
  name: 'Bundle',
  plugins: [
    fable(fableOptions),
  ],
};
