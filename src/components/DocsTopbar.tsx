import React, { useState, useEffect } from "react";
import clsx from "clsx";
import LanguageSwitcher from "@site/src/components/LanguageSwitcher";
import { type DocsLanguage } from "./LanguageStore";

type DocsTopbarProps = {
    title: string;
    supportedLanguages?: DocsLanguage[];
};

export default function DocsTopbar({
    title,
    supportedLanguages,
}: DocsTopbarProps) {
    const [isVisible, setIsVisible] = useState(false);
    const [isCollapsed, setIsCollapsed] = useState(false);

    useEffect(() => {
        const handleScroll = () => {
            const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
            const scrollThreshold = 250;
            setIsVisible(scrollTop >= scrollThreshold);
        };

        window.addEventListener("scroll", handleScroll);
        handleScroll();

        return () => {
            window.removeEventListener("scroll", handleScroll);
        };
    }, []);

    useEffect(() => {
        if (window.innerWidth < 768) {
            setIsCollapsed(true);
        }
    }, []);

    if (!supportedLanguages || supportedLanguages.length === 0) {
        return null;
    }

    return (
        <div
            className={clsx(
                "sticky top-[71.46px] z-30",
                "rounded-xl",
                "transition-all duration-300 ease-in-out",
                {
                    "max-h-[60px] opacity-100": isVisible,
                    "max-h-0 opacity-0": !isVisible,
                },
            )}
        >
            <div className="row">
                <div className="col min-[1640px]:!p-0">
                    <div
                        className={clsx(
                            "w-full p-2",
                            "flex justify-between flex-wrap items-center",
                            "rounded-xl",
                            "border border-black/10 dark:border-white/10",
                            "backdrop-blur supports-[backdrop-filter]:bg-white/60 dark:supports-[backdrop-filter]:bg-[#1b1b1d]/40 bg-white/90 dark:bg-[#1b1b1d]/90",
                            "shadow-xl/30",
                            "transition-all duration-300 ease-in-out",
                            {
                                "gap-2": !isCollapsed,
                            },
                        )}
                    >
                        <div className="flex justify-between items-center gap-2 truncate">
                            <div
                                className="text-base font-medium truncate"
                                title={title}
                            >
                                {title}
                            </div>
                            <button
                                className={clsx(
                                    "md:hidden ms-auto",
                                    "p-1 rounded",
                                    "hover:bg-black/5 dark:hover:bg-white/5",
                                    "transition-colors",
                                )}
                                onClick={() => setIsCollapsed(!isCollapsed)}
                                aria-label={
                                    isCollapsed
                                        ? "Show language switcher"
                                        : "Hide language switcher"
                                }
                            >
                                <svg
                                    className={clsx(
                                        "w-4 h-4",
                                        "transition-transform duration-200",
                                        {
                                            "rotate-180": isCollapsed,
                                        },
                                    )}
                                    fill="none"
                                    stroke="currentColor"
                                    viewBox="0 0 24 24"
                                >
                                    <path
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                        strokeWidth={2}
                                        d="M19 9l-7 7-7-7"
                                    />
                                </svg>
                            </button>
                        </div>
                        <div className="hidden md:block">
                            <LanguageSwitcher
                                supportedLanguages={supportedLanguages}
                                flush
                            />
                        </div>
                        <div
                            className={clsx(
                                "md:hidden",
                                "transition-all duration-300 ease-in-out",
                                {
                                    "max-h-0 opacity-0 overflow-hidden":
                                        isCollapsed,
                                    "opacity-100": !isCollapsed,
                                },
                            )}
                        >
                            <LanguageSwitcher
                                supportedLanguages={supportedLanguages}
                                flush
                            />
                        </div>
                    </div>
                </div>
                <div className="col col--3 lg:block"></div>
            </div>
        </div>
    );
}
