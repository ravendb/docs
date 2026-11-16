import { ACTIVE_VERSIONS, CURRENT_VERSION } from "../../../../scripts/lib/version-policy.js";

export { CURRENT_VERSION };

export const MARKDOWN_VERSIONS: readonly string[] = [CURRENT_VERSION, ...ACTIVE_VERSIONS];

export const MARKDOWN_SECTIONS: readonly string[] = ["cloud", "quill", "guides", "samples"];

export const UNVERSIONED_ROOTS: ReadonlySet<string> = new Set([...MARKDOWN_SECTIONS, "templates"]);

/** handle_redirects.js inlines the same lists. */
export function inMarkdownScope(route: string): boolean {
    const first = route.split("/")[1] ?? "";
    return MARKDOWN_VERSIONS.includes(first) || MARKDOWN_SECTIONS.includes(first);
}

export function versionOfRoute(route: string): string | null {
    const first = route.split("/")[1] ?? "";
    return MARKDOWN_VERSIONS.includes(first) ? first : null;
}
