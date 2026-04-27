/**
 * Behavioural tests for scripts/handle_redirects.js (the CloudFront Function).
 *
 * The edge runtime is single-file — its only resolvable import is the
 * runtime-provided `cloudfront` module. To exercise `handler()` locally we
 * read the source as text, strip the `import cf from "cloudfront"` line, and
 * rewrite the hardcoded `const CURRENT_VERSION = "..."` declaration to pull
 * the value from the test wrapper — letting tests simulate different default
 * versions without touching the source of truth. A `cf` stub (KVS handle
 * backed by a Map) is injected through a `new Function` wrapper; the stub
 * surface matches what CloudFront Functions provides: `cf.kvs()` returns an
 * object with an async `get(key)` that resolves to a string value or rejects
 * on miss.
 *
 * Test fixtures use realistic URIs (static assets, /templates, /guides,
 * /cloud, versioned, versionless) to cover the handler's branches. Chain
 * and minimumVersion cases target the chain-resolution logic.
 */

import { test } from "node:test";
import assert from "node:assert/strict";
import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const EDGE_FILE = path.join(__dirname, "..", "..", "..", "..", "scripts", "handle_redirects.js");

/**
 * CloudFront Function event shape (minimal — the handler only reads
 * `event.request.uri` and mutates `request.uri`).
 */
interface CfEvent {
    request: { uri: string };
}

interface CfRedirectResponse {
    statusCode: number;
    statusDescription: string;
    headers: { location: { value: string } };
}

type CfReturn = CfEvent["request"] | CfRedirectResponse;

type Handler = (event: CfEvent) => Promise<CfReturn>;

interface KvsRule {
    targetUrl: string;
    minimumVersion?: string;
}

/**
 * Build a callable `handler` from the edge source. `kvs` is a map of
 * versionless path → rule; absent keys trigger a KVS miss (rejected promise),
 * matching how the real KVS behaves.
 */
function buildHandler(kvs: Map<string, KvsRule>, currentVersion = "7.2"): Handler {
    const source = fs.readFileSync(EDGE_FILE, "utf8");

    // Strip the `cloudfront` import (runtime-provided, stubbed below) and
    // rewrite the hardcoded `const CURRENT_VERSION = "..."` line to pull from
    // the wrapper so tests can simulate different defaults.
    const transformed = source
        .replace(/^import cf from "cloudfront";\s*$/m, "")
        .replace(/^const CURRENT_VERSION = "[\d.]+";\s*$/m, "const CURRENT_VERSION = __currentVersion;");

    // The edge file doesn't export `handler` — it's defined top-level. The
    // wrapper declares the `cf` stub, inlines the transformed source, and
    // returns the handler reference.
    const wrapper = `
        const cf = {
            kvs: () => ({
                get: async (key) => {
                    if (!__kvs.has(key)) {
                        throw new Error("not found: " + key);
                    }
                    return JSON.stringify(__kvs.get(key));
                },
            }),
        };
        ${transformed}
        return handler;
    `;

    const factory = new Function("__kvs", "__currentVersion", wrapper) as (
        kvs: Map<string, KvsRule>,
        currentVersion: string
    ) => Handler;

    return factory(kvs, currentVersion);
}

function makeEvent(uri: string): CfEvent {
    return { request: { uri } };
}

function isRedirect(result: CfReturn): result is CfRedirectResponse {
    return (result as CfRedirectResponse).statusCode === 301;
}

// ---------------------------------------------------------------------------
// Static asset pass-through
// ---------------------------------------------------------------------------

test("handler lets static assets through unchanged", async () => {
    const handler = buildHandler(new Map());
    const result = await handler(makeEvent("/images/logo.png"));
    assert.ok(!isRedirect(result));
    assert.equal(result.uri, "/images/logo.png");
});

test("handler treats .html as a static asset (pass-through)", async () => {
    const handler = buildHandler(new Map());
    const result = await handler(makeEvent("/7.2/foo/index.html"));
    assert.ok(!isRedirect(result));
    assert.equal(result.uri, "/7.2/foo/index.html");
});

