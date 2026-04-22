/**
 * HTML canonical-link rewriter.
 *
 * Strategy:
 *   - For HTML files under /LEGACY/*  → canonical = self, plus inject
 *     <meta name="robots" content="noindex,follow"> into <head> if a robots
 *     meta isn't already present. Docusaurus emits a current-version
 *     canonical for every version; legacy pages are disallowed in
 *     robots.txt but we still stamp a self-canonical so any accidental
 *     crawl points back at the legacy URL rather than a 404, and the
 *     noindex meta is the belt to robots.txt's braces for crawlers that
 *     ignore robots.txt.
 *   - For HTML files under /INDEXED/* → canonical = current-version URL,
 *     resolved through the redirect chain. This is the core fix: when a
 *     page moves or renames in the current version, every older version
 *     of that page gets its canonical rewritten to the new current URL.
 *
 * Skips <meta property="og:url"> and <meta name="twitter:url"> by design —
 * canonical is the search-signalling field that matters.
 */

import type { RedirectMap } from "./redirects.js";
import { resolveChain } from "./redirects.js";

export interface RewriteInput {
    /** The HTML document text. */
    html: string;
    /** The version prefix of the file being rewritten (e.g. "7.2", "6.2"). */
    fileVersion: string;
    /** The versionless path of the file (e.g. "/client-api/foo"). */
    versionlessPath: string;
    /** Current (= latest indexed) version. */
    currentVersion: string;
    /** Legacy versions — files under these get self-canonical. */
    legacyVersions: readonly string[];
    /** Redirect map, keyed on versionless source paths. */
    redirects: RedirectMap;
    /** Site origin, e.g. "https://docs.ravendb.net". No trailing slash. */
    baseUrl: string;
}

export interface RewriteResult {
    /** Rewritten HTML (or input unchanged if no rewrite was required). */
    html: string;
    /** True when a change was made (either canonical or meta injection). */
    changed: boolean;
    /** The canonical URL after this pass, or null if no canonical was found. */
    newCanonical: string | null;
    /** True when the redirect map resolved at least one hop for this file. */
    chainResolved: boolean;
    /**
     * True when a `<meta name="robots" content="noindex,follow">` tag was
     * injected into this legacy page's `<head>`. False for current/indexed
     * versions, for legacy pages that already had a robots meta, and for
     * pages without a `<head>` tag.
     */
    noindexInjected: boolean;
}

// Matches canonical links regardless of attribute order, self-closing style, or
// quoting. Docusaurus emits quoted attributes; the swc minifier used by
// @docusaurus/faster strips quotes where legal, so post-minified HTML contains
// `rel=canonical href=https://…` — we must recognize both forms.
const CANONICAL_TAG_REGEX =
    /<link\b[^>]*\brel\s*=\s*(?:"canonical"|'canonical'|canonical(?=[\s/>]))[^>]*>/i;
const HREF_ATTR_REGEX = /\bhref\s*=\s*(?:"([^"]*)"|'([^']*)'|([^\s"'=<>`]+))/i;
// Detects an existing <meta name="robots" ...> — any form / quoting. If one
// already exists we leave it alone, even if it says "index,follow", because
// the author presumably overrode the default intentionally.
const ROBOTS_META_REGEX =
    /<meta\b[^>]*\bname\s*=\s*(?:"robots"|'robots'|robots(?=[\s/>]))[^>]*>/i;
const HEAD_OPEN_REGEX = /<head\b[^>]*>/i;
const NOINDEX_META_TAG = `<meta name="robots" content="noindex,follow">`;

function extractHref(tag: string): string | null {
    const m = tag.match(HREF_ATTR_REGEX);
    if (!m) return null;
    return m[1] ?? m[2] ?? m[3] ?? null;
}

function replaceHref(tag: string, newHref: string): string {
    return tag.replace(HREF_ATTR_REGEX, `href="${newHref}"`);
}

function isLegacy(version: string, legacyVersions: readonly string[]): boolean {
    return legacyVersions.includes(version);
}

/** Strip trailing slash, but preserve "/" itself. */
function stripTrailingSlash(p: string): string {
    return p.length > 1 && p.endsWith("/") ? p.slice(0, -1) : p;
}

/**
 * Pure function: given the inputs, return the rewritten HTML.
 * No I/O — callers handle reading and writing files.
 */
export function rewriteHtml(input: RewriteInput): RewriteResult {
    const { html, fileVersion, versionlessPath, currentVersion, legacyVersions, redirects, baseUrl } = input;

    const tagMatch = html.match(CANONICAL_TAG_REGEX);
    if (!tagMatch) {
        return {
            html,
            changed: false,
            newCanonical: null,
            chainResolved: false,
            noindexInjected: false,
        };
    }

    const originalTag = tagMatch[0];
    const originalHref = extractHref(originalTag);

    const legacy = isLegacy(fileVersion, legacyVersions);

    let desiredCanonical: string;
    let chainResolved = false;

    if (legacy) {
        // Self-canonical for legacy pages.
        desiredCanonical = `${baseUrl}/${fileVersion}${versionlessPath === "/" ? "" : versionlessPath}`;
    } else {
        // Resolve against the CURRENT version: the canonical expresses
        // "the current-version URL equivalent of this page". Per-hop gating
        // still applies via each rule's minimumVersion.
        const resolved = resolveChain(redirects, versionlessPath, currentVersion);
        chainResolved = resolved !== versionlessPath;
        desiredCanonical = `${baseUrl}/${currentVersion}${resolved === "/" ? "" : resolved}`;
    }

    // Normalize trailing slash — Docusaurus-emitted canonicals don't have one.
    desiredCanonical = stripTrailingSlash(desiredCanonical);

    const canonicalNeedsUpdate = !originalHref || stripTrailingSlash(originalHref) !== desiredCanonical;

    let workingHtml = html;
    if (canonicalNeedsUpdate) {
        const newTag = replaceHref(originalTag, desiredCanonical);
        workingHtml = html.replace(originalTag, newTag);
    }

    // Legacy-only: inject noindex,follow after <head> if no robots meta exists.
    // Idempotent — a second pass finds the tag we just wrote and skips.
    let noindexInjected = false;
    if (legacy && !ROBOTS_META_REGEX.test(workingHtml)) {
        const headMatch = workingHtml.match(HEAD_OPEN_REGEX);
        if (headMatch) {
            const headTag = headMatch[0];
            workingHtml = workingHtml.replace(headTag, `${headTag}${NOINDEX_META_TAG}`);
            noindexInjected = true;
        }
    }

    const changed = canonicalNeedsUpdate || noindexInjected;

    return {
        html: workingHtml,
        changed,
        newCanonical: desiredCanonical,
        chainResolved,
        noindexInjected,
    };
}
