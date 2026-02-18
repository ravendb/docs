import React from "react";
import Link from "@docusaurus/Link";
import Heading from "@theme/Heading";
import Tag from "../../theme/Tag";
import clsx from "clsx";
import isInternalUrl from "@docusaurus/isInternalUrl";
import { useTagLimit } from "@site/src/hooks/useTagLimit";

export interface RecentGuidesListItemProps {
    title: string;
    tags: { label: string; permalink: string }[];
    time: string;
    url: string;
    isLast?: boolean;
}

export default function RecentGuidesListItem({ title, tags, time, url, isLast }: RecentGuidesListItemProps) {
    const { visibleTags, hiddenCount, isExpanded, expandTags } = useTagLimit({
        tags,
    });

    return (
        <div
            className={clsx(
                "relative group p-4 flex flex-col gap-1 cursor-pointer !transition-colors",
                "text-base !text-inherit",
                "hover:bg-black/5 dark:hover:bg-white/5",
                !isLast && "border-b border-black/10 dark:border-white/10"
            )}
        >
            <Heading as="h6" className="!mb-0 !font-bold !text-inherit">
                <Link
                    to={url}
                    className="!text-inherit no-underline hover:!no-underline after:absolute after:inset-0 block truncate w-full"
                    title={title}
                >
                    {title}
                </Link>
            </Heading>
            <div className="flex flex-col-reverse sm:flex-row sm:items-center gap-1 justify-between pointer-events-none">
                <div className="flex gap-1 relative flex-wrap flex-1 min-w-0">
                    {visibleTags.map((tag) => (
                        <Tag key={tag.label} permalink={tag.permalink} size="xs" className="pointer-events-auto">
                            {tag.label}
                        </Tag>
                    ))}
                    {!isExpanded && hiddenCount > 0 && (
                        <Tag
                            size="xs"
                            onClick={(e) => {
                                e.preventDefault();
                                e.stopPropagation();
                                expandTags();
                            }}
                            title="Show all tags"
                            className="pointer-events-auto"
                        >
                            +{hiddenCount} more
                        </Tag>
                    )}
                </div>
                <span className="text-xs shrink-0">
                    {!isInternalUrl(url) && "External • "}
                    {time}
                </span>
            </div>
        </div>
    );
}
