/**
 * Version policy knobs. Some control SEO (canonicals, noindex); others feed
 * components that walk every served version (redirect validator, sitemap,
 * edge handler).
 *
 * CommonJS for cross-consumer interop.
 */

const CURRENT_VERSION = "7.2";

// Non-current versions still supported and indexed.
const ACTIVE_VERSIONS = ["7.1", "6.2"];

// Older versions kept on disk but noindex'd.
const LEGACY_VERSIONS = [
    "7.0",
    "6.0",
    "5.4",
    "5.3",
    "5.2",
    "5.1",
    "5.0",
    "4.2",
    "4.1",
    "4.0",
    "3.5",
    "3.0",
    "2.5",
    "2.0",
    "1.0",
];

// Every version with content on disk — current + active + legacy. Derived
// view for consumers that walk all served versions (redirect target
// validator, sitemap builder, edge handler).
const BUILT_VERSIONS = [CURRENT_VERSION, ...ACTIVE_VERSIONS, ...LEGACY_VERSIONS];

module.exports = { CURRENT_VERSION, ACTIVE_VERSIONS, LEGACY_VERSIONS, BUILT_VERSIONS };
