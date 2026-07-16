import React, { useEffect } from "react";
import { useLocation } from "@docusaurus/router";
import { useLanguage, LANGUAGE_QUERY_PARAM, type DocsLanguage } from "./LanguageStore";
import { languageConfig } from "./languageConfig";
import clsx from "clsx";

type LanguageSwitcherProps = {
    supportedLanguages: DocsLanguage[];
    flush?: boolean;
};

export default function LanguageSwitcher({ supportedLanguages, flush = false }: LanguageSwitcherProps) {
    const { language, setLanguage } = useLanguage();
    const location = useLocation();

    const isCurrentLanguageSupported = supportedLanguages.includes(language);
    const firstSupportedLanguage = supportedLanguages[0];

    useEffect(() => {
        if (!isCurrentLanguageSupported) {
            setLanguage(firstSupportedLanguage);
        }
    }, [isCurrentLanguageSupported, firstSupportedLanguage, setLanguage]);

    // Explicit "?lang=" (incl. the default) so Ctrl/Cmd/middle-click opens that language in a new tab (RDoc-3454).
    const buildHref = (value: DocsLanguage): string => {
        const params = new URLSearchParams(location.search);
        params.set(LANGUAGE_QUERY_PARAM, value);
        return `${location.pathname}?${params.toString()}${location.hash}`;
    };

    return (
        <div className={clsx("flex flex-wrap gap-2", { "mb-8": !flush })}>
            {languageConfig
                .filter((lang) => supportedLanguages.includes(lang.value))
                .map((lang) => {
                    const isActive = language === lang.value;

                    return (
                        <a
                            key={lang.value}
                            href={buildHref(lang.value)}
                            aria-current={isActive ? "true" : undefined}
                            onClick={(e) => {
                                // Let modified clicks through (new tab/window); intercept only a plain left click.
                                if (
                                    e.defaultPrevented ||
                                    e.button !== 0 ||
                                    e.metaKey ||
                                    e.ctrlKey ||
                                    e.shiftKey ||
                                    e.altKey
                                ) {
                                    return;
                                }
                                e.preventDefault();
                                setLanguage(lang.value);
                            }}
                            className={clsx(
                                "px-3 py-1.5 rounded-md border text-sm transition-colors",
                                "no-underline hover:no-underline",
                                "border-black/10 text-gray-500 hover:bg-black/5 hover:border-black/15 hover:text-gray-600",
                                "dark:text-gray-300 dark:border-white/10 dark:hover:text-gray-200 dark:hover:border-white/15 dark:hover:bg-white/5"
                            )}
                            style={
                                isActive
                                    ? {
                                          backgroundColor: `${lang.brand}20`,
                                          color: lang.brand,
                                          borderColor: lang.brand,
                                      }
                                    : {}
                            }
                        >
                            {lang.label}
                        </a>
                    );
                })}
        </div>
    );
}
