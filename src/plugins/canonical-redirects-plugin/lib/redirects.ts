/**
 * Redirect rule loading, schema validation, map-building and chain resolution.
 *
 * The JSON file at scripts/redirects.json is consumed by two separate systems:
 *   - at the edge:   scripts/handle_redirects.js (CloudFront Function)
 *   - at build time: this plugin (rewrites canonicals in emitted HTML)
 *
 * Both read the same shape. This module is the build-time authority.
 */

import fs from "fs";
import path from "path";
// Plugin-local copy of compareVersions. The edge handler
// (scripts/handle_redirects.js) inlines its own copy because CloudFront
// Functions can't resolve imports at runtime; a parity test in __tests__/
// guards against drift between the two.
import { compareVersions } from "./compare-versions.js";

export interface RedirectRule {
    key: string;
    value: {
        targetUrl: string;
        /**
         * Lowest version at which the rule applies. A reader on an older
         * version hitting the rule's key stops there instead of hopping to
         * a page that didn't exist yet on their version.
         *
         * Required on versioned (docs) keys; forbidden on versionless
         * keys — `/guides/…` and `/cloud/…` — since those areas have
         * no version axis at all (the edge handler's /guides and /cloud
         * branches redirect without consulting minimumVersion).
         * See validateRedirects for the enforced rule.
         */
        minimumVersion?: string;
    };
}

export type RedirectMap = Map<string, { targetUrl: string; minimumVersion?: string }>;

/**
 * A redirect's `key` lives in a versionless content area when it starts
 * with one of these prefixes. Used by validateRedirects to decide whether
 * minimumVersion is required (versioned docs keys) or forbidden
 * (versionless keys).
 *
 * `/templates` is deliberately excluded: it's an internal authoring area
 * with no public routes, so redirects under it aren't expected and
 * shouldn't need special-casing.
 */
const VERSIONLESS_KEY_PREFIXES = ["/guides/", "/cloud/"] as const;

function isVersionlessKey(key: string): boolean {
    return VERSIONLESS_KEY_PREFIXES.some((p) => key.startsWith(p));
}

export interface ValidationError {
    index: number;
    key: string | null;
    message: string;
}

const PATH_REGEX = /^\/[\w\-./]*$/;
const VERSION_REGEX = /^\d+\.\d+$/;

/** Load and JSON-parse redirects.json; throws on IO / parse failure. */
export function loadRedirects(filePath: string): RedirectRule[] {
    const raw = fs.readFileSync(filePath, "utf8");
    const parsed: unknown = JSON.parse(raw);
    if (!Array.isArray(parsed)) {
        throw new Error(`redirects file must contain a JSON array, got ${typeof parsed}`);
    }
    return parsed as RedirectRule[];
}

/**
 * Structural validation. Returns a list of human-readable errors — empty list
 * means the file is well-formed. Never throws; callers decide whether to fail
 * the build.
 */
export function validateRedirects(rules: unknown): ValidationError[] {
    const errors: ValidationError[] = [];

    if (!Array.isArray(rules)) {
        return [{ index: -1, key: null, message: "redirects must be a JSON array" }];
    }

    const seenKeys = new Set<string>();

    rules.forEach((rule, index) => {
        const r = rule as Partial<RedirectRule> & Record<string, unknown>;

        if (!r || typeof r !== "object") {
            errors.push({ index, key: null, message: "entry is not an object" });
            return;
        }

        const keyCandidate = r.key;
        if (typeof keyCandidate !== "string") {
            errors.push({ index, key: null, message: "'key' must be a string" });
            return;
        }
        const key = keyCandidate;

        if (!PATH_REGEX.test(key)) {
            errors.push({ index, key, message: `'key' must be an absolute path (got ${JSON.stringify(key)})` });
        }

        if (seenKeys.has(key)) {
            errors.push({ index, key, message: "duplicate 'key'" });
        }
        seenKeys.add(key);

        const value = r.value as RedirectRule["value"] | undefined;
        if (!value || typeof value !== "object") {
            errors.push({ index, key, message: "'value' must be an object" });
            return;
        }

        if (typeof value.targetUrl !== "string" || !PATH_REGEX.test(value.targetUrl)) {
            errors.push({
                index,
                key,
                message: `'value.targetUrl' must be an absolute path (got ${JSON.stringify(value.targetUrl)})`,
            });
        }

        // minimumVersion is required on versioned docs keys (version
        // gating is load-bearing for chain resolution on old versions).
        // Versionless keys (/guides, /cloud) are exempt — they have no
        // version axis, so the edge's /guides and /cloud branches redirect
        // without consulting minimumVersion. A stray minimumVersion on a
        // versionless entry isn't flagged here; PR review will catch it.
        if (!isVersionlessKey(key)) {
            if (value.minimumVersion === undefined) {
                errors.push({
                    index,
                    key,
                    message: `'value.minimumVersion' is required on versioned keys and must be a "MAJOR.MINOR" string`,
                });
            } else if (typeof value.minimumVersion !== "string" || !VERSION_REGEX.test(value.minimumVersion)) {
                errors.push({
                    index,
                    key,
                    message: `'value.minimumVersion' must be a "MAJOR.MINOR" string (got ${JSON.stringify(value.minimumVersion)})`,
                });
            }
        }
    });

    return errors;
}

export function buildRedirectMap(rules: RedirectRule[]): RedirectMap {
    const map: RedirectMap = new Map();
    for (const rule of rules) {
        map.set(rule.key, rule.value);
    }
    return map;
}

