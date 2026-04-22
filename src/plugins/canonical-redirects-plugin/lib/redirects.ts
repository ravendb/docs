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
// compare-versions.js is a CJS module (module.exports = { compareVersions }).
// Node ESM supports named-import interop for CJS files, which works for both
// the tsx --test runner and the tsImport loader used by validate-redirects.js.
import { compareVersions } from "../../../../scripts/lib/compare-versions.js";

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
 * Transitively follow redirect hops, gating each hop by its minimumVersion
 * against the resolving version. Detects cycles and throws a loud error.
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
    const visited = new Set<string>();
    let current = startPath;

    while (map.has(current)) {
        const rule = map.get(current)!;
        if (rule.minimumVersion && compareVersions(version, rule.minimumVersion) < 0) {
            break;
        }
        if (visited.has(current)) {
            const cycle = [...visited, current].join(" → ");
            throw new Error(`Redirect cycle detected: ${cycle}`);
        }
        visited.add(current);
        current = rule.targetUrl;
    }

    return current;
}
