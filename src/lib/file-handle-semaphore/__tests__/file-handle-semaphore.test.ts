import { test } from "node:test";
import assert from "node:assert/strict";
import { execFileSync } from "node:child_process";
import path from "node:path";

type Callback = (error: null, result: number) => void;
type FileSystem = {
    offset: number;
    readFile: {
        (value: number): Promise<number>;
        (value: number, callback: Callback): string;
    };
};

const PRELOAD = path.join(__dirname, "..", "..", "..", "..", "scripts", "limit-docusaurus-file-handles.cjs");

const { installReadFileSemaphore, installWriteFileSemaphore } = require(PRELOAD) as {
    installReadFileSemaphore: (fs: { readFile: unknown }, maxConcurrent: number) => void;
    installWriteFileSemaphore: (fs: { writeFile: unknown }, maxConcurrent: number) => void;
};

test("installReadFileSemaphore limits promise reads and preserves callback reads", async () => {
    let active = 0;
    let peak = 0;

    const readFile = function (this: FileSystem, value: number, callback?: Callback): Promise<number> | string {
        if (callback) {
            setImmediate(() => callback(null, value + this.offset));
            return "callback-return";
        }

        return new Promise<number>((resolve) => {
            active += 1;
            peak = Math.max(peak, active);
            setImmediate(() => {
                active -= 1;
                resolve(value + this.offset);
            });
        });
    } as FileSystem["readFile"];

    const fs: FileSystem = { offset: 10, readFile };
    installReadFileSemaphore(fs, 2);

    const results = await Promise.all(Array.from({ length: 8 }, (_, index) => fs.readFile(index)));
    assert.equal(peak, 2);
    assert.deepEqual(results, [10, 11, 12, 13, 14, 15, 16, 17]);

    const callbackResult = await new Promise<number>((resolve) => {
        const returnValue = fs.readFile(5, (_error, result) => resolve(result));
        assert.equal(returnValue, "callback-return");
    });
    assert.equal(callbackResult, 15);
});

test("installReadFileSemaphore releases a permit after a rejected read", async () => {
    const expectedError = new Error("expected");
    const fs = {
        readFile(value: number) {
            return value === 1 ? Promise.reject(expectedError) : Promise.resolve(value);
        },
    };

    installReadFileSemaphore(fs, 1);

    const results = await Promise.allSettled([fs.readFile(1), fs.readFile(2)]);
    assert.equal(results[0].status, "rejected");
    assert.deepEqual(results[1], { status: "fulfilled", value: 2 });
});

test("installWriteFileSemaphore bounds concurrent callback writes", async () => {
    let active = 0;
    let peak = 0;

    const fs = {
        writeFile(_file: string, _data: string, callback: (error: null) => void) {
            active += 1;
            peak = Math.max(peak, active);
            setImmediate(() => {
                active -= 1;
                callback(null);
            });
        },
    };

    installWriteFileSemaphore(fs, 2);

    await Promise.all(
        Array.from(
            { length: 8 },
            (_, index) => new Promise<void>((resolve) => fs.writeFile(`f${index}`, "x", () => resolve()))
        )
    );

    assert.equal(peak, 2);
});

test("installWriteFileSemaphore releases the permit when the write throws synchronously", async () => {
    const expectedError = new Error("expected");
    let secondWriteRan = false;

    const fs = {
        writeFile(file: string, _data: string, callback: (error: null) => void) {
            if (file === "boom") {
                throw expectedError;
            }

            secondWriteRan = true;
            setImmediate(() => callback(null));
        },
    };

    installWriteFileSemaphore(fs, 1);

    assert.throws(() => fs.writeFile("boom", "x", () => {}), expectedError);

    // The permit must be back in the pool, otherwise this write never starts.
    await new Promise<void>((resolve) => fs.writeFile("ok", "x", () => resolve()));
    assert.equal(secondWriteRan, true);
});

const MAX_CONCURRENT_FILE_WRITES = 256;
const HANDLE_COUNTER = path.join(__dirname, "fixtures", "count-open-handles.cjs");
const REPO_ROOT = path.join(__dirname, "..", "..", "..", "..");

// Mirrors the fan-out that broke the build: emitUtils.generate -> fs-extra.outputFile,
// one route-data file per route, all started at once.
const WRITE_FANOUT = `
    const os = require("node:os");
    const path = require("node:path");
    const fsExtra = require("fs-extra");
    const dir = path.join(os.tmpdir(), "write-fanout-" + process.pid);
    const writes = Array.from({ length: 2000 }, (_, i) => fsExtra.outputFile(path.join(dir, "f" + i + ".json"), "{}"));
    Promise.all(writes).then(() => {
        fsExtra.removeSync(dir);
        process.stdout.write(String(globalThis.__peakOpenHandles()));
    });
`;

function peakOpenHandles(preloads: string[]): number {
    const preloadArgs = preloads.flatMap((preload) => ["--require", preload]);
    const stdout = execFileSync(process.execPath, [...preloadArgs, "-e", WRITE_FANOUT], {
        encoding: "utf8",
        cwd: REPO_ROOT,
        // The child must never be able to hang CI; a stall is a failure, not a wait.
        timeout: 60_000,
    });

    return Number(stdout.trim());
}

// Guards the test below: if the fan-out ever stopped being exercised, that test
// would keep passing for the wrong reason. The threshold sits well clear of the
// bound on one side and of any platform handle limit on the other.
test("the fs-extra write fan-out is unbounded on its own", () => {
    const peak = peakOpenHandles([HANDLE_COUNTER]);

    assert.ok(peak > MAX_CONCURRENT_FILE_WRITES * 2, `expected an unbounded peak, saw ${peak}`);
});

test("the preload bounds the fs-extra write fan-out", () => {
    const peak = peakOpenHandles([HANDLE_COUNTER, PRELOAD]);

    assert.ok(peak > 0 && peak <= MAX_CONCURRENT_FILE_WRITES, `expected a bounded peak, saw ${peak}`);
});
