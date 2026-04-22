import { test } from "node:test";
import assert from "node:assert/strict";

import { buildRedirectMap, type RedirectRule } from "../lib/redirects.js";
import { rewriteHtml } from "../lib/rewrite.js";
import { buildUniverse, verifyCanonicals, type CanonicalRecord } from "../lib/verify.js";

const CURRENT = "7.2";
const LEGACY = ["5.4", "5.3", "4.2"];
const BASE_URL = "https://docs.ravendb.net";

const RULES: RedirectRule[] = [
    { key: "/old/a", value: { targetUrl: "/new/a", minimumVersion: "7.2" } },
    { key: "/old/b", value: { targetUrl: "/mid/b" } },
    { key: "/mid/b", value: { targetUrl: "/new/b" } },
];
const MAP = buildRedirectMap(RULES);

function htmlWithCanonical(href: string): string {
    return `<!doctype html><html><head><link rel="canonical" href="${href}"><title>x</title></head><body>x</body></html>`;
}

test("rewriteHtml redirects indexed-version canonical to current-version resolved path", () => {
    const html = htmlWithCanonical(`${BASE_URL}/${CURRENT}/old/a`);
    const result = rewriteHtml({
        html,
        fileVersion: "6.2",
        versionlessPath: "/old/a",
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        redirects: MAP,
        baseUrl: BASE_URL,
    });
    assert.equal(result.changed, true);
    assert.equal(result.newCanonical, `${BASE_URL}/${CURRENT}/new/a`);
    assert.equal(result.chainResolved, true);
    assert.match(result.html, /href="https:\/\/docs\.ravendb\.net\/7\.2\/new\/a"/);
});

test("rewriteHtml follows multi-hop chains", () => {
    const html = htmlWithCanonical(`${BASE_URL}/${CURRENT}/old/b`);
    const result = rewriteHtml({
        html,
        fileVersion: CURRENT,
        versionlessPath: "/old/b",
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        redirects: MAP,
        baseUrl: BASE_URL,
    });
    assert.equal(result.newCanonical, `${BASE_URL}/${CURRENT}/new/b`);
    assert.equal(result.chainResolved, true);
});

test("rewriteHtml leaves canonical untouched when no redirect rule matches", () => {
    const html = htmlWithCanonical(`${BASE_URL}/${CURRENT}/stable/path`);
    const result = rewriteHtml({
        html,
        fileVersion: "7.0",
        versionlessPath: "/stable/path",
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        redirects: MAP,
        baseUrl: BASE_URL,
    });
    assert.equal(result.changed, false);
    assert.equal(result.chainResolved, false);
    assert.equal(result.newCanonical, `${BASE_URL}/${CURRENT}/stable/path`);
});

test("rewriteHtml rewrites legacy-version canonical to self", () => {
    // Docusaurus emits /7.2/... as canonical for /4.2/... — we overwrite to self.
    const html = htmlWithCanonical(`${BASE_URL}/${CURRENT}/client-api/foo`);
    const result = rewriteHtml({
        html,
        fileVersion: "4.2",
        versionlessPath: "/client-api/foo",
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        redirects: MAP,
        baseUrl: BASE_URL,
    });
    assert.equal(result.changed, true);
    assert.equal(result.newCanonical, `${BASE_URL}/4.2/client-api/foo`);
});

test("rewriteHtml is idempotent when run twice", () => {
    const html = htmlWithCanonical(`${BASE_URL}/${CURRENT}/old/a`);
    const first = rewriteHtml({
        html,
        fileVersion: "6.2",
        versionlessPath: "/old/a",
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        redirects: MAP,
        baseUrl: BASE_URL,
    });
    const second = rewriteHtml({
        html: first.html,
        fileVersion: "6.2",
        versionlessPath: "/old/a",
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        redirects: MAP,
        baseUrl: BASE_URL,
    });
    assert.equal(second.changed, false);
    assert.equal(second.html, first.html);
});

