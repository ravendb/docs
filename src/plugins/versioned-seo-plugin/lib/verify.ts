/**
 * Build-time SEO verifiers for versioned pages.
 *
 * Two assertions, both run during the postBuild walk:
 *
 * 1. Canonical verifier — for every non-legacy (indexed) page, confirm the
 *    emitted canonical URL resolves to a page in the final site (i.e. its
 *    path is in the Docusaurus `routes` universe). Legacy pages are
 *    self-canonical and can't point at a dead URL by construction, so
 *    they're skipped here.
 *
 * 2. Legacy noindex assertion — for every legacy-version page, confirm
 *    Docusaurus injected `<meta name="robots" content="noindex,...">`.
 *    The injection is delegated to Docusaurus' native per-version
 *    `noIndex` config; this assertion is the build-time guarantee that
 *    a typo, a forgotten LEGACY_VERSIONS entry, or a swizzled
 *    `DocVersionRoot` that drops the meta tag fails the build instead
 *    of silently leaking legacy pages into search engines.
 *
 * A strict build that fails the canonical check catches:
 *   - redirects.json entries whose targetUrl typo-es or diverges from current
 *   - pages whose slug changed without a matching redirect rule
 *   - broken chains (terminal target doesn't exist)
 *
 * A strict build that fails the noindex check catches:
 *   - a legacy version missing from `versions[v].noIndex: true` in docusaurus.config.ts
 *   - a swizzled `DocVersionRoot` that omits the meta tag
 */

/**
 * Matches `<meta name="robots" content="...noindex...">` regardless of attribute
 * order, quoting style (double, single, unquoted), or whitespace. The content
 * attribute must contain `noindex` somewhere; we don't care whether it pairs
 * with `follow` or `nofollow`. Docusaurus emits `noindex, nofollow`; older
 * hand-rolled tags used `noindex,follow`. Both pass.
 */
const ROBOTS_NOINDEX_REGEX =
    /<meta\b[^>]*\bname\s*=\s*(?:"robots"|'robots'|robots(?=[\s/>]))[^>]*\bcontent\s*=\s*(?:"[^"]*\bnoindex\b[^"]*"|'[^']*\bnoindex\b[^']*'|[^\s"'=<>`]*\bnoindex\b[^\s"'=<>`]*)[^>]*>/i;

/**
 * Pure predicate: does this HTML carry a `<meta name="robots">` tag whose
 * content includes `noindex`? Tag order in `<head>` doesn't matter — we scan
 * the whole document. Used by the postBuild assertion that every legacy
 * page got Docusaurus' native noindex injection.
 */
export function hasNoindexRobotsMeta(html: string): boolean {
    return ROBOTS_NOINDEX_REGEX.test(html);
}

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

// Map<scope, Set<route>>. Scope keys: currently-maintained version strings, "cloud", "guides", "samples".
// Legacy versions, /templates, and root are excluded — invalid see_also targets, no slice needed.
const VERSION_PREFIX_REGEX = /^\/(\d+\.\d+)(\/|$)/;
const SECTION_PREFIX_REGEX = /^\/(cloud|guides|samples)(\/|$)/;

export function buildScopedRoutes(
    routePaths: readonly string[],
    legacyVersions: readonly string[]
): Map<string, Set<string>> {
    const legacy = new Set(legacyVersions);
    const routesByScope = new Map<string, Set<string>>();
    const addRoute = (scope: string, route: string) => {
        let set = routesByScope.get(scope);
        if (!set) {
            set = new Set();
            routesByScope.set(scope, set);
        }
        set.add(route);
    };
    for (const routePath of routePaths) {
        const normalisedPath = normalizeRoute(routePath);
        const versionMatch = routePath.match(VERSION_PREFIX_REGEX);
        if (versionMatch) {
            if (!legacy.has(versionMatch[1])) {
                addRoute(versionMatch[1], normalisedPath);
            }
            continue;
        }
        const sectionMatch = routePath.match(SECTION_PREFIX_REGEX);
        if (sectionMatch) {
            addRoute(sectionMatch[1], normalisedPath);
        }
    }
    return routesByScope;
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