// ---------------------------------------------------------------------------
// /templates branch
// ---------------------------------------------------------------------------

test("handler rewrites /templates/foo → /templates/foo/index.html", async () => {
    const handler = buildHandler(new Map());
    const result = await handler(makeEvent("/templates/getting-started"));
    assert.ok(!isRedirect(result));
    assert.equal(result.uri, "/templates/getting-started/index.html");
});

test("handler 301s /templates/foo/ → /templates/foo (trailing slash)", async () => {
    const handler = buildHandler(new Map());
    const result = await handler(makeEvent("/templates/getting-started/"));
    assert.ok(isRedirect(result));
    assert.equal(result.headers.location.value, "/templates/getting-started");
});

// ---------------------------------------------------------------------------
// /guides and /cloud branches
// ---------------------------------------------------------------------------

test("handler 301s /guides/* to its KVS redirect target when one exists", async () => {
    // /guides rules are versionless — no minimumVersion on the KVS payload.
    const kvs = new Map<string, KvsRule>([["/guides/old-guide", { targetUrl: "/guides/new-guide" }]]);
    const handler = buildHandler(kvs);
    const result = await handler(makeEvent("/guides/old-guide"));
    assert.ok(isRedirect(result));
    assert.equal(result.headers.location.value, "/guides/new-guide");
});

test("handler serves /guides/* index.html on KVS miss with no trailing slash", async () => {
    const handler = buildHandler(new Map());
    const result = await handler(makeEvent("/guides/some-guide"));
    assert.ok(!isRedirect(result));
    assert.equal(result.uri, "/guides/some-guide/index.html");
});

test("handler 301s /guides/*/ to strip the trailing slash", async () => {
    const handler = buildHandler(new Map());
    const result = await handler(makeEvent("/guides/some-guide/"));
    assert.ok(isRedirect(result));
    assert.equal(result.headers.location.value, "/guides/some-guide");
});

test("handler applies the same KVS-first logic under /cloud", async () => {
    // /cloud rules are versionless — no minimumVersion on the KVS payload.
    const kvs = new Map<string, KvsRule>([["/cloud/legacy", { targetUrl: "/cloud/current" }]]);
    const handler = buildHandler(kvs);
    const hit = await handler(makeEvent("/cloud/legacy"));
    assert.ok(isRedirect(hit));
    assert.equal(hit.headers.location.value, "/cloud/current");

    const miss = await handler(makeEvent("/cloud/foo"));
    assert.ok(!isRedirect(miss));
    assert.equal(miss.uri, "/cloud/foo/index.html");
});

// ---------------------------------------------------------------------------
// Versioned URIs
// ---------------------------------------------------------------------------

test("handler serves /{version}/foo index.html when no redirect matches", async () => {
    const handler = buildHandler(new Map());
    const result = await handler(makeEvent("/7.2/foo"));
    assert.ok(!isRedirect(result));
    assert.equal(result.uri, "/7.2/foo/index.html");
});

test("handler 301s /{version}/foo/ to strip the trailing slash", async () => {
    const handler = buildHandler(new Map());
    const result = await handler(makeEvent("/7.2/foo/"));
    assert.ok(isRedirect(result));
    assert.equal(result.headers.location.value, "/7.2/foo");
});

test("handler 301s a versioned URI to its redirect target with the version prefix preserved", async () => {
    const kvs = new Map<string, KvsRule>([["/old/path", { targetUrl: "/new/path", minimumVersion: "7.0" }]]);
    const handler = buildHandler(kvs);
    const result = await handler(makeEvent("/7.2/old/path"));
    assert.ok(isRedirect(result));
    assert.equal(result.headers.location.value, "/7.2/new/path");
});

// ---------------------------------------------------------------------------
// Versionless URIs default to CURRENT_VERSION
// ---------------------------------------------------------------------------

test("handler 301s a versionless URI to the current version's equivalent", async () => {
    const handler = buildHandler(new Map(), "7.2");
    const result = await handler(makeEvent("/foo"));
    assert.ok(isRedirect(result));
    assert.equal(result.headers.location.value, "/7.2/foo");
});

