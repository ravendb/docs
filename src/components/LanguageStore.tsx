import { useSyncExternalStore, useCallback } from "react";
import { useHistory } from "@docusaurus/router";
import { languageConfig } from "./languageConfig";

export type DocsLanguage = "csharp" | "java" | "python" | "php" | "nodejs";

export const DEFAULT_LANGUAGE: DocsLanguage = "csharp";
export const LANGUAGE_QUERY_PARAM = "lang";

const LANGUAGE_STORAGE_KEY = "docs-language";

// Same-tab only (not the native cross-tab "storage" event) so two tabs can show different languages side-by-side.
const LANGUAGE_CHANGE_EVENT = "docs-language-change";

export const isDocsLanguage = (value: string | null | undefined): value is DocsLanguage =>
    !!value && languageConfig.some((lang) => lang.value === value);

export const getLanguageFromSearch = (search: string): DocsLanguage | null => {
    const value = new URLSearchParams(search).get(LANGUAGE_QUERY_PARAM);
    return isDocsLanguage(value) ? value : null;
};

export const getStoredLanguage = (): DocsLanguage => {
    const value = window.localStorage.getItem(LANGUAGE_STORAGE_KEY);
    return isDocsLanguage(value) ? value : DEFAULT_LANGUAGE;
};

// URL wins (per-tab), then the stored cross-session default. Server snapshot is the default so the
// first client render matches the query-less prerendered HTML; the URL/stored value applies after hydration.
const getSnapshot = (): DocsLanguage => getLanguageFromSearch(window.location.search) ?? getStoredLanguage();
const getServerSnapshot = (): DocsLanguage => DEFAULT_LANGUAGE;

const subscribe = (callback: () => void): (() => void) => {
    window.addEventListener(LANGUAGE_CHANGE_EVENT, callback);
    window.addEventListener("popstate", callback);
    return () => {
        window.removeEventListener(LANGUAGE_CHANGE_EVENT, callback);
        window.removeEventListener("popstate", callback);
    };
};

export const useLanguage = (): {
    language: DocsLanguage;
    setLanguage: (newLanguage: DocsLanguage) => void;
} => {
    const history = useHistory();
    const language = useSyncExternalStore(subscribe, getSnapshot, getServerSnapshot);

    const setLanguage = useCallback(
        (newLanguage: DocsLanguage) => {
            const { pathname, search, hash } = history.location;
            const params = new URLSearchParams(search);
            // Keep the default language out of the URL; stamp everything else.
            if (newLanguage === DEFAULT_LANGUAGE) {
                params.delete(LANGUAGE_QUERY_PARAM);
            } else {
                params.set(LANGUAGE_QUERY_PARAM, newLanguage);
            }

            const newSearch = params.toString();
            window.localStorage.setItem(LANGUAGE_STORAGE_KEY, newLanguage);
            history.replace({ pathname, search: newSearch ? `?${newSearch}` : "", hash });
            // Notify after storage and URL are both updated so getSnapshot re-reads fresh values.
            window.dispatchEvent(new window.Event(LANGUAGE_CHANGE_EVENT));
        },
        [history]
    );

    return { language, setLanguage };
};
