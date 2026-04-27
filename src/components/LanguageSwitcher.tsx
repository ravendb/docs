import React, { useEffect } from "react";
import { useLanguage, type DocsLanguage } from "./LanguageStore";
import { languageConfig } from "./languageConfig";
import clsx from "clsx";

type LanguageSwitcherProps = {
    supportedLanguages: DocsLanguage[];
    flush?: boolean;
};

export default function LanguageSwitcher({ supportedLanguages, flush = false }: LanguageSwitcherProps) {
    const { language, setLanguage } = useLanguage();

    const isCurrentLanguageSupported = supportedLanguages.includes(language);
    const firstSupportedLanguage = supportedLanguages[0];

    useEffect(() => {
        if (!isCurrentLanguageSupported) {
            setLanguage(firstSupportedLanguage);
        }
    }, [isCurrentLanguageSupported, firstSupportedLanguage, setLanguage]);

    return (
        <div className={clsx("flex flex-wrap gap-2", { "mb-8": !flush })}>
            {languageConfig
                .filter((lang) => supportedLanguages.includes(lang.value))
                .map((lang) => {
                    const isActive = language === lang.value;

                    return (
                        <button
                            key={lang.value}
                            type="button"
                            onClick={() => setLanguage(lang.value)}
                            className={clsx(
                                "px-3 py-1.5 rounded-md border text-sm transition-colors cursor-pointer",
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
                        </button>
                    );
                })}
        </div>
    );
}
