import { useSyncExternalStore, useCallback } from "react";

export type DocsLanguage = "csharp" | "java" | "python" | "php" | "nodejs";

const DEFAULT_LANGUAGE: DocsLanguage = "csharp";
const LANGUAGE_STORAGE_KEY = "docs-language";

const getLanguageFromLocalStorage = (): DocsLanguage => {
    return (window.localStorage.getItem(LANGUAGE_STORAGE_KEY) as DocsLanguage) || DEFAULT_LANGUAGE;
};

const subscribe = (callback: () => void): (() => void) => {
    window.addEventListener("storage", callback);
    return () => {
        window.removeEventListener("storage", callback);
    };
};

export const useLanguage = (): {
    language: DocsLanguage;
    setLanguage: (newLanguage: DocsLanguage) => void;
} => {
    const language = useSyncExternalStore(subscribe, getLanguageFromLocalStorage, () => DEFAULT_LANGUAGE);

    const setLanguage = useCallback((newLanguage: DocsLanguage) => {
        window.localStorage.setItem(LANGUAGE_STORAGE_KEY, newLanguage);
        window.dispatchEvent(new window.Event("storage"));
    }, []);

    return { language, setLanguage };
};
