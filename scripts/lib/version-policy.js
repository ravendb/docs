/**
 * Single source of truth for version policy. When a version ages out of support,
 * move it from versions.json into LEGACY_VERSIONS here.
 *
 * CommonJS so Node scripts, compiled TS, and ES-module consumers can all import it.
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
