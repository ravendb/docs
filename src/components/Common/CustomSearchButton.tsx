import React, { useEffect, useState } from "react";
import clsx from "clsx";
import { useDocSearchKeyboardEvents } from "@docsearch/react";

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
  if (typeof navigator === "undefined") return "Ctrl+K";
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
        "flex items-center gap-2 text-sm  transition-colors transition-duration-300 cursor-pointer",
        "bg-ifm-background border border-black/10 p-2.5 md:py-1.5 md:pr-1.5 md:pl-3",
        "dark:border-white/10 text-ifm-menu justify-between rounded-[32px] hover:bg-black/5 dark:hover:bg-white/5",
      )}
      aria-label={translations.buttonAriaLabel || "Search"}
      onClick={onClick}
      onTouchStart={onTouchStart}
      onFocus={onFocus}
      onMouseOver={onMouseOver}
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="14"
        height="14"
        viewBox="0 0 14 14"
        fill="none"
        className="text-ifm-menu w-3 h-3"
      >
        <path
          fill="currentColor"
          fillRule="evenodd"
          d="M10.438 6.188a4.25 4.25 0 1 1-8.5 0 4.25 4.25 0 0 1 8.5 0Zm-.909 4.049a5.25 5.25 0 1 1 .707-.707l2.68 2.679a.5.5 0 0 1-.707.707l-2.68-2.68Z"
          clipRule="evenodd"
        />
      </svg>
      <span className="hidden md:inline-flex text-ifm-menu md:text-sm/3.5 text-left w-auto">
        {translations.buttonText || "Search"}
      </span>
      <div className="hidden md:inline-flex bg-black/10 dark:bg-white/10 border border-black/10 dark:border-white/10 text-ifm-menu px-1.5 rounded-2xl text-xs font-mono">
        {shortcutKey}
      </div>
    </button>
  );
}
