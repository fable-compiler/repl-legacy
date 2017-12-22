define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    function isChar(input) {
        return typeof input === "string" && input.length === 1;
    }
    function isLetter(input) {
        return isChar(input) && input.toLowerCase() !== input.toUpperCase();
    }
    exports.isLetter = isLetter;
    function isUpper(input) {
        return isLetter(input) && input.toUpperCase() === input;
    }
    exports.isUpper = isUpper;
    function isLower(input) {
        return isLetter(input) && input.toLowerCase() === input;
    }
    exports.isLower = isLower;
    function isDigit(input) {
        return isChar(input) && /\d/.test(input);
    }
    exports.isDigit = isDigit;
    function isLetterOrDigit(input) {
        return isChar(input) &&
            (input.toLowerCase() !== input.toUpperCase() || /\d/.test(input));
    }
    exports.isLetterOrDigit = isLetterOrDigit;
    function isWhitespace(input) {
        return isChar(input) && /\s/.test(input);
    }
    exports.isWhitespace = isWhitespace;
});
