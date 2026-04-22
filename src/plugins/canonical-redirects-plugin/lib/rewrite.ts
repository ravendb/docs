/**
 * HTML canonical-link rewriter.
 *
 * Strategy:
 *   - For HTML files under /LEGACY/*  → canonical = self. Docusaurus emits
 *     a current-version canonical for every version; legacy pages are
 *     disallowed in robots.txt but we still stamp a self-canonical so any
 *     accidental crawl points back at the legacy URL rather than a 404.
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
    /** True when a change was made. */
    changed: boolean;
    /** The canonical URL after this pass, or null if no canonical was found. */
    newCanonical: string | null;
    /** True when the redirect map resolved at least one hop for this file. */
    chainResolved: boolean;
}

// Matches canonical links regardless of attribute order, self-closing style, or
// quoting. Docusaurus emits quoted attributes; the swc minifier used by
// @docusaurus/faster strips quotes where legal, so post-minified HTML contains
// `rel=canonical href=https://…` — we must recognize both forms.
const CANONICAL_TAG_REGEX =
    /<link\b[^>]*\brel\s*=\s*(?:"canonical"|'canonical'|canonical(?=[\s/>]))[^>]*>/i;
const HREF_ATTR_REGEX = /\bhref\s*=\s*(?:"([^"]*)"|'([^']*)'|([^\s"'=<>`]+))/i;

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
        return { html, changed: false, newCanonical: null, chainResolved: false };
    }

    const originalTag = tagMatch[0];
    const originalHref = extractHref(originalTag);

    let desiredCanonical: string;
    let chainResolved = false;

    if (isLegacy(fileVersion, legacyVersions)) {
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

    if (originalHref && stripTrailingSlash(originalHref) === desiredCanonical) {
        return { html, changed: false, newCanonical: desiredCanonical, chainResolved };
    }

    const newTag = replaceHref(originalTag, desiredCanonical);
    const newHtml = html.replace(originalTag, newTag);

    return { html: newHtml, changed: true, newCanonical: desiredCanonical, chainResolved };
}
