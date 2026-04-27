/**
 * Canonical verifier.
 *
 * After the rewriter has run across every HTML file, confirms that each
 * emitted canonical URL actually resolves to a page in the final site —
 * i.e. the canonical's path is in the Docusaurus `routes` universe.
 *
 * Scope: only non-legacy (indexed) pages. Legacy pages are self-canonical
 * and can't point at a dead URL by construction.
 *
 * A strict build that fails this check catches:
 *   - redirects.json entries whose targetUrl typo-es or diverges from 7.2
 *   - pages whose slug changed without a matching redirect rule
 *   - broken chains (terminal target doesn't exist)
 */

export interface CanonicalRecord {
    /** Absolute path to the HTML file (relative paths also accepted, stored as-is). */
    file: string;
    /** Canonical URL written into the file's <link rel="canonical"> tag. */
    canonical: string;
    /** Version prefix of the file, or null if not version-prefixed. */
    fileVersion: string | null;
}

export interface VerifierIssue {
    file: string;
    canonical: string;
    canonicalPath: string;
    reason: string;
    fix?: string;
}

export interface VerifyInput {
    records: CanonicalRecord[];
    /** Set of route paths emitted by Docusaurus — the authoritative universe. */
    universe: Set<string>;
    currentVersion: string;
    legacyVersions: readonly string[];
    baseUrl: string;
}

function stripTrailingSlash(p: string): string {
    return p.length > 1 && p.endsWith("/") ? p.slice(0, -1) : p;
}

function normalizeRoute(route: string): string {
    // Routes are paths like "/7.2/foo". Normalize trailing slash for Set lookup.
    return stripTrailingSlash(route);
}

/**
 * Build a universe Set from Docusaurus `routesPaths`, scoped to the current
 * version. Only current-version routes are valid canonical targets for
 * non-legacy pages.
 */
export function buildUniverse(routePaths: readonly string[], currentVersion: string): Set<string> {
    const prefix = `/${currentVersion}`;
    const universe = new Set<string>();
    for (const p of routePaths) {
        if (p === prefix || p.startsWith(`${prefix}/`)) {
            universe.add(normalizeRoute(p));
        }
    }
    return universe;
}

/**
 * Derive the versionless path from a canonical URL, given the current version.
 * `https://host/7.2/foo/bar` → `/foo/bar`. Falls back to the full path when
 * the canonical doesn't start with the expected /<version>/ prefix (that's
 * already a different flavor of failure, signalled by the "does not point at
 * baseUrl" reason).
 */
function deriveVersionlessPath(canonicalPath: string, currentVersion: string): string {
    const prefix = `/${currentVersion}`;
    if (canonicalPath === prefix) {
        return "/";
    }
    if (canonicalPath.startsWith(`${prefix}/`)) {
        return canonicalPath.slice(prefix.length);
    }
    return canonicalPath;
}

/** Build the remediation block printed under each "not in universe" issue. */
function buildUniverseFix(canonicalPath: string, currentVersion: string): string {
    const versionlessPath = deriveVersionlessPath(canonicalPath, currentVersion);
    const snippet = JSON.stringify(
        {
            key: versionlessPath,
            value: {
                targetUrl: "<new-target-path>",
                minimumVersion: currentVersion,
            },
        },
        null,
        2
    );
    return [
        "add an entry to scripts/redirects.json:",
        ...snippet.split("\n").map((l) => `    ${l}`),
        "then run: npm run validate-redirects",
    ].join("\n");
}

export function verifyCanonicals(input: VerifyInput): VerifierIssue[] {
    const { records, universe, currentVersion, legacyVersions, baseUrl } = input;

    const issues: VerifierIssue[] = [];

    for (const rec of records) {
        if (rec.fileVersion && legacyVersions.includes(rec.fileVersion)) {
            continue; // self-canonical; trivially valid
        }

        const { canonical } = rec;
        if (!canonical.startsWith(baseUrl)) {
            issues.push({
                file: rec.file,
                canonical,
                canonicalPath: canonical,
                reason: `canonical does not point at baseUrl (${baseUrl})`,
            });
            continue;
        }

        const canonicalPath = normalizeRoute(canonical.slice(baseUrl.length));

        if (!universe.has(canonicalPath)) {
            issues.push({
                file: rec.file,
                canonical,
                canonicalPath,
                reason: "canonical path is not in the current-version route universe",
                fix: buildUniverseFix(canonicalPath, currentVersion),
            });
        }
    }

    return issues;
}
