import { test } from "node:test";
import assert from "node:assert/strict";
import fs from "node:fs";
import os from "node:os";
import path from "node:path";

import { splitSitemap } from "../lib/split.js";

const BASE_URL = "https://docs.ravendb.net";
const LEGACY = ["4.2", "5.4"];

function urlBlock(loc: string): string {
    return `<url><loc>${loc}</loc><changefreq>weekly</changefreq></url>`;
}

function buildSitemap(urls: string[]): string {
    return (
        `<?xml version="1.0" encoding="UTF-8"?>` +
        `<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">` +
        urls.map(urlBlock).join("") +
        `</urlset>`
    );
}

function withTempBuildDir(body: (dir: string) => void): void {
    const dir = fs.mkdtempSync(path.join(os.tmpdir(), "split-sitemap-"));
    try {
        body(dir);
    } finally {
        fs.rmSync(dir, { recursive: true, force: true });
    }
}

test("splitSitemap skips when sitemap.xml is absent", () => {
    withTempBuildDir((dir) => {
        const result = splitSitemap({ buildDir: dir, legacyVersions: LEGACY, baseUrl: BASE_URL });
        assert.equal(result.skipped, true);
    });
});

test("splitSitemap groups URLs by section and version", () => {
    withTempBuildDir((dir) => {
        const urls = [
            `${BASE_URL}/7.2/foo`,
            `${BASE_URL}/7.2/bar`,
            `${BASE_URL}/6.2/baz`,
            `${BASE_URL}/cloud/account`,
            `${BASE_URL}/guides/intro`,
            `${BASE_URL}/search`,
        ];
        fs.writeFileSync(path.join(dir, "sitemap.xml"), buildSitemap(urls));
        const result = splitSitemap({ buildDir: dir, legacyVersions: LEGACY, baseUrl: BASE_URL });
        assert.equal(result.skipped, false);
        if (result.skipped) {
            return;
        }
        const names = result.files.map((f) => f.name).sort();
        assert.deepEqual(names, [
            "sitemap-cloud.xml",
            "sitemap-docs-6.2.xml",
            "sitemap-docs-7.2.xml",
            "sitemap-guides.xml",
            "sitemap-misc.xml",
        ]);
        const docs72 = result.files.find((f) => f.name === "sitemap-docs-7.2.xml");
        assert.equal(docs72?.urls, 2);
    });
});

test("splitSitemap excludes legacy-version URLs", () => {
    withTempBuildDir((dir) => {
        const urls = [`${BASE_URL}/7.2/ok`, `${BASE_URL}/4.2/legacy`, `${BASE_URL}/5.4/also-legacy`];
        fs.writeFileSync(path.join(dir, "sitemap.xml"), buildSitemap(urls));
        const result = splitSitemap({ buildDir: dir, legacyVersions: LEGACY, baseUrl: BASE_URL });
        assert.equal(result.skipped, false);
        if (result.skipped) {
            return;
        }
        assert.equal(result.includedUrls, 1);
        assert.equal(result.skippedLegacyUrls, 2);
        assert.ok(!result.files.some((f) => f.name.includes("4.2") || f.name.includes("5.4")));
    });
});

test("splitSitemap replaces sitemap.xml with a sitemapindex referencing each sub-file", () => {
    withTempBuildDir((dir) => {
        const urls = [`${BASE_URL}/7.2/foo`, `${BASE_URL}/cloud/x`];
        fs.writeFileSync(path.join(dir, "sitemap.xml"), buildSitemap(urls));
        splitSitemap({ buildDir: dir, legacyVersions: LEGACY, baseUrl: BASE_URL });
        const indexXml = fs.readFileSync(path.join(dir, "sitemap.xml"), "utf8");
        assert.match(indexXml, /<sitemapindex/);
        assert.match(indexXml, /sitemap-docs-7\.2\.xml/);
        assert.match(indexXml, /sitemap-cloud\.xml/);
    });
});
