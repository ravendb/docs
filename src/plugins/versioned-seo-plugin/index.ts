// Versioned-SEO plugin — three postBuild assertions over emitted HTML:
//   1. canonical rewrite + verify (current-version slice)
//   2. legacy-version noindex audit (Docusaurus injects, we assert)
//   3. see_also link audit (existence + versionless format)
// See RDoc-3786 / RDoc-3789 for full design.

import docusaurusLogger from "@docusaurus/logger";
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
import { buildScopedRoutes, hasNoindexRobotsMeta, verifyCanonicals, type CanonicalRecord } from "./lib/verify.js";
import { fanoutSeeAlsoRecords, verifySeeAlsoLinks, type SeeAlsoRecord } from "./lib/verify-seealso.js";
// version-policy.js is CJS; named-import interop lets both tsx --test and
// the Docusaurus webpack loader resolve it without a shim.
import { BUILT_VERSIONS, CURRENT_VERSION, LEGACY_VERSIONS } from "../../../scripts/lib/version-policy.js";

export interface VersionedSeoPluginOptions {
    /** Absolute or site-relative path to redirects.json. */
    redirectsPath?: string;
    /** When true, verifier issues fail the build. Defaults to `DOCUSAURUS_STRICT_SEO === "true"`. */
    failOnInvalidSeo?: boolean;
    /** "silent" | "info" | "verbose" (default "info"). */
    logLevel?: "silent" | "info" | "verbose";
}

type ResolvedOptions = Required<VersionedSeoPluginOptions>;

const BASE_URL = "https://docs.ravendb.net";
const VERSION_SEGMENT_REGEX = /^(\d+\.\d+)(\/.*)?$/;
const PLUGIN_NAME = "versioned-seo-plugin";
const LOG_PREFIX = "[versioned-seo]";

function resolveOptions(opts: VersionedSeoPluginOptions | undefined, siteDir: string): ResolvedOptions {
    const userOpts = opts ?? {};
    const providedPath = userOpts.redirectsPath;
    const redirectsPath = providedPath
        ? path.isAbsolute(providedPath)
            ? providedPath
            : path.join(siteDir, providedPath)
        : path.join(siteDir, "scripts", "redirects.json");

    return {
        redirectsPath,
        failOnInvalidSeo: userOpts.failOnInvalidSeo ?? process.env.DOCUSAURUS_STRICT_SEO === "true",
        logLevel: userOpts.logLevel ?? "info",
    };
}

