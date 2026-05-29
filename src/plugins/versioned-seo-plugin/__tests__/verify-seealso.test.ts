import { test } from "node:test";
import assert from "node:assert/strict";

import { fanoutSeeAlsoRecords, verifySeeAlsoLinks, type SeeAlsoRecord } from "../lib/verify-seealso.js";

const CURRENT = "7.2";

function scoped(entries: Record<string, string[]>): Map<string, Set<string>> {
    const map = new Map<string, Set<string>>();
    for (const [k, v] of Object.entries(entries)) {
        map.set(k, new Set(v));
    }
    return map;
}

function record(partial: Partial<SeeAlsoRecord> & Pick<SeeAlsoRecord, "href" | "source">): SeeAlsoRecord {
    return {
        articlePath: partial.articlePath ?? "/7.1/some-article",
        href: partial.href,
        source: partial.source,
        articleVersion: "articleVersion" in partial ? partial.articleVersion! : "7.1",
    };
}

// --- verifySeeAlsoLinks ---

test("verifySeeAlsoLinks accepts a docs href that resolves in the article's version", () => {
    const issues = verifySeeAlsoLinks({
        records: [record({ source: "docs", href: "/7.1/client-api/foo" })],
        routesByScope: scoped({ "7.1": ["/7.1/client-api/foo"] }),
        latestVersion: CURRENT,
    });
    assert.deepEqual(issues, []);
});

test("verifySeeAlsoLinks flags a docs href that doesn't resolve in the article's version", () => {
    // Frontmatter wrote `link: "/7.2/foo"` on a /7.1/ article → href /7.2/foo looked up in
    // the "7.1" slice → miss → existence error.
    const issues = verifySeeAlsoLinks({
        records: [record({ source: "docs", href: "/7.2/client-api/foo" })],
        routesByScope: scoped({ "7.1": [], "7.2": ["/7.2/client-api/foo"] }),
        latestVersion: CURRENT,
    });
    assert.equal(issues.length, 1);
    assert.match(issues[0].reason, /scope "7\.1"/);
});

test("verifySeeAlsoLinks resolves docs hrefs on cloud articles against the latest version", () => {
    const issues = verifySeeAlsoLinks({
        records: [
            record({
                source: "docs",
                href: "/7.2/client-api/foo",
                articleVersion: null,
                articlePath: "/cloud/billing",
            }),
        ],
        routesByScope: scoped({ "7.2": ["/7.2/client-api/foo"] }),
        latestVersion: CURRENT,
    });
    assert.deepEqual(issues, []);
});

test("verifySeeAlsoLinks routes source: cloud hrefs to the cloud slice", () => {
    const issues = verifySeeAlsoLinks({
        records: [record({ source: "cloud", href: "/cloud/some-page" })],
        routesByScope: scoped({ "7.1": [], cloud: ["/cloud/some-page"] }),
        latestVersion: CURRENT,
    });
    assert.deepEqual(issues, []);
});

test("verifySeeAlsoLinks routes source: guides hrefs to the guides slice", () => {
    const issues = verifySeeAlsoLinks({
        records: [record({ source: "guides", href: "/guides/some-guide" })],
        routesByScope: scoped({ "7.1": [], guides: ["/guides/some-guide"] }),
        latestVersion: CURRENT,
    });
    assert.deepEqual(issues, []);
});

test("verifySeeAlsoLinks routes source: samples hrefs to the samples slice", () => {
    const issues = verifySeeAlsoLinks({
        records: [record({ source: "samples", href: "/samples/fit-assistant" })],
        routesByScope: scoped({ "7.1": [], samples: ["/samples/fit-assistant"] }),
        latestVersion: CURRENT,
    });
    assert.deepEqual(issues, []);
});

test("verifySeeAlsoLinks reports a guides href whose target doesn't exist", () => {
    const issues = verifySeeAlsoLinks({
        records: [record({ source: "guides", href: "/guides/nonexistent" })],
        routesByScope: scoped({ "7.1": [], guides: ["/guides/some-guide"] }),
        latestVersion: CURRENT,
    });
    assert.equal(issues.length, 1);
    assert.match(issues[0].reason, /scope "guides"/);
});

test("verifySeeAlsoLinks skips source: external", () => {
    const issues = verifySeeAlsoLinks({
        records: [record({ source: "external", href: "https://demo.ravendb.net/anything" })],
        routesByScope: scoped({ "7.1": [] }),
        latestVersion: CURRENT,
    });
    assert.deepEqual(issues, []);
});

// --- fanoutSeeAlsoRecords ---

test("fanoutSeeAlsoRecords emits one record per data-seealso anchor with its source and href", () => {
    const html = `<a data-seealso="docs" href="/7.1/foo">x</a><a data-seealso="external" href="https://x.example">y</a>`;
    const records = fanoutSeeAlsoRecords(html, "/7.1/page", "7.1");
    assert.deepEqual(records, [
        { articlePath: "/7.1/page", href: "/7.1/foo", source: "docs", articleVersion: "7.1" },
        { articlePath: "/7.1/page", href: "https://x.example", source: "external", articleVersion: "7.1" },
    ]);
});

test("fanoutSeeAlsoRecords ignores anchors without data-seealso", () => {
    const html = `<a href="/7.1/missing">x</a><a data-seealso="docs" href="/7.1/present">y</a>`;
    const records = fanoutSeeAlsoRecords(html, "/7.1/page", "7.1");
    assert.equal(records.length, 1);
    assert.equal(records[0].href, "/7.1/present");
});

test("fanoutSeeAlsoRecords strips #fragment from the href", () => {
    const html = `<a data-seealso="docs" href="/7.1/foo#anchor">x</a>`;
    const records = fanoutSeeAlsoRecords(html, "/7.1/page", "7.1");
    assert.equal(records[0].href, "/7.1/foo");
});
