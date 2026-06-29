import React, { type ReactNode } from "react";
import clsx from "clsx";
import { Icon } from "@site/src/components/Common/Icon";
import { ContentSource, getContentSourceIcon, getContentSourceLabel } from "@site/src/components/Common/contentSource";

// Color per source.
const BADGE_CLASSES: Record<ContentSource, string> = {
    docs: "bg-slate-500/15 text-slate-700 dark:text-slate-300 ring-slate-500/25",
    guides: "bg-emerald-500/15 text-emerald-700 dark:text-emerald-300 ring-emerald-600/25 dark:ring-emerald-400/25",
    cloud: "bg-sky-500/15 text-sky-700 dark:text-sky-300 ring-sky-600/25 dark:ring-sky-400/25",
    samples: "bg-violet-500/15 text-violet-700 dark:text-violet-300 ring-violet-600/25 dark:ring-violet-400/25",
    external: "bg-zinc-500/15 text-zinc-700 dark:text-zinc-300 ring-zinc-500/25",
};

interface SearchResultBadgeProps {
    source: ContentSource | null;
    className?: string;
}

export default function SearchResultBadge({ source, className }: SearchResultBadgeProps): ReactNode {
    if (!source) {
        return null;
    }

    return (
        <span
            className={clsx(
                "search-source-badge inline-flex shrink-0 items-center gap-1 select-none",
                "rounded-full py-0.5 pl-1.5 pr-2 text-[11px] font-semibold leading-4 ring-1 ring-inset",
                BADGE_CLASSES[source],
                className
            )}
        >
            <span aria-hidden="true" className="inline-flex">
                <Icon icon={getContentSourceIcon(source)} size="2xs" />
            </span>
            {getContentSourceLabel(source)}
        </span>
    );
}
