/**
 * Canonical redirects plugin.
 *
 * Rewrites <link rel="canonical"> URLs in every emitted HTML file so that
 * pages moved or renamed in the current (=latest indexed) version advertise
 * their new home rather than a stale URL that 404s. Legacy versions get a
 * self-canonical. After rewriting, canonicals are verified against the
 * Docusaurus route universe so strict builds fail loudly on bad targets.
 *
 * See plan RDoc-3786 for the full design.
 */

import type { LoadContext, Plugin } from "@docusaurus/types";
import fs from "fs";
import path from "path";

import {
    buildRedirectMap,
    loadRedirects,
    validateNoCycles,
    validateRedirects,
    validateTargetsExist,
    type RedirectMap,
} from "./lib/redirects.js";
import { rewriteHtml } from "./lib/rewrite.js";
import { buildUniverse, verifyCanonicals, type CanonicalRecord } from "./lib/verify.js";
// version-policy.js is CJS; named-import interop lets both tsx --test and
// the Docusaurus webpack loader resolve it without a shim.
import { CURRENT_VERSION, LEGACY_VERSIONS } from "../../../scripts/lib/version-policy.js";

export interface CanonicalRedirectsPluginOptions {
    /** Absolute or site-relative path to redirects.json. */
    redirectsPath?: string;
    /** When true, verifier issues fail the build. Defaults to `DOCUSAURUS_STRICT_CANONICALS === "true"`. */
    failOnInvalidCanonical?: boolean;
    /** "silent" | "info" | "verbose" (default "info"). */
    logLevel?: "silent" | "info" | "verbose";
}

type ResolvedOptions = Required<CanonicalRedirectsPluginOptions>;

const BASE_URL = "https://docs.ravendb.net";
const VERSION_SEGMENT_REGEX = /^(\d+\.\d+)(\/.*)?$/;

function resolveOptions(opts: CanonicalRedirectsPluginOptions | undefined, siteDir: string): ResolvedOptions {
    const userOpts = opts ?? {};
    const providedPath = userOpts.redirectsPath;
    const redirectsPath = providedPath
        ? path.isAbsolute(providedPath)
            ? providedPath
            : path.join(siteDir, providedPath)
        : path.join(siteDir, "scripts", "redirects.json");

    return {
        redirectsPath,
        failOnInvalidCanonical: userOpts.failOnInvalidCanonical ?? process.env.DOCUSAURUS_STRICT_CANONICALS === "true",
        logLevel: userOpts.logLevel ?? "info",
    };
}

function logger(level: ResolvedOptions["logLevel"]) {
    const shouldLog = (lvl: "info" | "verbose") => {
        if (level === "silent") return false;
        if (level === "info") return lvl === "info";
        return true; // verbose
    };
    return {
        info: (msg: string) => {
            if (shouldLog("info")) console.log(`[canonical-redirects] ${msg}`);
        },
        verbose: (msg: string) => {
            if (shouldLog("verbose")) console.log(`[canonical-redirects] ${msg}`);
        },
        warn: (msg: string) => {
            // Always show warnings (unless fully silent).
            if (level !== "silent") console.warn(`[canonical-redirects] ${msg}`);
        },
    };
}

/** Recursively walk `rootDir`, invoking `onFile` for every file encountered. */
function walk(rootDir: string, onFile: (filePath: string) => void): void {
    const entries = fs.readdirSync(rootDir, { withFileTypes: true });
    for (const entry of entries) {
        const full = path.join(rootDir, entry.name);
        if (entry.isDirectory()) {
            walk(full, onFile);
        } else if (entry.isFile()) {
            onFile(full);
        }
    }
}

/**
 * Given an HTML file's path relative to `outDir` (using forward slashes),
 * extract [version, versionlessPath].
 *
 * Returns `null` as an intentional signal that the file is outside the
 * versioned docs tree and therefore should be skipped by the canonical
 * rewriter — e.g. the top-level /search and / routes, or the non-versioned
 * /cloud, /guides, /templates sections. Callers rely on this skip; do not
 * treat `null` as an error.
 */
function extractVersionInfo(relPath: string): { version: string; versionlessPath: string } | null {
    // Strip trailing /index.html
    const noIndex = relPath.replace(/\/index\.html$/i, "");
    const match = noIndex.match(VERSION_SEGMENT_REGEX);
    if (!match) return null;
    const version = match[1];
    const tail = match[2] ?? "";
    // versionlessPath: leading slash, no trailing slash (except for version root "/")
    const versionlessPath = tail === "" ? "/" : tail;
    return { version, versionlessPath };
}

