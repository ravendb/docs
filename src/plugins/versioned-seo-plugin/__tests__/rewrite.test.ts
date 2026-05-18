import { test } from "node:test";
import assert from "node:assert/strict";

import { buildRedirectMap, type RedirectRule } from "../lib/redirects.js";
import { rewriteHtml } from "../lib/rewrite.js";
import { buildScopedRoutes, hasNoindexRobotsMeta, verifyCanonicals, type CanonicalRecord } from "../lib/verify.js";

const CURRENT = "7.2";
const LEGACY = ["5.4", "5.3", "4.2"];
const BASE_URL = "https://docs.ravendb.net";

const RULES: RedirectRule[] = [
    { key: "/old/a", value: { targetUrl: "/new/a", minimumVersion: "7.2" } },
    { key: "/old/b", value: { targetUrl: "/mid/b", minimumVersion: "7.2" } },
    { key: "/mid/b", value: { targetUrl: "/new/b", minimumVersion: "7.2" } },
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
    const gatedMap = buildRedirectMap([{ key: "/future", value: { targetUrl: "/future-new", minimumVersion: "9.9" } }]);
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

test("buildScopedRoutes partitions routes by version and section, skipping legacy/templates/root", () => {
    const map = buildScopedRoutes(
        ["/7.2/a", "/7.2/b/", "/6.2/c", "/4.2/legacy", "/cloud/x", "/guides/y", "/templates/z", "/"],
        LEGACY
    );
    assert.equal(map.get("7.2")?.size, 2);
    assert.ok(map.get("7.2")?.has("/7.2/a"));
    assert.ok(map.get("7.2")?.has("/7.2/b"));
    assert.equal(map.get("6.2")?.size, 1);
    assert.ok(map.get("6.2")?.has("/6.2/c"));
    assert.equal(map.get("4.2"), undefined, "legacy versions should not appear in the map");
    assert.equal(map.get("cloud")?.size, 1);
    assert.equal(map.get("guides")?.size, 1);
    assert.equal(map.get("templates"), undefined, "templates are never valid see_also targets");
    assert.equal(map.get("root"), undefined, "root is never a valid see_also target");
});

test("verifyCanonicals reports canonicals pointing outside the universe", () => {
    const universe = buildScopedRoutes(["/7.2/new/a", "/7.2/stable/path"], LEGACY).get(CURRENT)!;
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
    // Issue should carry an actionable fix block naming the versionless path
    // and the validate command so the author can paste + edit in one go.
    assert.ok(issues[0].fix, "expected fix block on universe miss");
    assert.match(issues[0].fix!, /scripts\/redirects\.json/);
    assert.match(issues[0].fix!, /"key": "\/does-not-exist"/);
    assert.match(issues[0].fix!, /"minimumVersion": "7\.2"/);
    assert.match(issues[0].fix!, /npm run validate-redirects/);
});

test("verifyCanonicals skips legacy-version files (self-canonical is trivially valid)", () => {
    const universe = buildScopedRoutes(["/7.2/foo"], LEGACY).get(CURRENT)!;
    const records: CanonicalRecord[] = [{ file: "legacy.html", canonical: `${BASE_URL}/4.2/foo`, fileVersion: "4.2" }];
    const issues = verifyCanonicals({
        records,
        universe,
        currentVersion: CURRENT,
        legacyVersions: LEGACY,
        baseUrl: BASE_URL,
    });
    assert.deepEqual(issues, []);
});

// --- hasNoindexRobotsMeta predicate ---

test("hasNoindexRobotsMeta detects Docusaurus default form", () => {
    // What @docusaurus/theme-classic's DocVersionRoot emits today.
    const html = `<!doctype html><html><head><meta name="robots" content="noindex, nofollow" /></head></html>`;
    assert.equal(hasNoindexRobotsMeta(html), true);
});

test("hasNoindexRobotsMeta detects the legacy hand-rolled form (noindex,follow no space)", () => {
    // The form our removed templates-noindex-plugin used to inject — should
    // still be recognised so a brownfield build during the transition passes.
    const html = `<!doctype html><html><head><meta name="robots" content="noindex,follow"></head></html>`;
    assert.equal(hasNoindexRobotsMeta(html), true);
});

test("hasNoindexRobotsMeta detects unquoted attributes (post-minified HTML)", () => {
    // The swc minifier under @docusaurus/faster strips quotes where legal.
    const html = `<!doctype html><html><head><meta name=robots content="noindex, nofollow"></head></html>`;
    assert.equal(hasNoindexRobotsMeta(html), true);
});

test("hasNoindexRobotsMeta detects single-quoted attributes", () => {
    const html = `<!doctype html><html><head><meta name='robots' content='noindex, nofollow' /></head></html>`;
    assert.equal(hasNoindexRobotsMeta(html), true);
});

test("hasNoindexRobotsMeta is case-insensitive on the directive value", () => {
    // Some tooling emits NOINDEX. The regex carries the /i flag so this passes.
    const html = `<!doctype html><html><head><meta name="robots" content="NOINDEX, NOFOLLOW"></head></html>`;
    assert.equal(hasNoindexRobotsMeta(html), true);
});

test("hasNoindexRobotsMeta returns false when no robots meta exists", () => {
    const html = `<!doctype html><html><head><title>x</title><meta name="description" content="y"></head></html>`;
    assert.equal(hasNoindexRobotsMeta(html), false);
});

test("hasNoindexRobotsMeta returns false when robots meta lacks 'noindex'", () => {
    // A page with index,follow should fail the audit — that's the bug the
    // assertion is here to catch (legacy version not actually noindex'd).
    const html = `<!doctype html><html><head><meta name="robots" content="index, follow"></head></html>`;
    assert.equal(hasNoindexRobotsMeta(html), false);
});

test("hasNoindexRobotsMeta requires 'noindex' as a whole word, not a substring", () => {
    // Defensive: a hypothetical attribute value of "noindexing" (no such
    // directive exists, but make sure the regex doesn't accept partial
    // matches that could mask a real bug).
    const html = `<!doctype html><html><head><meta name="robots" content="noindexing"></head></html>`;
    assert.equal(hasNoindexRobotsMeta(html), false);
});