/**
 * Detect redirect cycles in a validated rule set. Runs after structural
 * validation passes and returns the same `ValidationError[]` shape so the
 * CLI and plugin renderers can print both kinds uniformly.
 *
 * Why this lives in validation and not just at runtime: the edge handler
 * (`scripts/handle_redirects.js`) runs on every request with a 1ms budget
 * and can't afford a runtime `visited` set. Enforcing "no cycles" as a
 * build-time invariant via `npm run validate-redirects` lets the edge
 * trust the KVS data and keep its chain loop minimal.
 *
 * Algorithm: walk each key's chain, tracking the path. If we revisit a key
 * in the same walk, slice out the cycle portion and emit one error per
 * distinct cycle (dedup via an `explored` set so starting from any key
 * inside a cycle doesn't produce N duplicate reports).
 */
export function validateNoCycles(rules: RedirectRule[]): ValidationError[] {
    const errors: ValidationError[] = [];
    const map = buildRedirectMap(rules);
    const explored = new Set<string>();

    for (const rule of rules) {
        if (explored.has(rule.key)) continue;

        const path: string[] = [];
        const visited = new Set<string>();
        let current: string = rule.key;

        while (map.has(current)) {
            if (visited.has(current)) {
                const cycleStart = path.indexOf(current);
                const cycleKeys = path.slice(cycleStart);
                const cycle = [...cycleKeys, current].join(" → ");
                errors.push({
                    index: rules.findIndex((r) => r.key === cycleKeys[0]),
                    key: cycleKeys[0],
                    message: `redirect cycle detected: ${cycle}`,
                });
                for (const k of path) explored.add(k);
                break;
            }
            visited.add(current);
            path.push(current);
            current = map.get(current)!.targetUrl;
        }

        // Chain terminated cleanly — mark every key in the walk as
        // explored so a later starting key won't re-trace the same
        // tail.
        if (!map.has(current)) {
            for (const k of path) explored.add(k);
        }
    }

    return errors;
}

/**
 * Map a versionless redirect targetUrl to the filesystem content root that
 * would serve it. Kept in sync with docusaurus.config.ts:
 *   - /guides/…    → <projectRoot>/guides/
 *   - /cloud/…     → <projectRoot>/cloud/
 *   - anything else → <projectRoot>/docs/ (the current-version content tree)
 *
 * `/templates` is excluded — it's an internal authoring area, not routed
 * publicly, and shouldn't appear as a redirect target.
 */
function resolveContentPath(targetUrl: string, projectRoot: string): { root: string; rel: string[] } {
    const segments = targetUrl.split("/").filter(Boolean);
    const first = segments[0];
    if (first === "guides" || first === "cloud") {
        return { root: path.join(projectRoot, first), rel: segments.slice(1) };
    }
    return { root: path.join(projectRoot, "docs"), rel: segments };
}

/**
 * Verify that every redirect's `targetUrl` maps to a real .mdx/.md file in
 * the content tree. Runs after structural + cycle validation and returns
 * the same `ValidationError[]` shape so callers can print all three kinds
 * uniformly.
 *
 * Why this lives in validation: previously a broken target only surfaced
 * at `postBuild` via the canonical verifier — and only for pages that
 * actually emitted a canonical resolving through the dead rule. Direct
 * filesystem resolution at `validate-redirects` time gives authors the
 * feedback the moment they edit redirects.json, before any build work.
 *
 * A target matches if any of these exist:
 *   - <root>/<rel>.mdx
 *   - <root>/<rel>.md
 *   - <root>/<rel>/index.mdx
 *   - <root>/<rel>/index.md
 * Pure directory-with-_category_.json entries aren't accepted — redirect
 * targets should always resolve to a concrete page.
 */
export function validateTargetsExist(rules: RedirectRule[], projectRoot: string): ValidationError[] {
    const errors: ValidationError[] = [];
    for (let index = 0; index < rules.length; index++) {
        const rule = rules[index];
        const target = rule.value.targetUrl;
        const { root, rel } = resolveContentPath(target, projectRoot);
        const basePath = path.join(root, ...rel);
        const candidates = [
            `${basePath}.mdx`,
            `${basePath}.md`,
            path.join(basePath, "index.mdx"),
            path.join(basePath, "index.md"),
        ];
        if (!candidates.some((p) => fs.existsSync(p))) {
            errors.push({
                index,
                key: rule.key,
                message: `targetUrl does not resolve to an existing document: ${target}`,
            });
        }
    }
    return errors;
}

/**
 * Transitively follow redirect hops, gating each hop by its minimumVersion
 * against the resolving version.
 *
 * No cycle guard: `validateNoCycles` runs upstream (CLI + plugin
 * `loadContent`) and makes cycles impossible by the time the rewriter
 * calls here. Adding a runtime `visited` set would re-check an invariant
 * that can't break and mask any real bug with a silent early return.
 *
 * Today's redirects.json has zero chains — all targets are terminal. The
 * implementation handles multi-hop so future authoring doesn't silently
 * break.
 *
 * minimumVersion handling: the rewriter only calls resolveChain with
 * versioned-docs paths, and validateRedirects requires a minimumVersion
 * on every docs-area rule — so in practice `rule.minimumVersion` is
 * always a string here. The `rule.minimumVersion &&` guard stays as a
 * cheap belt-and-braces for the versionless-key shape: if a /guides or
 * /cloud rule ever reaches this function, "absent minimumVersion" is
 * treated as "always apply", matching the schema intent.
 *
 * @param map     Built from redirects.json
 * @param startPath  Versionless path starting with '/'
 * @param version The caller's effective version for minimumVersion gating
 * @returns Terminal versionless path (startPath if no hop applied)
 */
export function resolveChain(map: RedirectMap, startPath: string, version: string): string {
    let current = startPath;

    while (map.has(current)) {
        const rule = map.get(current)!;
        if (rule.minimumVersion && compareVersions(version, rule.minimumVersion) < 0) {
            break;
        }
        current = rule.targetUrl;
    }

    return current;
}
