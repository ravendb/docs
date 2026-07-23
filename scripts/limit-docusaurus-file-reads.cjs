const { createRequire } = require("node:module");

// Resolve through @docusaurus/core, which declares fs-extra directly, instead
// of relying on it being hoisted as an undeclared project dependency.
const requireFromDocusaurus = createRequire(require.resolve("@docusaurus/core/package.json"));
const fsExtra = requireFromDocusaurus("fs-extra");

// Docusaurus has two independent, unbounded read fan-outs: loading Markdown
// across all versions and checking generated route data. Together they can
// exceed Node's Windows open-file ceiling. Bound only the promise-based reads;
// version loading, parsing, route generation, and non-Windows builds stay fully
// parallel. 256 keeps a wide margin below the platform's failure point.
const MAX_CONCURRENT_FILE_READS = 256;

function installReadFileSemaphore(fs, maxConcurrent) {
    const originalReadFile = fs.readFile;
    const waiters = [];
    let availablePermits = maxConcurrent;

    function acquire() {
        if (availablePermits > 0) {
            availablePermits -= 1;
            return;
        }

        return new Promise((resolve) => waiters.push(resolve));
    }

    function release() {
        const next = waiters.shift();
        if (next) {
            next();
        } else {
            availablePermits += 1;
        }
    }

    async function readWithPermit(receiver, args) {
        await acquire();
        try {
            return await originalReadFile.apply(receiver, args);
        } finally {
            release();
        }
    }

    fs.readFile = function readFileWithSemaphore(...args) {
        // Preserve callback-style calls; Docusaurus uses the promise form.
        if (typeof args[args.length - 1] === "function") {
            return originalReadFile.apply(this, args);
        }

        return readWithPermit(this, args);
    };
}

if (process.platform === "win32") {
    installReadFileSemaphore(fsExtra, MAX_CONCURRENT_FILE_READS);
}

module.exports = { installReadFileSemaphore };
