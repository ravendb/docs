import React, { type ReactNode } from "react";
import clsx from "clsx";
import { SEARCH_FILTERS } from "./searchSource";
import type { ContentSource } from "@site/src/components/Common/contentSource";

interface SearchFilterPillsProps {
    active: ContentSource | null;
    onChange: (kind: ContentSource | null) => void;
    className?: string;
}

export default function SearchFilterPills({ active, onChange, className }: SearchFilterPillsProps): ReactNode {
    return (
        <div
            role="group"
            aria-label="Filter results by source"
            className={clsx("flex flex-wrap items-center gap-2", className)}
        >
            {SEARCH_FILTERS.map((filter) => {
                const isActive = active === filter.kind;
                return (
                    <React.Fragment key={filter.label}>
                        {filter.separated && (
                            <span
                                aria-hidden="true"
                                className="mx-1 h-4 w-px self-center bg-black/15 dark:bg-white/15"
                            />
                        )}
                        <button
                            type="button"
                            aria-pressed={isActive}
                            onClick={() => onChange(filter.kind)}
                            className={clsx(
                                "cursor-pointer select-none rounded-full border px-3 py-1 text-xs font-semibold transition-colors",
                                isActive
                                    ? "border-transparent bg-primary text-white"
                                    : "border-black/10 bg-black/5 text-ifm-menu hover:bg-black/10 dark:border-white/15 dark:bg-white/5 dark:hover:bg-white/10"
                            )}
                        >
                            {filter.label}
                        </button>
                    </React.Fragment>
                );
            })}
        </div>
    );
}
