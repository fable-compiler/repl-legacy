/// @ts-check

import * as Babel from "babel-standalone";
import BabelTemplate from "babel-template";
import * as BabelPlugins from "../../../Fable/src/js/fable-utils/babel-plugins";

var checker = null;
var metadata = {}

// Files have .txt extension to allow gzipping in Github Pages
var references = [
    "Fable.Core.dll",
    "mscorlib.dll",
    "System.Data.dll",
    "System.IO.dll",
    "System.Net.Requests.dll",
    "System.Runtime.dll",
    "System.Runtime.Serialization.Formatters.Soap.dll",
    "System.ValueTuple.dll",
    "System.Windows.Forms.dll",
    "Fable.Import.Browser.dll",
    "System.Collections.dll",
    "System.dll",
    "System.Linq.dll",
    "System.Numerics.dll",
    "System.Runtime.Numerics.dll",
    "System.Threading.dll",
    "System.Web.dll",
    "System.Xml.dll",
    "FSharp.Core.dll",
    "System.Core.dll",
    "System.Drawing.dll",
    "System.Linq.Expressions.dll",
    "System.Reflection.dll",
    "System.Runtime.Remoting.dll",
    "System.Threading.Tasks.dll",
    "System.Web.Services.dll",
];

function isSigdata(ref) {
    return ref.indexOf(".sigdata") >= 0;
}

function getFileBlob(key, url) {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", url + ".txt");
    xhr.responseType = "arraybuffer";
    xhr.onload = function (oEvent) {
        var arrayBuffer = xhr.response;
        if (arrayBuffer) {
            metadata[key] = new Uint8Array(arrayBuffer);
        }
    };
    xhr.onerror = function (oEvent) {
        console.log('Error loading ' + url);
    };
    xhr.send();
};

references.forEach(function (fileName) {
    getFileBlob(fileName, "metadata/" + fileName);
});

export function getChecker(createChecker) {
    if (checker === null) {
        if (Object.getOwnPropertyNames(metadata).length < references.length) {
            return null;
        }
        var readAllBytes = function (fileName) { return metadata[fileName]; }
        var references2 = references.filter(x => !isSigdata(x)).map(x => x.replace(".dll", ""));
        checker = createChecker(references2, readAllBytes)
    }
    return checker;
}

function babelOptions(extraPlugin) {
    var commonPlugins = [
        BabelPlugins.getTransformMacroExpressions(BabelTemplate),
        BabelPlugins.getRemoveUnneededNulls(),
    ];

    return {
        plugins:
            extraPlugin != null
                ? commonPlugins.concat(extraPlugin)
                : commonPlugins,
        filename: 'repl',
        babelrc: false,
    };
}

export function runAst(jsonAst) {
    try {
        var ast = JSON.parse(jsonAst);

        var optionsES2015 = babelOptions();
        var optionsAMD = babelOptions("transform-es2015-modules-amd");


        var codeES2015 = Babel.transformFromAst(ast, null, optionsES2015).code;
        var codeAMD = Babel.transformFromAst(ast, null, optionsAMD)
                        .code
                        .replace("define", "require")
                        .replace('"use strict";', '"use strict"; try { exports = exports || {}; } catch (err) {}');

        return [codeES2015, codeAMD];
    } catch (err) {
        console.error(err.message + "\n" + err.stack);
    }
}
