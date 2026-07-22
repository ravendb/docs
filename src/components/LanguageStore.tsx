import { useSyncExternalStore, useCallback } from "react";
import { useHistory } from "@docusaurus/router";
import { languageConfig } from "./languageConfig";

export type DocsLanguage = "csharp" | "java" | "python" | "php" | "nodejs";

export const DEFAULT_LANGUAGE: DocsLanguage = "csharp";
export const LANGUAGE_QUERY_PARAM = "lang";

const LANGUAGE_STORAGE_KEY = "docs-language";

// Same-tab event (not the cross-tab "storage" event) so two tabs can hold different languages.
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

// URL (per-tab) wins over the stored default; server snapshot is the default to avoid a hydration mismatch.
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

// Store-only (no router) so consumers don't re-render on unrelated location changes.
export const useLanguage = (): DocsLanguage => useSyncExternalStore(subscribe, getSnapshot, getServerSnapshot);

export const useSetLanguage = (): ((newLanguage: DocsLanguage) => void) => {
    const history = useHistory();

    return useCallback(
        (newLanguage: DocsLanguage) => {
            const { pathname, search, hash } = history.location;
            const params = new URLSearchParams(search);
            if (newLanguage === DEFAULT_LANGUAGE) {
                params.delete(LANGUAGE_QUERY_PARAM);
            } else {
                params.set(LANGUAGE_QUERY_PARAM, newLanguage);
            }

            const newSearch = params.toString();
            window.localStorage.setItem(LANGUAGE_STORAGE_KEY, newLanguage);
            history.replace({ pathname, search: newSearch ? `?${newSearch}` : "", hash });
            // Dispatch after storage + URL are updated so getSnapshot re-reads fresh values.
            window.dispatchEvent(new window.Event(LANGUAGE_CHANGE_EVENT));
        },
        [history]
    );
};
