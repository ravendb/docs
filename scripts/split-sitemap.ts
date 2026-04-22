/**
 * Post-build script: splits Docusaurus's single sitemap.xml into section-based
 * sub-sitemaps and rewrites sitemap.xml as a <sitemapindex>.
 *
 * Runs as a post-`docusaurus build` step rather than a Docusaurus plugin because
 * @docusaurus/plugin-sitemap emits sitemap.xml *after* user plugin postBuild
 * hooks — so a plugin here would see no sitemap.xml and skip silently.
 *
 * The behavior-carrying logic lives in src/plugins/split-sitemap-plugin/lib/split.ts
 * where it's unit-tested in isolation. This script is just the runtime shell.
 *
 * Usage:  tsx scripts/split-sitemap.ts [buildDir]
 *         Defaults to ./build
 */

import path from "node:path";
import { fileURLToPath } from "node:url";
import { splitSitemap } from "../src/lib/split-sitemap/lib/split.js";
import { LEGACY_VERSIONS } from "./lib/version-policy.js";

const BASE_URL = "https://docs.ravendb.net";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const buildDir = process.argv[2] ?? path.join(__dirname, "..", "build");

const result = splitSitemap({ buildDir, legacyVersions: LEGACY_VERSIONS, baseUrl: BASE_URL });

if (result.skipped) {
    console.log(`[split-sitemap] skipped: ${result.reason}`);
    process.exit(0);
}

for (const { name, urls } of result.files) {
    console.log(`[split-sitemap]   ${name}: ${urls} URLs`);
}
console.log(
    `[split-sitemap] split into ${result.files.length} sub-sitemaps ` +
        `(${result.includedUrls} URLs included, ${result.skippedLegacyUrls} legacy URLs excluded)`
);
