import React, { useEffect } from "react";
import { useLanguage } from "./LanguageContext";
import clsx from "clsx";

const languages = [
  { label: "C#", value: "csharp", brand: "#9179E4" },
  { label: "Java", value: "java", brand: "#f89820" },
  { label: "Python", value: "python", brand: "#fbcb24" },
  { label: "PHP", value: "php", brand: "#8993be" },
  { label: "Node.JS", value: "nodejs", brand: "#3c873a" },
];

type LanguageSwitcherProps = {
  supportedLanguages: string[];
};

export default function LanguageSwitcher({
  supportedLanguages,
}: LanguageSwitcherProps) {
  const { language, setLanguage } = useLanguage();

  useEffect(() => {
      if (!supportedLanguages.includes(language)) {
          setLanguage(supportedLanguages[0]);
      }
  }, [supportedLanguages, language, setLanguage]);

  return (
    <div className="flex flex-wrap gap-2 mb-8">
      {languages
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
                "border-gray-300 text-gray-500 hover:bg-black/5 hover:border-gray-500 hover:text-gray-600",
                "dark:text-gray-300 dark:border-gray-600 dark:hover:text-gray-200 dark:hover:border-gray-400 dark:hover:bg-white/5",
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