const canonicalRedirectsPlugin = function canonicalRedirectsPlugin(
    context: LoadContext,
    pluginOptions?: CanonicalRedirectsPluginOptions
): Plugin<{ redirectMap: RedirectMap }> {
    const options = resolveOptions(pluginOptions, context.siteDir);
    const log = logger(options.logLevel);

    return {
        name: "canonical-redirects-plugin",

        async loadContent() {
            if (!fs.existsSync(options.redirectsPath)) {
                throw new Error(`canonical-redirects-plugin: redirects file not found at ${options.redirectsPath}`);
            }

            const rules = loadRedirects(options.redirectsPath);
            // Structural validation first, then cycle detection + target-
            // existence on the structurally-sound rule set. Cycles and dead
            // targets are build-time errors because the edge handler trusts
            // this pre-gate and doesn't carry its own guards.
            const structuralErrors = validateRedirects(rules);
            const errors =
                structuralErrors.length > 0
                    ? structuralErrors
                    : [...validateNoCycles(rules), ...validateTargetsExist(rules, context.siteDir)];
            if (errors.length > 0) {
                const rendered = errors
                    .map((e) => `  [${e.index}] ${e.key ? `'${e.key}' — ` : ""}${e.message}`)
                    .join("\n");
                throw new Error(`canonical-redirects-plugin: redirects.json failed validation:\n${rendered}`);
            }

            log.info(`loaded ${rules.length} redirect rule(s) from ${options.redirectsPath}`);
            return { redirectMap: buildRedirectMap(rules) };
        },

        async postBuild(props) {
            const content = (props as unknown as { content: { redirectMap: RedirectMap } }).content;
            const redirectMap = content?.redirectMap;
            if (!redirectMap) {
                log.warn("no redirect map available in postBuild; skipping");
                return;
            }

            const { outDir, routesPaths } = props;
            const universe = buildUniverse(routesPaths, CURRENT_VERSION);
            log.verbose(`current-version universe: ${universe.size} routes`);

            const records: CanonicalRecord[] = [];
            let scanned = 0;
            let rewritten = 0;
            let chainsResolved = 0;
            let noindexInjectedCount = 0;
            let missingCanonicalCount = 0;

            walk(outDir, (filePath) => {
                if (!filePath.endsWith(".html")) return;
                const rel = path.relative(outDir, filePath).split(path.sep).join("/");
                const info = extractVersionInfo(rel);
                if (!info) return; // non-versioned file (search, cloud, guides, templates, root)

                scanned++;

                const originalHtml = fs.readFileSync(filePath, "utf8");

                const result = rewriteHtml({
                    html: originalHtml,
                    fileVersion: info.version,
                    versionlessPath: info.versionlessPath,
                    currentVersion: CURRENT_VERSION,
                    legacyVersions: LEGACY_VERSIONS,
                    redirects: redirectMap,
                    baseUrl: BASE_URL,
                });

                if (result.changed) {
                    fs.writeFileSync(filePath, result.html, "utf8");
                    rewritten++;
                }
                if (result.chainResolved) chainsResolved++;
                if (result.noindexInjected) noindexInjectedCount++;

                if (result.newCanonical) {
                    records.push({
                        file: rel,
                        canonical: result.newCanonical,
                        fileVersion: info.version,
                    });
                } else {
                    missingCanonicalCount++;
                }
            });

            log.info(
                `scanned ${scanned} versioned HTML file(s), rewrote ${rewritten}, chains resolved ${chainsResolved}, missing canonicals ${missingCanonicalCount}, noindex injected ${noindexInjectedCount}`
            );

            // Missing canonicals mean CANONICAL_TAG_REGEX is stale — fix the regex, not each file.
            if (missingCanonicalCount > 0) {
                const msg = `canonical-redirects-plugin: ${missingCanonicalCount} versioned HTML file(s) emitted without <link rel="canonical"> — update CANONICAL_TAG_REGEX in lib/rewrite.ts`;
                if (options.failOnInvalidCanonical) throw new Error(msg);
                log.warn(msg);
            }

            const issues = verifyCanonicals({
                records,
                universe,
                currentVersion: CURRENT_VERSION,
                legacyVersions: LEGACY_VERSIONS,
                baseUrl: BASE_URL,
            });

            if (issues.length > 0) {
                const maxShow = 10;
                const shown = issues.slice(0, maxShow);
                const header = `canonical-redirects-plugin: ${issues.length} invalid canonical${issues.length === 1 ? "" : "s"} detected`;
                const body = shown
                    .map((i) => {
                        const base = `  - ${i.file}\n      canonical: ${i.canonical}\n      reason: ${i.reason}`;
                        if (!i.fix) return base;
                        // Indent the fix block two extra spaces under the issue bullet
                        // so it reads as a sub-block in the terminal.
                        const indentedFix = i.fix
                            .split("\n")
                            .map((l) => `      ${l}`)
                            .join("\n");
                        return `${base}\n      fix:\n${indentedFix}`;
                    })
                    .join("\n");
                const tail = issues.length > maxShow ? `\n  … and ${issues.length - maxShow} more` : "";
                const message = `${header}\n${body}${tail}`;

                if (options.failOnInvalidCanonical) {
                    throw new Error(message);
                }
                log.warn(message);
            } else {
                log.info("all canonicals verified against the current-version route universe");
            }
        },
    };
};

export default canonicalRedirectsPlugin;
