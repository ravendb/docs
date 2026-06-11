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
import { BUILT_VERSIONS, CURRENT_VERSION } from "../../../../scripts/lib/version-policy.js";

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
    const bad: RedirectRule[] = [{ key: "/ok", value: { targetUrl: "/fine", minimumVersion: "seven-two" } }];
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
    assert.deepEqual(validateTargetsExist(rules, root, "7.2", ["7.2"]), []);
});

test("validateTargetsExist accepts an index.md target (directory-as-page)", () => {
    const root = makeTempProject();
    const rules: RedirectRule[] = [{ key: "/old/q", value: { targetUrl: "/querying", minimumVersion: "7.2" } }];
    assert.deepEqual(validateTargetsExist(rules, root, "7.2", ["7.2"]), []);
});

test("validateTargetsExist routes /guides/* to guides/ content root", () => {
    const root = makeTempProject();
    const rules: RedirectRule[] = [
        { key: "/old/guide", value: { targetUrl: "/guides/kubernetes", minimumVersion: "7.2" } },
    ];
    assert.deepEqual(validateTargetsExist(rules, root, "7.2", ["7.2"]), []);
});

test("validateTargetsExist flags a target with no matching file", () => {
    const root = makeTempProject();
    const rules: RedirectRule[] = [
        { key: "/old/dead", value: { targetUrl: "/nowhere/to/be/found", minimumVersion: "7.2" } },
    ];
    const errors = validateTargetsExist(rules, root, "7.2", ["7.2"]);
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
    const errors = validateTargetsExist(rules, root, "7.2", ["7.2"]);
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
    const errors = validateTargetsExist(rules, root, "7.2", ["7.2"]);
    assert.equal(errors.length, 1);
});

test("validateTargetsExist resolves against the real project (smoke)", () => {
    // Sanity check against the actual repo — catches any drift between
    // resolveContentPath and docusaurus.config.ts's content roots.
    const projectRoot = path.join(__dirname, "..", "..", "..", "..");
    const redirectsPath = path.join(projectRoot, "scripts", "redirects.json");
    const rules = loadRedirects(redirectsPath);
    assert.deepEqual(validateTargetsExist(rules, projectRoot, CURRENT_VERSION, BUILT_VERSIONS), []);
});

// --- validateTargetsExist (multi-version) -------------------------------
//
// Catches redirects whose target only exists on newer versions: a rule with
// minimumVersion=7.0 pointing at a 7.1-only page passes the structural check
// but ships a redirect-to-404 for 7.0 readers.

function makeMultiVersionProject(): string {
    const root = fs.mkdtempSync(path.join(os.tmpdir(), "redirects-multi-"));
    // current 7.2 — has vector-search/start
    fs.mkdirSync(path.join(root, "docs", "ai", "vector-search"), { recursive: true });
    fs.writeFileSync(path.join(root, "docs", "ai", "vector-search", "start.mdx"), "");
    // 7.1 — also has start
    fs.mkdirSync(path.join(root, "versioned_docs", "version-7.1", "ai", "vector-search"), { recursive: true });
    fs.writeFileSync(path.join(root, "versioned_docs", "version-7.1", "ai", "vector-search", "start.mdx"), "");
    // 7.0 — directory exists but no start.mdx (the bug shape)
    fs.mkdirSync(path.join(root, "versioned_docs", "version-7.0", "ai", "vector-search"), { recursive: true });
    // versionless /guides — has kubernetes
    fs.mkdirSync(path.join(root, "guides"), { recursive: true });
    fs.writeFileSync(path.join(root, "guides", "kubernetes.mdx"), "");
    return root;
}

test("validateTargetsExist multi-version: flags target missing on a version >= minimumVersion", () => {
    const root = makeMultiVersionProject();
    const rules: RedirectRule[] = [
        { key: "/ai/old", value: { targetUrl: "/ai/vector-search/start", minimumVersion: "7.0" } },
    ];
    const errors = validateTargetsExist(rules, root, "7.2", ["7.2", "7.1", "7.0"]);
    assert.equal(errors.length, 1);
    assert.match(errors[0].message, /version 7\.0/);
    assert.equal(errors[0].key, "/ai/old");
});

