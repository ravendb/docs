import { test } from "node:test";
import assert from "node:assert/strict";
import path from "node:path";
import { fileURLToPath } from "node:url";

import {
    buildRedirectMap,
    loadRedirects,
    resolveChain,
    validateRedirects,
    type RedirectRule,
} from "../lib/redirects.js";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const FIXTURE = path.join(__dirname, "fixtures", "redirects.sample.json");

test("loadRedirects parses the sample fixture", () => {
    const rules = loadRedirects(FIXTURE);
    assert.equal(rules.length, 4);
    assert.equal(rules[0].key, "/old/a");
});

test("validateRedirects accepts a well-formed file", () => {
    const rules = loadRedirects(FIXTURE);
    const errors = validateRedirects(rules);
    assert.deepEqual(errors, []);
});

test("validateRedirects flags missing leading slash on targetUrl", () => {
    const bad: RedirectRule[] = [{ key: "/ok", value: { targetUrl: "no-slash" } }];
    const errors = validateRedirects(bad);
    assert.equal(errors.length, 1);
    assert.match(errors[0].message, /targetUrl/);
});

test("validateRedirects flags duplicate keys", () => {
    const bad: RedirectRule[] = [
        { key: "/dup", value: { targetUrl: "/a" } },
        { key: "/dup", value: { targetUrl: "/b" } },
    ];
    const errors = validateRedirects(bad);
    assert.ok(errors.some((e) => e.message === "duplicate 'key'"));
});

test("validateRedirects flags malformed minimumVersion", () => {
    const bad: RedirectRule[] = [
        { key: "/ok", value: { targetUrl: "/fine", minimumVersion: "seven-two" } },
    ];
    const errors = validateRedirects(bad);
    assert.equal(errors.length, 1);
    assert.match(errors[0].message, /minimumVersion/);
});

test("validateRedirects rejects non-array input", () => {
    const errors = validateRedirects({ not: "an array" } as unknown);
    assert.equal(errors.length, 1);
    assert.match(errors[0].message, /array/);
});

test("resolveChain returns startPath when no rule matches", () => {
    const rules = loadRedirects(FIXTURE);
    const map = buildRedirectMap(rules);
    assert.equal(resolveChain(map, "/not-in-map", "7.2"), "/not-in-map");
});

test("resolveChain follows a single-hop redirect", () => {
    const rules = loadRedirects(FIXTURE);
    const map = buildRedirectMap(rules);
    assert.equal(resolveChain(map, "/old/a", "7.2"), "/new/a");
});

test("resolveChain follows a multi-hop chain", () => {
    const rules = loadRedirects(FIXTURE);
    const map = buildRedirectMap(rules);
    assert.equal(resolveChain(map, "/old/b", "7.2"), "/new/b");
});

test("resolveChain stops at a hop gated above the current version", () => {
    const rules = loadRedirects(FIXTURE);
    const map = buildRedirectMap(rules);
    // /old/a requires 7.2; at 7.1 the hop shouldn't apply.
    assert.equal(resolveChain(map, "/old/a", "7.1"), "/old/a");
});

test("resolveChain detects cycles and throws", () => {
    const cyclic: RedirectRule[] = [
        { key: "/a", value: { targetUrl: "/b" } },
        { key: "/b", value: { targetUrl: "/a" } },
    ];
    const map = buildRedirectMap(cyclic);
    assert.throws(() => resolveChain(map, "/a", "7.2"), /cycle/i);
});

test("buildRedirectMap has all keys from input", () => {
    const rules = loadRedirects(FIXTURE);
    const map = buildRedirectMap(rules);
    assert.equal(map.size, 4);
    assert.ok(map.has("/gated"));
});

test("parity: plugin compareVersions agrees with handle_redirects behavior", async () => {
    // The shared lib is what scripts/handle_redirects.js imports. This test
    // locks in behavior across 1-major and 2-minor boundaries so a future
    // inlined copy at the edge can be kept in lockstep.
    const { compareVersions } = await import("../../../../scripts/lib/compare-versions.js");
    const cases: Array<[string, string, number]> = [
        ["7.2", "7.2", 0],
        ["7.2", "7.1", 1],
        ["7.1", "7.2", -1],
        ["7.11", "7.2", 1],
        ["7.2", "7.11", -1],
        ["6.2", "7.0", -1],
        ["8.0", "7.2", 1],
        ["10.0", "9.9", 1],
    ];
    for (const [a, b, expected] of cases) {
        assert.equal(compareVersions(a, b), expected, `compareVersions(${a}, ${b})`);
    }
});
