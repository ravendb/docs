#!/usr/bin/env -S tsx
/**
 * Standalone validator for scripts/redirects.json.
 *
 * Wraps the same validateRedirects function used by the canonical-redirects
 * plugin at loadContent time, but runs without a full Docusaurus build so it
 * can be wired into CI as a fast pre-gate.
 *
 * Exit code: 0 on clean file, 1 on any error.
 */

import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

import {
    loadRedirects,
    validateRedirects,
} from "../src/plugins/canonical-redirects-plugin/lib/redirects.js";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const redirectsPath = path.join(__dirname, "redirects.json");

if (!fs.existsSync(redirectsPath)) {
    console.error(`validate-redirects: file not found: ${redirectsPath}`);
    process.exit(1);
}

let rules;
try {
    rules = loadRedirects(redirectsPath);
} catch (err) {
    console.error(`validate-redirects: failed to read ${redirectsPath}`);
    console.error(err instanceof Error ? err.message : err);
    process.exit(1);
}

const errors = validateRedirects(rules);
if (errors.length === 0) {
    console.log(`validate-redirects: OK (${rules.length} rule${rules.length === 1 ? "" : "s"})`);
    process.exit(0);
}

console.error(`validate-redirects: ${errors.length} error${errors.length === 1 ? "" : "s"} in ${redirectsPath}`);
for (const e of errors) {
    console.error(`  [${e.index}] ${e.key ? `'${e.key}' — ` : ""}${e.message}`);
}
process.exit(1);
