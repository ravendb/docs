/**
 * Sitemap splitter.
 *
 * Takes a single Docusaurus-generated sitemap.xml and splits it into
 * section-based sub-sitemaps referenced from a <sitemapindex>. Keeps the
 * per-version sitemap size small enough to satisfy search-engine limits
 * and lets us re-ping only changed sections on deploy.
 *
 *   /cloud/*  → sitemap-cloud.xml
 *   /guides/* → sitemap-guides.xml
 *   /X.Y/*    → sitemap-docs-X.Y.xml  (if X.Y isn't legacy)
 *   other     → sitemap-misc.xml      (search, root pages, etc.)
 *
 * Legacy versions are excluded entirely (they're also disallowed in robots.txt).
 *
 * Invoked from scripts/split-sitemap.ts as a post-`docusaurus build` step —
 * NOT a Docusaurus plugin, because @docusaurus/plugin-sitemap writes
 * sitemap.xml after user plugin postBuild hooks, so a plugin here would run
 * before the file exists.
 */

import fs from "node:fs";
import path from "node:path";

export interface SplitOptions {
    /** Absolute path to the output build directory (where sitemap.xml lives). */
    buildDir: string;
    /** Legacy versions — excluded from the sitemap index. */
    legacyVersions: readonly string[];
    /** Site origin for sitemapindex <loc> entries. No trailing slash. */
    baseUrl: string;
}

export interface SplitResult {
    skipped: true;
    reason: string;
}

export interface SplitSucceeded {
    skipped: false;
    files: { name: string; urls: number }[];
    includedUrls: number;
    skippedLegacyUrls: number;
}

const SECTION_MAP: Record<string, string> = {
    cloud: "sitemap-cloud.xml",
    guides: "sitemap-guides.xml",
};

const XML_HEADER = '<?xml version="1.0" encoding="UTF-8"?>';
const URLSET_OPEN =
    '<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9" ' +
    'xmlns:news="http://www.google.com/schemas/sitemap-news/0.9" ' +
    'xmlns:xhtml="http://www.w3.org/1999/xhtml" ' +
    'xmlns:image="http://www.google.com/schemas/sitemap-image/1.1" ' +
    'xmlns:video="http://www.google.com/schemas/sitemap-video/1.1">';

function getSitemapFile(loc: string, legacySet: Set<string>, baseUrl: string): string | null {
    const prefix = `${baseUrl}/`;
    const urlPath = loc.startsWith(prefix) ? loc.slice(prefix.length) : loc.replace(/^\//, "");
    const firstSegment = urlPath.split("/")[0];

    if (SECTION_MAP[firstSegment]) {
        return SECTION_MAP[firstSegment];
    }
    if (/^\d+\.\d+$/.test(firstSegment)) {
        if (legacySet.has(firstSegment)) {
            return null;
        }
        return `sitemap-docs-${firstSegment}.xml`;
    }
    return "sitemap-misc.xml";
}

export function splitSitemap(options: SplitOptions): SplitResult | SplitSucceeded {
    const { buildDir, legacyVersions, baseUrl } = options;
    const sitemapPath = path.join(buildDir, "sitemap.xml");

    if (!fs.existsSync(sitemapPath)) {
        return { skipped: true, reason: `no sitemap.xml found in ${buildDir}` };
    }

    const xml = fs.readFileSync(sitemapPath, "utf8");
    const urlBlocks = xml.match(/<url>[\s\S]*?<\/url>/g);
    if (!urlBlocks || urlBlocks.length === 0) {
        return { skipped: true, reason: "sitemap.xml contains no URLs" };
    }

    const legacySet = new Set(legacyVersions);
    const groups: Record<string, string[]> = {};
    let skippedLegacy = 0;

    for (const block of urlBlocks) {
        const locMatch = block.match(/<loc>(.*?)<\/loc>/);
        if (!locMatch) continue;
        const file = getSitemapFile(locMatch[1], legacySet, baseUrl);
        if (!file) {
            skippedLegacy++;
            continue;
        }
        (groups[file] ??= []).push(block);
    }

    const today = new Date().toISOString().split("T")[0];
    const sitemapFiles = Object.keys(groups).sort();
    const result: SplitSucceeded["files"] = [];

    for (const file of sitemapFiles) {
        const content = `${XML_HEADER}\n${URLSET_OPEN}\n${groups[file].join("\n")}\n</urlset>`;
        fs.writeFileSync(path.join(buildDir, file), content, "utf8");
        result.push({ name: file, urls: groups[file].length });
    }

    const indexEntries = sitemapFiles
        .map((f) => `<sitemap><loc>${baseUrl}/${f}</loc><lastmod>${today}</lastmod></sitemap>`)
        .join("\n");

    const sitemapIndex =
        `${XML_HEADER}\n` +
        `<sitemapindex xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">\n` +
        `${indexEntries}\n` +
        `</sitemapindex>`;

    fs.writeFileSync(sitemapPath, sitemapIndex, "utf8");

    return {
        skipped: false,
        files: result,
        includedUrls: urlBlocks.length - skippedLegacy,
        skippedLegacyUrls: skippedLegacy,
    };
}
