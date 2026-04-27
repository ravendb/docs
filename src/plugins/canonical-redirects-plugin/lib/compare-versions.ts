/**
 * Semantic-like version comparator for "MAJOR.MINOR" strings.
 *
 * This is a deliberate duplicate of the function defined inline in
 * scripts/handle_redirects.js. The edge handler (a CloudFront Function) can't
 * import additional modules at runtime, so it inlines its own copy. A parity
 * test (see __tests__/compare-versions-parity.test.ts) reads the edge file as
 * text, extracts its compareVersions body, and asserts behavioural equality
 * with the implementation below — catching drift between the two.
 *
 * @returns  1  when v1 > v2
 *          -1  when v1 < v2
 *           0  when v1 === v2
 */
export function compareVersions(v1: string, v2: string): number {
    const parts1 = v1.split(".");
    const parts2 = v2.split(".");
    const major1 = parseInt(parts1[0], 10);
    const major2 = parseInt(parts2[0], 10);
    const minor1 = parseInt(parts1[1], 10);
    const minor2 = parseInt(parts2[1], 10);
    if (major1 === major2) {
        if (minor1 > minor2) {
            return 1;
        }
        if (minor1 < minor2) {
            return -1;
        }
        return 0;
    }
    if (major1 > major2) {
        return 1;
    }
    if (major1 < major2) {
        return -1;
    }
    return 0;
}
