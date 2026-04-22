import { test } from "node:test";
import assert from "node:assert/strict";
import fs from "node:fs";
import os from "node:os";
import path from "node:path";
import { fileURLToPath } from "node:url";

import {
    buildRedirectMap,
    loadRedirects,
    resolveChain,
    validateNoCycles,
    validateRedirects,
    validateTargetsExist,
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
    const bad: RedirectRule[] = [{ key: "/ok", value: { targetUrl: "no-slash", minimumVersion: "7.2" } }];
    const errors = validateRedirects(bad);
    assert.equal(errors.length, 1);
    assert.match(errors[0].message, /targetUrl/);
});

test("validateRedirects flags duplicate keys", () => {
    const bad: RedirectRule[] = [
        { key: "/dup", value: { targetUrl: "/a", minimumVersion: "7.2" } },
        { key: "/dup", value: { targetUrl: "/b", minimumVersion: "7.2" } },
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

test("validateRedirects flags a versioned docs key missing minimumVersion", () => {
    // minimumVersion is required on docs-area (non-versionless) keys:
    // chain resolution for older-version readers depends on the gate.
    const bad: RedirectRule[] = [{ key: "/ok", value: { targetUrl: "/fine" } }];
    const errors = validateRedirects(bad);
    assert.equal(errors.length, 1);
    assert.match(errors[0].message, /minimumVersion/);
    assert.match(errors[0].message, /required/);
});

test("validateRedirects accepts a versionless /guides rule without minimumVersion", () => {
    // /guides and /cloud are versionless — minimumVersion is exempt.
    const good: RedirectRule[] = [{ key: "/guides/old", value: { targetUrl: "/guides/new" } }];
    assert.deepEqual(validateRedirects(good), []);
});

test("validateRedirects accepts a versionless /cloud rule without minimumVersion", () => {
    const good: RedirectRule[] = [{ key: "/cloud/old", value: { targetUrl: "/cloud/new" } }];
    assert.deepEqual(validateRedirects(good), []);
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
        { key: "/a", value: { targetUrl: "/b", minimumVersion: "7.2" } },
        { key: "/b", value: { targetUrl: "/a", minimumVersion: "7.2" } },
    ];
    const errors = validateNoCycles(rules);
    assert.equal(errors.length, 1);
    assert.match(errors[0].message, /cycle/i);
    assert.match(errors[0].message, /\/a.*\/b.*\/a/); // cycle includes both keys
});

test("validateNoCycles flags a self-loop", () => {
    const rules: RedirectRule[] = [{ key: "/loop", value: { targetUrl: "/loop", minimumVersion: "7.2" } }];
    const errors = validateNoCycles(rules);
    assert.equal(errors.length, 1);
    assert.match(errors[0].message, /cycle/i);
});

test("validateNoCycles reports each distinct cycle only once", () => {
    // Three keys in the same cycle: /a → /b → /c → /a. Whichever key we
    // start from, the cycle should only be reported once (not three
    // times) via the `explored` dedup.
    const rules: RedirectRule[] = [
        { key: "/a", value: { targetUrl: "/b", minimumVersion: "7.2" } },
        { key: "/b", value: { targetUrl: "/c", minimumVersion: "7.2" } },
        { key: "/c", value: { targetUrl: "/a", minimumVersion: "7.2" } },
    ];
    const errors = validateNoCycles(rules);
    assert.equal(errors.length, 1);
});

test("validateNoCycles accepts chains that share a tail without cycling", () => {
    // /a → /z, /b → /z — both chains terminate at /z which has no rule.
    // Exercises the "chain terminated cleanly" branch's `explored`
    // marking so the second chain doesn't re-walk the shared tail.
    const rules: RedirectRule[] = [
        { key: "/a", value: { targetUrl: "/z", minimumVersion: "7.2" } },
        { key: "/b", value: { targetUrl: "/z", minimumVersion: "7.2" } },
    ];
    assert.deepEqual(validateNoCycles(rules), []);
});

// --- validateTargetsExist ------------------------------------------------
//
// Builds a throwaway project root with a small fake content tree, then
// exercises each resolution path (flat file, index file, prefixed content
// root, missing target).

function makeTempProject(): string {
    const root = fs.mkdtempSync(path.join(os.tmpdir(), "redirects-test-"));
    // docs/ — the default content root for un-prefixed paths.
    fs.mkdirSync(path.join(root, "docs", "licensing"), { recursive: true });
    fs.writeFileSync(path.join(root, "docs", "licensing", "overview.mdx"), "# Overview\n");
    fs.mkdirSync(path.join(root, "docs", "querying"), { recursive: true });
    fs.writeFileSync(path.join(root, "docs", "querying", "index.md"), "# Querying\n");
    // guides/ — prefixed content root.
    fs.mkdirSync(path.join(root, "guides"), { recursive: true });
    fs.writeFileSync(path.join(root, "guides", "kubernetes.mdx"), "# K8s\n");
    return root;
}

test("validateTargetsExist accepts a flat .mdx target", () => {
    const root = makeTempProject();
    const rules: RedirectRule[] = [
        { key: "/old/licensing", value: { targetUrl: "/licensing/overview", minimumVersion: "7.2" } },
    ];
    assert.deepEqual(validateTargetsExist(rules, root), []);
});

test("validateTargetsExist accepts an index.md target (directory-as-page)", () => {
    const root = makeTempProject();
    const rules: RedirectRule[] = [{ key: "/old/q", value: { targetUrl: "/querying", minimumVersion: "7.2" } }];
    assert.deepEqual(validateTargetsExist(rules, root), []);
});

test("validateTargetsExist routes /guides/* to guides/ content root", () => {
    const root = makeTempProject();
    const rules: RedirectRule[] = [
        { key: "/old/guide", value: { targetUrl: "/guides/kubernetes", minimumVersion: "7.2" } },
    ];
    assert.deepEqual(validateTargetsExist(rules, root), []);
});

test("validateTargetsExist flags a target with no matching file", () => {
    const root = makeTempProject();
    const rules: RedirectRule[] = [
        { key: "/old/dead", value: { targetUrl: "/nowhere/to/be/found", minimumVersion: "7.2" } },
    ];
    const errors = validateTargetsExist(rules, root);
    assert.equal(errors.length, 1);
    assert.match(errors[0].message, /does not resolve/);
    assert.match(errors[0].message, /\/nowhere\/to\/be\/found/);
    assert.equal(errors[0].key, "/old/dead");
});

test("validateTargetsExist reports each broken target independently", () => {
    const root = makeTempProject();
    const rules: RedirectRule[] = [
        { key: "/ok", value: { targetUrl: "/licensing/overview", minimumVersion: "7.2" } },
        { key: "/broken-a", value: { targetUrl: "/does-not-exist-a", minimumVersion: "7.2" } },
        { key: "/broken-b", value: { targetUrl: "/does-not-exist-b", minimumVersion: "7.2" } },
    ];
    const errors = validateTargetsExist(rules, root);
    assert.equal(errors.length, 2);
    assert.equal(errors[0].key, "/broken-a");
    assert.equal(errors[0].index, 1);
    assert.equal(errors[1].key, "/broken-b");
    assert.equal(errors[1].index, 2);
});

test("validateTargetsExist rejects a bare directory with no index file", () => {
    // A directory backed only by _category_.json without an explicit
    // index.{mdx,md} isn't a valid redirect endpoint — Docusaurus may or
    // may not auto-generate a page for it, so requiring the explicit file
    // keeps authoring intent unambiguous.
    const root = makeTempProject();
    fs.mkdirSync(path.join(root, "docs", "bare-category"), { recursive: true });
    fs.writeFileSync(path.join(root, "docs", "bare-category", "_category_.json"), '{"label": "Bare"}');
    const rules: RedirectRule[] = [{ key: "/old", value: { targetUrl: "/bare-category", minimumVersion: "7.2" } }];
    const errors = validateTargetsExist(rules, root);
    assert.equal(errors.length, 1);
});

test("validateTargetsExist resolves against the real project (smoke)", () => {
    // Sanity check against the actual repo — catches any drift between
    // resolveContentPath and docusaurus.config.ts's content roots.
    const projectRoot = path.join(__dirname, "..", "..", "..", "..");
    const redirectsPath = path.join(projectRoot, "scripts", "redirects.json");
    const rules = loadRedirects(redirectsPath);
    assert.deepEqual(validateTargetsExist(rules, projectRoot), []);
});