test("handler 301s the versionless root / to /{current}", async () => {
    const handler = buildHandler(new Map(), "7.2");
    const result = await handler(makeEvent("/"));
    assert.ok(isRedirect(result));
    assert.equal(result.headers.location.value, "/7.2");
});

// ---------------------------------------------------------------------------
// minimumVersion gating
// ---------------------------------------------------------------------------

test("handler honours minimumVersion: rule not applied when file version is below threshold", async () => {
    const kvs = new Map<string, KvsRule>([["/moved", { targetUrl: "/new-home", minimumVersion: "7.2" }]]);
    const handler = buildHandler(kvs);
    // 6.2 file predates the 7.2 rename — rule should be ignored and the
    // versioned path served as-is (with index.html appended).
    const result = await handler(makeEvent("/6.2/moved"));
    assert.ok(!isRedirect(result));
    assert.equal(result.uri, "/6.2/moved/index.html");
});

test("handler applies minimumVersion: rule kicks in at exactly the threshold version", async () => {
    const kvs = new Map<string, KvsRule>([["/moved", { targetUrl: "/new-home", minimumVersion: "7.2" }]]);
    const handler = buildHandler(kvs);
    const result = await handler(makeEvent("/7.2/moved"));
    assert.ok(isRedirect(result));
    assert.equal(result.headers.location.value, "/7.2/new-home");
});

// ---------------------------------------------------------------------------
// Chain resolution
// ---------------------------------------------------------------------------

test("handler collapses a 3-hop chain into a single 301 to the terminal target", async () => {
    const kvs = new Map<string, KvsRule>([
        ["/a", { targetUrl: "/b", minimumVersion: "7.0" }],
        ["/b", { targetUrl: "/c", minimumVersion: "7.0" }],
        ["/c", { targetUrl: "/d", minimumVersion: "7.0" }],
    ]);
    const handler = buildHandler(kvs);
    const result = await handler(makeEvent("/7.2/a"));
    assert.ok(isRedirect(result));
    assert.equal(result.headers.location.value, "/7.2/d");
});

test("handler stops chaining when a hop's minimumVersion fails the resolving version", async () => {
    const kvs = new Map<string, KvsRule>([
        // 6.2 reader can follow /old → /mid, but the /mid → /new hop
        // is gated at 7.2 so it stops at /mid.
        ["/old", { targetUrl: "/mid", minimumVersion: "7.1" }],
        ["/mid", { targetUrl: "/new", minimumVersion: "7.2" }],
    ]);
    const handler = buildHandler(kvs);
    const reader72 = await handler(makeEvent("/7.2/old"));
    assert.ok(isRedirect(reader72));
    assert.equal(reader72.headers.location.value, "/7.2/new");

    const reader62 = await handler(makeEvent("/6.2/old"));
    assert.ok(!isRedirect(reader62));
    // /6.2 can't follow /old either (minimumVersion 7.1 fails), so it
    // serves the versioned path as-is.
    assert.equal(reader62.uri, "/6.2/old/index.html");
});

test("handler treats a rule with no minimumVersion as unconditional", async () => {
    // Versionless content areas (/guides, /cloud) short-circuit above the
    // chain loop, so in practice the chain loop only sees docs-area keys
    // whose minimumVersion is validated present. The `rule.minimumVersion`
    // guard in the chain loop is a cheap belt-and-braces for the
    // versionless shape — if one ever reaches the loop, "absent" means
    // "always apply". This test pins that semantics.
    const kvs = new Map<string, KvsRule>([["/always", { targetUrl: "/everywhere" }]]);
    const handler = buildHandler(kvs);
    const result = await handler(makeEvent("/6.2/always"));
    assert.ok(isRedirect(result));
    assert.equal(result.headers.location.value, "/6.2/everywhere");
});

// Cycle and self-loop protection lives in validateNoCycles (unit-tested in
// redirects.test.ts), not at the edge. The KVS only ever contains pre-
// validated data, so the handler trusts that invariant and keeps its
// chain loop minimal — no `visited` set, no hop cap. Shipping a cyclic
// rule fails `npm run validate-redirects` (and the plugin's loadContent)
// before it can reach the edge.
