/**
 * Single source of truth for version policy.
 *
 * Consumed by:
 *   - docusaurus.config.ts (customFields, versions.current, softwareVersion, sitemap ignorePatterns)
 *   - scripts/generate-robots.js (legacy-version Disallow blocks)
 *   - scripts/split-sitemap.js / split-sitemap plugin (exclude legacy from index)
 *   - scripts/handle_redirects.js (default version fallback at the edge)
 *   - src/plugins/canonical-redirects-plugin (self-canonical rule for legacy)
 *
 * When a version ages out of support, move it from versions.json into LEGACY_VERSIONS
 * here. All downstream consumers update on the next build.
 *
 * CommonJS so Node scripts (plain .js), compiled TS (docusaurus.config), and
 * ES-module consumers (via interop) can all import from a single file.
 */

const CURRENT_VERSION = "7.2";

const LEGACY_VERSIONS = [
    "1.0",
    "2.0",
    "2.5",
    "3.0",
    "3.5",
    "4.0",
    "4.1",
    "4.2",
    "5.0",
    "5.1",
    "5.2",
    "5.3",
    "5.4",
    "6.0",
    "7.0",
];

module.exports = { CURRENT_VERSION, LEGACY_VERSIONS };
