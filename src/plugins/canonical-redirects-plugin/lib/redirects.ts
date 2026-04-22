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
// Plugin-local copy of compareVersions. The edge handler
// (scripts/handle_redirects.js) inlines its own copy because CloudFront
// Functions can't resolve imports at runtime; a parity test in __tests__/
// guards against drift between the two.
import { compareVersions } from "./compare-versions.js";

export interface RedirectRule {
    key: string;
    value: {
        targetUrl: string;
        minimumVersion?: string;
    };
}

export type RedirectMap = Map<string, { targetUrl: string; minimumVersion?: string }>;

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

        if (value.minimumVersion !== undefined) {
            if (typeof value.minimumVersion !== "string" || !VERSION_REGEX.test(value.minimumVersion)) {
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
 * Transitively follow redirect hops, gating each hop by its minimumVersion
 * against the resolving version.
 *
 * No cycle guard: `validateNoCycles` runs upstream (CLI + plugin
 * `loadContent`) and makes cycles impossible by the time the rewriter
 * calls here. Adding a runtime `visited` set would re-check an invariant
 * that can't break and mask any real bug with a silent early return.
 *
 * Today's redirects.json has zero chains (all 30 targets are terminal) — the
 * implementation handles multi-hop so future authoring doesn't silently break.
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
