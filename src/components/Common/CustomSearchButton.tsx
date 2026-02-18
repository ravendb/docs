import React, { useEffect, useState } from "react";
import clsx from "clsx";
import { useDocSearchKeyboardEvents } from "@docsearch/react";
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

export default function CustomSearchButton({
    onClick,
    onTouchStart,
    onFocus,
    onMouseOver,
    translations = {},
}: CustomSearchButtonProps) {
    const buttonRef = React.useRef<HTMLButtonElement>(null);
    const [shortcutKey, setShortcutKey] = useState("Ctrl+K");

    useEffect(() => {
        setShortcutKey(getShortcutKey());
    }, []);

    useDocSearchKeyboardEvents({
        isOpen: false,
        onOpen: onClick,
        onClose: () => {},
        searchButtonRef: buttonRef,
    });

    return (
        <button
            type="button"
            ref={buttonRef}
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
