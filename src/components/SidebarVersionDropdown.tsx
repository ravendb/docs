import React, { useState, useRef, useEffect } from "react";
import Link from "@docusaurus/Link";
import { useVersions, useLatestVersion, useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function CustomVersionDropdown() {
    const pluginId = "default";
    const versions = useVersions(pluginId);
    const latestVersion = useLatestVersion(pluginId);
    const { activeVersion } = useActiveDocContext(pluginId);

    const [open, setOpen] = useState(false);
    const wrapperRef = useRef<HTMLDivElement>(null);
    const buttonRef = useRef<HTMLButtonElement>(null);
    const [buttonWidth, setButtonWidth] = useState<number | null>(null);

    useEffect(() => {
        const updateWidth = () => {
            if (buttonRef.current) {
                setButtonWidth(buttonRef.current.offsetWidth);
            }
        };
        updateWidth();
        window.addEventListener("resize", updateWidth);
        return () => window.removeEventListener("resize", updateWidth);
    }, []);

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (!wrapperRef.current?.contains(event.target as Node)) {
                setOpen(false);
            }
        };
        document.addEventListener("mousedown", handleClickOutside);
        return () => document.removeEventListener("mousedown", handleClickOutside);
    }, []);

    const currentLabel = activeVersion?.label ?? latestVersion.label;

    return (
        <div ref={wrapperRef} className="relative w-full px-4 my-2">
            <span className="text-xs text-ifm-menu mb-1">Documentation version</span>
            <button
                ref={buttonRef}
                onClick={() => setOpen((o) => !o)}
                className="w-full flex justify-between items-center rounded-md border border-black/10 dark:border-white/10 px-3 text-sm py-2 bg-transparent text-ifm-menu hover:bg-black/5 dark:hover:bg-white/5 cursor-pointer !transition-all"
            >
                <span>{currentLabel}.x</span>
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    stroke-width="2"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                >
                    <path d="m7 15 5 5 5-5"></path>
                    <path d="m7 9 5-5 5 5"></path>
                </svg>
            </button>
            <div
                className={`absolute mt-1 z-50 rounded-md border border-black/10 dark:border-white/10 bg-ifm-background shadow-lg max-h-[400px] overflow-auto p-1 text-sm !transition-all !transition-duration-200 ease-out origin-top ${
                    open ? "opacity-100 scale-100 pointer-events-auto" : "opacity-0 scale-95 pointer-events-none"
                }`}
                style={{ width: buttonWidth ?? "100%" }}
            >
                <ul className="!p-0 !m-0">
                    {versions.map((version) => (
                        <li key={version.name} className="rounded-sm overflow-hidden">
                            <Link to={version.path} className="menu__link" onClick={() => setOpen(false)}>
                                {version.label}.x
                            </Link>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
}