test("validateTargetsExist multi-version: respects minimumVersion (versions below the gate are skipped)", () => {
    const root = makeMultiVersionProject();
    // minVer 7.1 — 7.0 isn't checked even though target is missing there.
    const rules: RedirectRule[] = [
        { key: "/ai/old", value: { targetUrl: "/ai/vector-search/start", minimumVersion: "7.1" } },
    ];
    assert.deepEqual(validateTargetsExist(rules, root, "7.2", ["7.2", "7.1", "7.0"]), []);
});

test("validateTargetsExist multi-version: versionless /guides keys are checked once, not per-version", () => {
    // /guides content has no version axis. Even with multi-version mode on,
    // the rule should be checked exactly once against guides/ — and pass.
    const root = makeMultiVersionProject();
    const rules: RedirectRule[] = [{ key: "/guides/old", value: { targetUrl: "/guides/kubernetes" } }];
    assert.deepEqual(validateTargetsExist(rules, root, "7.2", ["7.2", "7.1", "7.0"]), []);
});

test("validateTargetsExist multi-version: follows redirect chains per version", () => {
    // /ai/old → /ai/vector-search/start → /ai/restructured (only on 7.2).
    // For 7.2, chain resolves to /ai/restructured (must exist on 7.2).
    // For 7.1, second hop's gate fails — chain stops at /ai/vector-search/start
    // (must exist on 7.1).
    const root = makeMultiVersionProject();
    fs.writeFileSync(path.join(root, "docs", "ai", "restructured.mdx"), "");
    const rules: RedirectRule[] = [
        { key: "/ai/old", value: { targetUrl: "/ai/vector-search/start", minimumVersion: "7.0" } },
        { key: "/ai/vector-search/start", value: { targetUrl: "/ai/restructured", minimumVersion: "7.2" } },
    ];
    // 7.0 isn't in versions list — gate skips it. 7.1 chain stops at start.mdx
    // (which exists on 7.1 in the fixture). 7.2 walks to /ai/restructured (which
    // we just wrote in current docs). All clean.
    assert.deepEqual(validateTargetsExist(rules, root, "7.2", ["7.2", "7.1"]), []);
});

test("validateTargetsExist multi-version: chain miss reports terminus in the message", () => {
    // Chain target on 7.2 doesn't exist — error should mention both the
    // original target and the chain terminus so authors can locate the bug.
    const root = makeMultiVersionProject();
    const rules: RedirectRule[] = [
        { key: "/ai/old", value: { targetUrl: "/ai/vector-search/start", minimumVersion: "7.2" } },
        { key: "/ai/vector-search/start", value: { targetUrl: "/ai/nowhere", minimumVersion: "7.2" } },
    ];
    const errors = validateTargetsExist(rules, root, "7.2", ["7.2"]);
    assert.equal(errors.length, 2);
    const chainErr = errors.find((e) => e.key === "/ai/old")!;
    assert.match(chainErr.message, /chain → \/ai\/nowhere/);
});

// --- version-policy ↔ versions.json drift -------------------------------
//
// version-policy.js hand-curates the BUILT_VERSIONS list; Docusaurus
// auto-manages versions.json. When a snapshot is added or removed, both
// have to move together. This test fails loudly the first time they drift.

test("BUILT_VERSIONS matches versions.json contents (drift gate)", () => {
    const projectRoot = path.join(__dirname, "..", "..", "..", "..");
    const versionsJsonPath = path.join(projectRoot, "versions.json");
    const versionsJson = JSON.parse(fs.readFileSync(versionsJsonPath, "utf8")) as string[];
    // BUILT_VERSIONS = [CURRENT_VERSION, ...non-current versions]; versions.json
    // is just the non-current list. Compare as sets to avoid order coupling.
    const policyNonCurrent = BUILT_VERSIONS.filter((v) => v !== CURRENT_VERSION);
    assert.deepEqual([...policyNonCurrent].sort(), [...versionsJson].sort());
});
