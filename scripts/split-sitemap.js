/**
 * Post-build script: splits the Docusaurus-generated sitemap.xml into
 * section-based sub-sitemaps and a sitemap index.
 *
 * Sections are defined by the first path segment of each URL:
 *   /cloud/*   → sitemap-cloud.xml
 *   /guides/*  → sitemap-guides.xml
 *   /7.2/*     → sitemap-docs.xml   (current version)
 *   (other)    → sitemap-misc.xml   (search, root pages, etc.)
 *
 * The original sitemap.xml is replaced with a <sitemapindex> that
 * references each sub-sitemap.
 *
 * Usage:  node scripts/split-sitemap.js [buildDir]
 *         Defaults to ./build
 */

const fs = require("fs");
const path = require("path");

const buildDir = process.argv[2] || path.join(__dirname, "..", "build");
const sitemapPath = path.join(buildDir, "sitemap.xml");

if (!fs.existsSync(sitemapPath)) {
    console.log("No sitemap.xml found in", buildDir, "— skipping split.");
    process.exit(0);
}

const xml = fs.readFileSync(sitemapPath, "utf8");

// Extract all <url>…</url> blocks
const urlBlocks = xml.match(/<url>[\s\S]*?<\/url>/g);
if (!urlBlocks || urlBlocks.length === 0) {
    console.log("sitemap.xml contains no URLs — skipping split.");
    process.exit(0);
}

// Map first path segment → sitemap filename
const sectionMap = {
    cloud: "sitemap-cloud.xml",
    guides: "sitemap-guides.xml",
};

// Minimum version to include in sitemaps (versions below this are legacy)
const MIN_VERSION = 6.0;

function getSitemapFile(loc) {
    const urlPath = loc.replace("https://docs.ravendb.net/", "");
    const firstSegment = urlPath.split("/")[0];

    if (sectionMap[firstSegment]) {
        return sectionMap[firstSegment];
    }
    // Version-prefixed docs (e.g. 7.2/*, 6.2/*, 7.1/*)
    if (/^\d+\.\d+$/.test(firstSegment)) {
        const version = parseFloat(firstSegment);
        if (version < MIN_VERSION) {
            return null; // Exclude legacy versions
        }
        return `sitemap-docs-${firstSegment}.xml`;
    }
    return "sitemap-misc.xml";
}

// Group URL blocks by target sitemap file
const groups = {};
let skippedLegacy = 0;
for (const block of urlBlocks) {
    const locMatch = block.match(/<loc>(.*?)<\/loc>/);
    if (!locMatch) continue;
    const file = getSitemapFile(locMatch[1]);
    if (!file) {
        skippedLegacy++;
        continue;
    }
    if (!groups[file]) groups[file] = [];
    groups[file].push(block);
}

// XML header used by Docusaurus
const xmlHeader = '<?xml version="1.0" encoding="UTF-8"?>';
const urlsetOpen =
    '<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9" ' +
    'xmlns:news="http://www.google.com/schemas/sitemap-news/0.9" ' +
    'xmlns:xhtml="http://www.w3.org/1999/xhtml" ' +
    'xmlns:image="http://www.google.com/schemas/sitemap-image/1.1" ' +
    'xmlns:video="http://www.google.com/schemas/sitemap-video/1.1">';

// Write each sub-sitemap
const today = new Date().toISOString().split("T")[0];
const sitemapFiles = Object.keys(groups).sort();

for (const file of sitemapFiles) {
    const content = `${xmlHeader}\n${urlsetOpen}\n${groups[file].join("\n")}\n</urlset>`;
    fs.writeFileSync(path.join(buildDir, file), content, "utf8");
    console.log(`  ${file}: ${groups[file].length} URLs`);
}

// Write sitemap index, replacing the original sitemap.xml
const indexEntries = sitemapFiles
    .map((file) => `<sitemap><loc>https://docs.ravendb.net/${file}</loc><lastmod>${today}</lastmod></sitemap>`)
    .join("\n");

const sitemapIndex =
    `${xmlHeader}\n` +
    `<sitemapindex xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">\n` +
    `${indexEntries}\n` +
    `</sitemapindex>`;

fs.writeFileSync(sitemapPath, sitemapIndex, "utf8");

const includedCount = urlBlocks.length - skippedLegacy;
console.log(
    `\nSplit sitemap.xml into ${sitemapFiles.length} sub-sitemaps (${includedCount} URLs included, ${skippedLegacy} legacy URLs excluded)`
);
