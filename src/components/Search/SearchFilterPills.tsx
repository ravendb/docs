import React, { type ReactNode } from "react";
import clsx from "clsx";
import { SEARCH_FILTER_GROUPS } from "./searchSource";
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
            {SEARCH_FILTER_GROUPS.map((group, groupIndex) => (
                <React.Fragment key={groupIndex}>
                    {groupIndex > 0 && (
                        <span aria-hidden="true" className="mx-1 h-4 w-px self-center bg-black/10 dark:bg-white/10" />
                    )}
                    {group.map((filter) => (
                        <button
                            key={filter.label}
                            type="button"
                            aria-pressed={active === filter.kind}
                            onClick={() => onChange(filter.kind)}
                            className={clsx(
                                "cursor-pointer select-none rounded-full border px-3 py-1 text-xs font-semibold transition-colors",
                                active === filter.kind
                                    ? "border-transparent bg-primary text-white dark:text-black"
                                    : "border-black/10 bg-black/5 text-ifm-menu hover:bg-black/10 dark:border-white/10 dark:bg-white/5 dark:hover:bg-white/10"
                            )}
                        >
                            {filter.label}
                        </button>
                    ))}
                </React.Fragment>
            ))}
        </div>
    );
}
