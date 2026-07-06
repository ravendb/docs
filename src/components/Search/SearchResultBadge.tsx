import React, { type ReactNode } from "react";
import clsx from "clsx";
import { Icon } from "@site/src/components/Common/Icon";
import { ContentSource, getContentSourceIcon, getContentSourceLabel } from "@site/src/components/Common/contentSource";

// Color per source. Neutral docs/external reuse the repo's chip tokens; the rest tint that chip.
const BADGE_CLASSES: Record<ContentSource, string> = {
    docs: "bg-black/5 dark:bg-white/5 text-ifm-menu border-black/10 dark:border-white/10",
    guides: "bg-emerald-500/15 text-emerald-700 dark:text-emerald-300 border-emerald-600/25 dark:border-emerald-400/25",
    cloud: "bg-sky-500/15 text-sky-700 dark:text-sky-300 border-sky-600/25 dark:border-sky-400/25",
    samples: "bg-violet-500/15 text-violet-700 dark:text-violet-300 border-violet-600/25 dark:border-violet-400/25",
    external: "bg-black/5 dark:bg-white/5 text-ifm-menu border-black/10 dark:border-white/10",
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
                "inline-flex h-5 shrink-0 items-center gap-1 select-none border",
                "rounded-full pl-1.5 pr-2 text-[11px] font-medium leading-4",
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
