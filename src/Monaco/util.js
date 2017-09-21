/// @ts-check

var checker = null;
var metadata = {}

// Files have .txt extension to allow gzipping in Github Pages
var references = [
    "mscorlib.dll",
    "System.dll",
    "System.Core.dll",
    "System.Data.dll",
    "System.IO.dll",
    "System.Xml.dll",
    "System.Numerics.dll",
    "FSharp.Core.sigdata",
    "FSharp.Core.dll",
    "Fable.Core.dll"
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

references.forEach(function(fileName){
    getFileBlob(fileName, 'metadata/' + fileName);
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

