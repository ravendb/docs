import { test } from "node:test";
import assert from "node:assert/strict";

type Callback = (error: null, result: number) => void;
type FileSystem = {
    offset: number;
    readFile: {
        (value: number): Promise<number>;
        (value: number, callback: Callback): string;
    };
};

const { installReadFileSemaphore } = require("../../../../scripts/limit-docusaurus-file-reads.cjs") as {
    installReadFileSemaphore: (fs: { readFile: unknown }, maxConcurrent: number) => void;
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
