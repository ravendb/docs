// URL scheme of the Markdown copies. The same rules are inlined in scripts/handle_redirects.js,
// which cannot import project files; markdown-export's edge tests hold the two together.

export const DOCS_LANGUAGES = ["csharp", "java", "python", "php", "nodejs"] as const;
export type DocsLanguage = (typeof DOCS_LANGUAGES)[number];

export const LANGUAGE_LABELS: Record<string, string> = {
    csharp: "C#",
    java: "Java",
    python: "Python",
    php: "PHP",
    nodejs: "Node.js",
};

/** Matches what the language switcher shows when nothing is selected (see LanguageStore). */
export const DEFAULT_LANGUAGE = "csharp";

export function stripTrailingSlash(pathname: string): string {
    return pathname.length > 1 && pathname.endsWith("/") ? pathname.slice(0, -1) : pathname;
}

/**
 * /7.2/client-api/session/loading-entities -> /7.2/client-api/session/loading-entities.md, or
 * .python.md for a non-default language. A route with a trailing slash (a version or section
 * root) drops it first, the same way the edge normalizes the request.
 */
export function markdownPath(route: string, language?: string | null): string {
    return `${stripTrailingSlash(route)}${language ? `.${language}` : ""}.md`;
}

/**
 * The language a page shows before anyone picks one: the switcher default when the page offers
 * it, else the first language it declares (LanguageStore clamps to supported_languages[0]).
 */
export function defaultLanguageOf(declared: readonly string[], rendered: readonly string[]): string | null {
    if (rendered.length === 0) {
        return null;
    }
    const offered = declared.length > 0 ? declared : rendered;
    if (offered.includes(DEFAULT_LANGUAGE)) {
        return DEFAULT_LANGUAGE;
    }
    return offered.find((l) => rendered.includes(l)) ?? rendered[0];
}