test("rewriteHtml handles single-quoted href attribute", () => {
    const html = `<html><head><link rel='canonical' href='${BASE_URL}/${CURRENT}/old/a'></head></html>`;
    const result = rewriteHtml({
        html,
        fileVersion: CURRENT,
        versionlessPath: "/old/a",
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        redirects: MAP,
        baseUrl: BASE_URL,
    });
    assert.equal(result.changed, true);
    assert.match(result.html, /href="https:\/\/docs\.ravendb\.net\/7\.2\/new\/a"/);
});

test("rewriteHtml handles unquoted attributes (post-minification HTML)", () => {
    // @docusaurus/faster uses swc_html_minifier, which strips attribute quotes
    // where legal. Our plugin runs after minification, so it must match and
    // rewrite unquoted rel=canonical href=... forms too.
    const html =
        `<!doctype html><html><head>` +
        `<link data-rh=true rel=canonical href=${BASE_URL}/${CURRENT}/old/a />` +
        `</head></html>`;
    const result = rewriteHtml({
        html,
        fileVersion: "6.2",
        versionlessPath: "/old/a",
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        redirects: MAP,
        baseUrl: BASE_URL,
    });
    assert.equal(result.changed, true);
    assert.equal(result.newCanonical, `${BASE_URL}/${CURRENT}/new/a`);
    assert.match(result.html, /href="https:\/\/docs\.ravendb\.net\/7\.2\/new\/a"/);
});

test("rewriteHtml returns unchanged when no canonical tag exists", () => {
    const html = `<html><head></head><body>hi</body></html>`;
    const result = rewriteHtml({
        html,
        fileVersion: CURRENT,
        versionlessPath: "/old/a",
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        redirects: MAP,
        baseUrl: BASE_URL,
    });
    assert.equal(result.changed, false);
    assert.equal(result.newCanonical, null);
});

test("rewriteHtml respects minimumVersion gating when the file's version gates the hop", () => {
    // Rewriter resolves against currentVersion (7.2), not fileVersion, so the
    // gate at /old/a (minVersion 7.2) applies. For completeness, verify a rule
    // that's gated higher than current doesn't hop.
    const gatedMap = buildRedirectMap([
        { key: "/future", value: { targetUrl: "/future-new", minimumVersion: "9.9" } },
    ]);
    const html = htmlWithCanonical(`${BASE_URL}/${CURRENT}/future`);
    const result = rewriteHtml({
        html,
        fileVersion: "6.2",
        versionlessPath: "/future",
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        redirects: gatedMap,
        baseUrl: BASE_URL,
    });
    assert.equal(result.changed, false);
    assert.equal(result.newCanonical, `${BASE_URL}/${CURRENT}/future`);
});

// --- Verifier ---

test("buildUniverse keeps only current-version routes", () => {
    const universe = buildUniverse(["/7.2/a", "/7.2/b", "/6.2/c", "/cloud/x", "/"], CURRENT);
    assert.equal(universe.size, 2);
    assert.ok(universe.has("/7.2/a"));
    assert.ok(universe.has("/7.2/b"));
});

test("verifyCanonicals reports canonicals pointing outside the universe", () => {
    const universe = buildUniverse(["/7.2/new/a", "/7.2/stable/path"], CURRENT);
    const records: CanonicalRecord[] = [
        { file: "a.html", canonical: `${BASE_URL}/${CURRENT}/new/a`, fileVersion: "7.2" },
        { file: "b.html", canonical: `${BASE_URL}/${CURRENT}/does-not-exist`, fileVersion: "7.2" },
    ];
    const issues = verifyCanonicals({
        records,
        universe,
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        baseUrl: BASE_URL,
    });
    assert.equal(issues.length, 1);
    assert.equal(issues[0].file, "b.html");
    assert.match(issues[0].reason, /universe/);
});

test("verifyCanonicals skips legacy-version files (self-canonical is trivially valid)", () => {
    const universe = buildUniverse(["/7.2/foo"], CURRENT);
    const records: CanonicalRecord[] = [
        { file: "legacy.html", canonical: `${BASE_URL}/4.2/foo`, fileVersion: "4.2" },
    ];
    const issues = verifyCanonicals({
        records,
        universe,
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        baseUrl: BASE_URL,
    });
    assert.deepEqual(issues, []);
});
