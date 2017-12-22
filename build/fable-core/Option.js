define(["require", "exports", "./Util"], function (require, exports, Util_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    // Options are erased in runtime by Fable, but some cases
    // (unit, not resolved generics, nested options) are wrapped
    // by the following JS type (for Some cases)
    // So options in Fable follow these two rules:
    // 1- None is always null in runtime, so a non-strict null check
    //    (`x == null`) is enough to check the case of an option.
    // 2- To get the value of an option the `getValue` helper
    //    below must **always** be used.
    class Some {
        constructor(value) {
            this.value = value;
            this.value = value;
        }
        // We don't prefix it with "Some" for consistency with erased options
        ToString() {
            return Util_1.toString(this.value);
        }
        Equals(other) {
            if (other == null) {
                return false;
            }
            else {
                return Util_1.equals(this.value, other instanceof Some
                    ? other.value : other);
            }
        }
        CompareTo(other) {
            if (other == null) {
                return 1;
            }
            else {
                return Util_1.compare(this.value, other instanceof Some
                    ? other.value : other);
            }
        }
    }
    exports.Some = Some;
    function getValue(x, acceptNull) {
        if (x == null) {
            if (!acceptNull) {
                throw new Error("Option has no value");
            }
            return null;
        }
        else {
            return x instanceof Some ? x.value : x;
        }
    }
    exports.getValue = getValue;
    function defaultArg(arg, defaultValue, f) {
        return arg == null ? defaultValue : (f != null ? f(getValue(arg)) : getValue(arg));
    }
    exports.defaultArg = defaultArg;
    function defaultArgWith(arg, defThunk) {
        return arg == null ? defThunk() : getValue(arg);
    }
    exports.defaultArgWith = defaultArgWith;
    function filter(predicate, arg) {
        return arg != null ? (!predicate(getValue(arg)) ? null : arg) : arg;
    }
    exports.filter = filter;
});
