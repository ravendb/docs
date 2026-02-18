import React from "react";
import clsx from "clsx";
import { Icon } from "@site/src/components/Common/Icon";

export type LayoutMode = "grid" | "list";

interface LayoutSwitcherProps {
    layoutMode: LayoutMode;
    onLayoutChange: (mode: LayoutMode) => void;
}

export default function LayoutSwitcher({ layoutMode, onLayoutChange }: LayoutSwitcherProps) {
    return (
        <div
            className={clsx(
                "relative w-fit ms-auto",
                "border border-black/10 dark:border-white/10",
                "flex gap-1 items-center p-1 rounded-3xl shrink-0"
            )}
        >
            <div
                className={clsx(
                    "absolute top-1 h-[32px] w-[32px]",
                    "bg-black/5 dark:bg-white/5",
                    "rounded-3xl",
                    "!transition-all",
                    "border border-black/10 dark:border-white/10"
                )}
                style={{
                    left: layoutMode === "grid" ? "4px" : "40px",
                }}
            />
            <button
                onClick={() => onLayoutChange("grid")}
                className={clsx(
                    "relative z-10",
                    "flex items-center justify-center p-1 rounded-3xl w-[32px] h-[32px]",
                    "border-0 cursor-pointer",
                    "!transition-all",
                    layoutMode === "grid" ? "opacity-100" : "opacity-50 hover:opacity-75"
                )}
                title="Grid view"
            >
                <Icon icon="layout-grid" size="xs" />
            </button>
            <button
                onClick={() => onLayoutChange("list")}
                className={clsx(
                    "relative z-10",
                    "flex items-center justify-center p-1 rounded-3xl w-[32px] h-[32px]",
                    "border-0 cursor-pointer",
                    "!transition-all",
                    layoutMode === "list" ? "opacity-100" : "opacity-50 hover:opacity-75"
                )}
                title="List view"
            >
                <Icon icon="layout-list" size="xs" />
            </button>
        </div>
    );
}
