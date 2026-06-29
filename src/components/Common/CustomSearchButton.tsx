import React, { forwardRef, useEffect, useState } from "react";
import clsx from "clsx";
import { Icon } from "./Icon";

interface CustomSearchButtonProps {
    onClick: () => void;
    onTouchStart?: () => void;
    onFocus?: () => void;
    onMouseOver?: () => void;
    translations?: {
        buttonText?: string;
        buttonAriaLabel?: string;
    };
}

function getShortcutKey(): string {
    if (typeof navigator === "undefined") {
        return "Ctrl+K";
    }
    const isMac = /Mac|iPhone|iPod|iPad/.test(navigator.platform);
    return isMac ? "⌘K" : "Ctrl+K";
}

// Keyboard handling (open shortcut, type-to-open) is owned by the parent SearchBar,
// which forwards its ref here so the shortcut targets this button.
const CustomSearchButton = forwardRef<HTMLButtonElement, CustomSearchButtonProps>(
    ({ onClick, onTouchStart, onFocus, onMouseOver, translations = {} }, ref) => {
        const [shortcutKey, setShortcutKey] = useState("Ctrl+K");

        useEffect(() => {
            setShortcutKey(getShortcutKey());
        }, []);

        return (
            <button
                type="button"
                ref={ref}
                className={clsx(
                    "flex items-center gap-2 text-sm cursor-pointer",
                    "bg-ifm-background border border-black/10 p-2.5 md:py-1.5 md:pr-1.5 md:pl-3",
                    "dark:border-white/10 text-ifm-menu justify-between rounded-[32px] hover:bg-black/5 dark:hover:bg-white/5"
                )}
                aria-label={translations.buttonAriaLabel || "Search"}
                onClick={onClick}
                onTouchStart={onTouchStart}
                onFocus={onFocus}
                onMouseOver={onMouseOver}
            >
                <Icon icon="search" size="xs" />
                <span className="hidden md:inline-flex text-ifm-menu md:text-sm/3.5 text-left w-auto">
                    {translations.buttonText || "Search"}
                </span>
                <div className="hidden md:inline-flex bg-black/10 dark:bg-white/10 border border-black/10 dark:border-white/10 text-ifm-menu px-1.5 rounded-2xl text-xs font-mono">
                    {shortcutKey}
                </div>
            </button>
        );
    }
);

CustomSearchButton.displayName = "CustomSearchButton";

export default CustomSearchButton;
