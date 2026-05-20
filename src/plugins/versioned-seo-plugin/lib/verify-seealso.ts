// Validates SeeAlso link records against the per-scope route universe.
// Records are produced from emitted HTML by extractSeeAlsoAnchors; the
// verifier itself is pure. `source: external` records are skipped.

export interface SeeAlsoRecord {
    articlePath: string;
    href: string;
    source: string;
    articleVersion: string | null;
}

export interface SeeAlsoIssue {
    articlePath: string;
    href: string;
    reason: string;
    fix: string;
}

export interface VerifySeeAlsoInput {
    records: readonly SeeAlsoRecord[];
    routesByScope: ReadonlyMap<string, ReadonlySet<string>>;
    latestVersion: string;
}

const SEEALSO_ANCHOR_REGEX = /<a\b[^>]*\bdata-seealso\b[^>]*>/gi;
const HREF_ATTR_REGEX = /\bhref\s*=\s*(?:"([^"]*)"|'([^']*)'|([^\s"'=<>`]+))/i;
const SEEALSO_VALUE_REGEX = /\bdata-seealso\s*=\s*(?:"([\w-]+)"|'([\w-]+)'|([\w-]+))/i;

/** Fans one rendered HTML doc out into one validation record per <a data-seealso> anchor. */
export function fanoutSeeAlsoRecords(
    html: string,
    articlePath: string,
    articleVersion: string | null
): SeeAlsoRecord[] {
    const records: SeeAlsoRecord[] = [];
    const anchors = html.match(SEEALSO_ANCHOR_REGEX);
    if (!anchors) {
        return records;
    }
    for (const anchor of anchors) {
        const valueMatch = anchor.match(SEEALSO_VALUE_REGEX);
        const source = valueMatch?.[1] ?? valueMatch?.[2] ?? valueMatch?.[3];
        if (!source) {
            continue;
        }
        const hrefMatch = anchor.match(HREF_ATTR_REGEX);
        const raw = hrefMatch?.[1] ?? hrefMatch?.[2] ?? hrefMatch?.[3];
        if (!raw) {
            continue;
        }
        const hashIdx = raw.indexOf("#");
        const href = hashIdx >= 0 ? raw.slice(0, hashIdx) : raw;
        records.push({ articlePath, href, source, articleVersion });
    }
    return records;
}

export function verifySeeAlsoLinks(input: VerifySeeAlsoInput): SeeAlsoIssue[] {
    const { records, routesByScope, latestVersion } = input;
    const issues: SeeAlsoIssue[] = [];

    for (const record of records) {
        if (record.source === "external") {
            continue;
        }

        const expectedVersion = record.articleVersion ?? latestVersion;
        const scopeKey = record.source === "cloud" ? "cloud" : record.source === "guides" ? "guides" : expectedVersion;

        if (routesByScope.get(scopeKey)?.has(record.href)) {
            continue;
        }

        issues.push({
            articlePath: record.articlePath,
            href: record.href,
            reason: `see_also href ${record.href} does not resolve in scope "${scopeKey}"`,
            fix: `edit ${record.articlePath}: link: must be versionless and point at a real page in ${scopeKey === "cloud" || scopeKey === "guides" ? `/${scopeKey}/` : `version ${scopeKey}`}.`,
        });
    }

    return issues;
}
