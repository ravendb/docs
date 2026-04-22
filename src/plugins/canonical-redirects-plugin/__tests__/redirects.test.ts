import { test } from "node:test";
import assert from "node:assert/strict";
import path from "node:path";
import { fileURLToPath } from "node:url";

import {
    buildRedirectMap,
    loadRedirects,
    resolveChain,
    validateNoCycles,
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

// No "resolveChain detects cycles" test: validateNoCycles runs upstream
// (CLI + loadContent) and guarantees cycles can't reach resolveChain.
// Feeding it a cyclic map on purpose would hang — validateNoCycles's own
// tests cover the cycle-detection surface.

test("buildRedirectMap has all keys from input", () => {
    const rules = loadRedirects(FIXTURE);
    const map = buildRedirectMap(rules);
    assert.equal(map.size, 4);
    assert.ok(map.has("/gated"));
});

test("validateNoCycles accepts an acyclic rule set", () => {
    const rules = loadRedirects(FIXTURE);
    assert.deepEqual(validateNoCycles(rules), []);
});

test("validateNoCycles flags a two-rule cycle", () => {
    const rules: RedirectRule[] = [
        { key: "/a", value: { targetUrl: "/b" } },
        { key: "/b", value: { targetUrl: "/a" } },
    ];
    const errors = validateNoCycles(rules);
    assert.equal(errors.length, 1);
    assert.match(errors[0].message, /cycle/i);
    assert.match(errors[0].message, /\/a.*\/b.*\/a/); // cycle includes both keys
});

test("validateNoCycles flags a self-loop", () => {
    const rules: RedirectRule[] = [{ key: "/loop", value: { targetUrl: "/loop" } }];
    const errors = validateNoCycles(rules);
    assert.equal(errors.length, 1);
    assert.match(errors[0].message, /cycle/i);
});

test("validateNoCycles reports each distinct cycle only once", () => {
    // Three keys in the same cycle: /a → /b → /c → /a. Whichever key we
    // start from, the cycle should only be reported once (not three
    // times) via the `explored` dedup.
    const rules: RedirectRule[] = [
        { key: "/a", value: { targetUrl: "/b" } },
        { key: "/b", value: { targetUrl: "/c" } },
        { key: "/c", value: { targetUrl: "/a" } },
    ];
    const errors = validateNoCycles(rules);
    assert.equal(errors.length, 1);
});

test("validateNoCycles accepts chains that share a tail without cycling", () => {
    // /a → /z, /b → /z — both chains terminate at /z which has no rule.
    // Exercises the "chain terminated cleanly" branch's `explored`
    // marking so the second chain doesn't re-walk the shared tail.
    const rules: RedirectRule[] = [
        { key: "/a", value: { targetUrl: "/z" } },
        { key: "/b", value: { targetUrl: "/z" } },
    ];
    assert.deepEqual(validateNoCycles(rules), []);
});
