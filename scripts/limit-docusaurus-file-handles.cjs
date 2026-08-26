const { createRequire } = require("node:module");
const nodeFs = require("node:fs");

const MAX_CONCURRENT_FILE_WRITES = 256;
const MAX_CONCURRENT_FILE_READS = 256;

function createPermitPool(maxConcurrent) {
    const waiters = [];
    let availablePermits = maxConcurrent;

    return {
        acquire() {
            if (availablePermits > 0) {
                availablePermits -= 1;
                return undefined;
            }

            return new Promise((resolve) => waiters.push(resolve));
        },
        release() {
            const next = waiters.shift();

            if (next) {
                next();
            } else {
                availablePermits += 1;
            }
        },
    };
}

function installReadFileSemaphore(fs, maxConcurrent) {
    const originalReadFile = fs.readFile;
    const pool = createPermitPool(maxConcurrent);

    async function readWithPermit(receiver, args) {
        await pool.acquire();

        try {
            return await originalReadFile.apply(receiver, args);
        } finally {
            pool.release();
        }
    }

    fs.readFile = function readFileWithSemaphore(...args) {
        if (typeof args[args.length - 1] === "function") {
            return originalReadFile.apply(this, args);
        }

        return readWithPermit(this, args);
    };
}

function installWriteFileSemaphore(fs, maxConcurrent) {
    const originalWriteFile = fs.writeFile;
    const pool = createPermitPool(maxConcurrent);

    fs.writeFile = function writeFileWithSemaphore(...args) {
        const callback = args[args.length - 1];

        if (typeof callback !== "function") {
            return originalWriteFile.apply(this, args);
        }

        const receiver = this;
        let released = false;

        function releasePermit() {
            if (!released) {
                released = true;
                pool.release();
            }
        }

        args[args.length - 1] = function releaseThenCallback(...results) {
            releasePermit();
            return callback.apply(this, results);
        };

        function runWrite() {
            try {
                originalWriteFile.apply(receiver, args);
            } catch (error) {
                releasePermit();
                throw error;
            }
        }

        const permit = pool.acquire();

        // Never return a value here: callback-form writeFile yields undefined, and
        // handing back a promise makes util.promisify emit DEP0174 on every call.
        if (permit === undefined) {
            runWrite();
            return undefined;
        }

        // Queued, so the synchronous channel is gone and errors go to the callback.
        permit.then(runWrite).catch(callback);

        return undefined;
    };
}

// Must run before the requires below: graceful-fs and fs-extra both capture
// their fs references at load time.
installWriteFileSemaphore(nodeFs, MAX_CONCURRENT_FILE_WRITES);

require("graceful-fs").gracefulify(nodeFs);

const requireFromDocusaurus = createRequire(require.resolve("@docusaurus/core/package.json"));
installReadFileSemaphore(requireFromDocusaurus("fs-extra"), MAX_CONCURRENT_FILE_READS);

module.exports = { installReadFileSemaphore, installWriteFileSemaphore };
