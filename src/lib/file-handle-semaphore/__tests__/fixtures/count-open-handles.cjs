// Counts concurrently open file handles. Must be the first --require preload so
// graceful-fs captures these wrappers instead of the pristine node:fs ones.
const fs = require("node:fs");

let live = 0;
let peak = 0;

const originalOpen = fs.open;
fs.open = function (...args) {
    const callback = args[args.length - 1];

    if (typeof callback !== "function") {
        return originalOpen.apply(this, args);
    }

    args[args.length - 1] = function (error) {
        if (!error) {
            live += 1;
            peak = Math.max(peak, live);
        }

        return callback.apply(this, arguments);
    };

    return originalOpen.apply(this, args);
};

const originalClose = fs.close;
fs.close = function (...args) {
    live -= 1;
    return originalClose.apply(this, args);
};

globalThis.__peakOpenHandles = () => peak;
