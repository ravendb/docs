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
import { compareVersions } from "./compare-versions.js";

export interface RedirectRule {
    key: string;
    value: {
        targetUrl: string;
        /** Lowest version at which the rule applies. Required on versioned keys. */
        minimumVersion?: string;
    };
}

export type RedirectMap = Map<string, { targetUrl: string; minimumVersion?: string }>;

/** Key prefixes whose content area has no version axis. */
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

/** Detect redirect cycles. One error per distinct cycle. */
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

/** Map a targetUrl to its filesystem content root: /guides, /cloud, or /docs (default). */
function resolveContentPath(targetUrl: string, projectRoot: string): { root: string; rel: string[] } {
    const segments = targetUrl.split("/").filter(Boolean);
    const first = segments[0];
    if (first === "guides" || first === "cloud") {
        return { root: path.join(projectRoot, first), rel: segments.slice(1) };
    }
    return { root: path.join(projectRoot, "docs"), rel: segments };
}

/** Verify every targetUrl maps to a real .mdx/.md file (or an index.* under a directory). */
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

/** Follow redirect hops from startPath, stopping on a failed minimumVersion gate. */
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
