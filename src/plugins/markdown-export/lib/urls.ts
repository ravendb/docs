// handle_redirects.js inlines the same scheme; the edge test keeps them in sync.

export const DOCS_LANGUAGES = ["csharp", "java", "python", "php", "nodejs"] as const;
export type DocsLanguage = (typeof DOCS_LANGUAGES)[number];

export const LANGUAGE_LABELS: Record<string, string> = {
    csharp: "C#",
    java: "Java",
    python: "Python",
    php: "PHP",
    nodejs: "Node.js",
};

export const DEFAULT_LANGUAGE = "csharp";

export function stripTrailingSlash(pathname: string): string {
    return pathname.length > 1 && pathname.endsWith("/") ? pathname.slice(0, -1) : pathname;
}

export function markdownPath(route: string, language?: string | null): string {
    return `${stripTrailingSlash(route)}${language ? `.${language}` : ""}.md`;
}

/** As LanguageStore: the default language when offered, else the first declared. */
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