function logger(level: ResolvedOptions["logLevel"]) {
    const shouldLog = (lvl: "info" | "verbose") => {
        if (level === "silent") {
            return false;
        }
        if (level === "info") {
            return lvl === "info";
        }
        return true; // verbose
    };
    return {
        info: (msg: string) => {
            if (shouldLog("info")) {
                docusaurusLogger.info(`${LOG_PREFIX} ${msg}`);
            }
        },
        verbose: (msg: string) => {
            if (shouldLog("verbose")) {
                docusaurusLogger.info(`${LOG_PREFIX} ${msg}`);
            }
        },
        warn: (msg: string) => {
            // Always show warnings (unless fully silent).
            if (level !== "silent") {
                docusaurusLogger.warn(`${LOG_PREFIX} ${msg}`);
            }
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
    if (!match) {
        return null;
    }
    const version = match[1];
    const tail = match[2] ?? "";
    // versionlessPath: leading slash, no trailing slash (except for version root "/")
    const versionlessPath = tail === "" ? "/" : tail;
    return { version, versionlessPath };
}

const versionedSeoPlugin = function versionedSeoPlugin(
    context: LoadContext,
    pluginOptions?: VersionedSeoPluginOptions
): Plugin<{ redirectMap: RedirectMap }> {
    const options = resolveOptions(pluginOptions, context.siteDir);
    const log = logger(options.logLevel);

    return {
        name: PLUGIN_NAME,

        async loadContent() {
            if (!fs.existsSync(options.redirectsPath)) {
                throw new Error(`${PLUGIN_NAME}: redirects file not found at ${options.redirectsPath}`);
            }

            const rules = loadRedirects(options.redirectsPath);
            const structuralErrors = validateRedirects(rules);
            const errors =
                structuralErrors.length > 0
                    ? structuralErrors
                    : [
                          ...validateNoCycles(rules),
                          ...validateTargetsExist(rules, context.siteDir, CURRENT_VERSION, BUILT_VERSIONS),
                      ];
            if (errors.length > 0) {
                const rendered = errors
                    .map((e) => `  [${e.index}] ${e.key ? `'${e.key}' — ` : ""}${e.message}`)
                    .join("\n");
                throw new Error(`${PLUGIN_NAME}: redirects.json failed validation:\n${rendered}`);
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
            const routesByScope = buildScopedRoutes(routesPaths, LEGACY_VERSIONS);
            const latestVersionRoutes = routesByScope.get(CURRENT_VERSION);
            if (!latestVersionRoutes) {
                throw new Error(
                    `${PLUGIN_NAME}: CURRENT_VERSION ${CURRENT_VERSION} produced no routes — DOCUSAURUS_VERSIONS must include 'current'`
                );
            }
            log.verbose(`current-version universe: ${latestVersionRoutes.size} routes`);

            const canonicalRecords: CanonicalRecord[] = [];
            const seeAlsoRecords: SeeAlsoRecord[] = [];
            let scanned = 0;
            let rewritten = 0;
            let chainsResolved = 0;
            let missingCanonicalCount = 0;
            let legacyChecked = 0;
            const legacyMissingNoindex: { file: string; fileVersion: string }[] = [];

            walk(outDir, (filePath) => {
                if (!filePath.endsWith(".html")) {
                    return;
                }
                const rel = path.relative(outDir, filePath).split(path.sep).join("/");
                const info = extractVersionInfo(rel);
                const originalHtml = fs.readFileSync(filePath, "utf8");

                // SeeAlso audit: currently-maintained versioned pages + /cloud + /guides articles.
                const shouldAuditSeeAlso =
                    (info && !LEGACY_VERSIONS.includes(info.version)) ||
                    (!info && (rel.startsWith("cloud/") || rel.startsWith("guides/")));
                if (shouldAuditSeeAlso) {
                    seeAlsoRecords.push(
                        ...fanoutSeeAlsoRecords(
                            originalHtml,
                            "/" + rel.replace(/\/index\.html$/i, "").replace(/\.html$/i, ""),
                            info?.version ?? null
                        )
                    );
                }

                if (!info) {
                    return;
                } // non-versioned file (search, cloud, guides, templates, root)

                scanned++;

                // Legacy-noindex assertion runs against the HTML Docusaurus
                // emitted, before our rewrite touches the canonical href —
                // that's the surface we're auditing.
                if (LEGACY_VERSIONS.includes(info.version)) {
                    legacyChecked++;
                    if (!hasNoindexRobotsMeta(originalHtml)) {
                        legacyMissingNoindex.push({ file: rel, fileVersion: info.version });
                    }
                }

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
                if (result.chainResolved) {
                    chainsResolved++;
                }

                if (result.newCanonical) {
                    canonicalRecords.push({
                        file: rel,
                        canonical: result.newCanonical,
                        fileVersion: info.version,
                    });
                } else {
                    missingCanonicalCount++;
                }
            });

            const seeAlsoBroken = verifySeeAlsoLinks({
                records: seeAlsoRecords,
                routesByScope,
                latestVersion: CURRENT_VERSION,
            });

            log.info(
                `scanned ${scanned} versioned HTML file(s), rewrote ${rewritten}, chains resolved ${chainsResolved}, missing canonicals ${missingCanonicalCount}, legacy noindex checked ${legacyChecked} (missing ${legacyMissingNoindex.length}), see_also fanned out ${seeAlsoRecords.length} (broken ${seeAlsoBroken.length})`
            );

            // Missing canonicals mean CANONICAL_TAG_REGEX is stale — fix the regex, not each file.
            if (missingCanonicalCount > 0) {
                const msg = `${PLUGIN_NAME}: ${missingCanonicalCount} versioned HTML file(s) emitted without <link rel="canonical"> — update CANONICAL_TAG_REGEX in lib/rewrite.ts`;
                if (options.failOnInvalidSeo) {
                    throw new Error(msg);
                }
                log.warn(msg);
            }

            // Legacy noindex assertion — Docusaurus owns the injection; we own the audit.
            if (legacyMissingNoindex.length > 0) {
                const versions = [...new Set(legacyMissingNoindex.map((r) => r.fileVersion))].sort();
                const maxShow = 10;
                const shown = legacyMissingNoindex.slice(0, maxShow);
                const header = `${PLUGIN_NAME}: ${legacyMissingNoindex.length} legacy-version page(s) missing <meta name="robots" content="noindex,...">`;
                const versionsLine = `  affected versions: ${versions.join(", ")}`;
                const fixHint = versions.map((v) => `      versions: { "${v}": { noIndex: true }, ... }`).join("\n");
                const fix = [
                    "  fix:",
                    `      check that each version above has \`noIndex: true\` in the docs preset's \`versions:\` map in docusaurus.config.ts:`,
                    fixHint,
                    `      and that no swizzle of @theme/DocVersionRoot drops the \`version.noIndex && <meta>\` line.`,
                ].join("\n");
                const body = shown.map((r) => `  - ${r.file} (version ${r.fileVersion})`).join("\n");
                const tail =
                    legacyMissingNoindex.length > maxShow
                        ? `\n  … and ${legacyMissingNoindex.length - maxShow} more`
                        : "";
                const message = `${header}\n${versionsLine}\n${body}${tail}\n${fix}`;

                if (options.failOnInvalidSeo) {
                    throw new Error(message);
                }
                log.warn(message);
            } else if (legacyChecked > 0) {
                log.info(
                    `all ${legacyChecked} legacy-version page(s) carry <meta name="robots" content="noindex,...">`
                );
            }

            const issues = verifyCanonicals({
                records: canonicalRecords,
                universe: latestVersionRoutes,
                currentVersion: CURRENT_VERSION,
                legacyVersions: LEGACY_VERSIONS,
                baseUrl: BASE_URL,
            });

            // Always emit the full report via log.warn so it surfaces in build output regardless of
            // failOnInvalidSeo. failOnInvalidSeo only controls whether the build dies; the report is
            // unconditional, and a canonical failure can't suppress the seeAlso report (or vice versa).
            const fatalCount = issues.length + seeAlsoBroken.length;

            if (issues.length > 0) {
                const header = `${PLUGIN_NAME}: ${issues.length} invalid canonical${issues.length === 1 ? "" : "s"} detected`;
                const body = issues
                    .map((i) => {
                        const base = `  - ${i.file}\n      canonical: ${i.canonical}\n      reason: ${i.reason}`;
                        if (!i.fix) {
                            return base;
                        }
                        // Indent the fix block two extra spaces under the issue bullet
                        // so it reads as a sub-block in the terminal.
                        const indentedFix = i.fix
                            .split("\n")
                            .map((l) => `      ${l}`)
                            .join("\n");
                        return `${base}\n      fix:\n${indentedFix}`;
                    })
                    .join("\n");
                log.warn(`${header}\n${body}`);
            } else {
                log.info("all canonicals verified against the current-version route universe");
            }

            // SeeAlso audit.
            if (seeAlsoBroken.length > 0) {
                const noun = seeAlsoBroken.length === 1 ? "link" : "links";
                const header = `${PLUGIN_NAME}: ${seeAlsoBroken.length} broken see_also ${noun} detected`;
                const body = seeAlsoBroken
                    .map(
                        (i) =>
                            `  - ${i.articlePath}\n      href: ${i.href}\n      reason: ${i.reason}\n      fix: ${i.fix}`
                    )
                    .join("\n");
                log.warn(`${header}\n${body}`);
            }

            if (fatalCount > 0 && options.failOnInvalidSeo) {
                throw new Error(
                    `${PLUGIN_NAME}: ${fatalCount} SEO issue${fatalCount === 1 ? "" : "s"} detected (see warnings above)`
                );
            }
        },
    };
};

export default versionedSeoPlugin;
