import React from "react";
import Link from "@docusaurus/Link";
import clsx from "clsx";
import { useTagLimit } from "@site/src/hooks/useTagLimit";
import Tag from "@site/src/theme/Tag";
import isInternalUrl from "@docusaurus/isInternalUrl";

export interface GuideListItemProps {
    title: string;
    url: string;
    tags?: { label: string; permalink: string }[];
    date?: string;
}

export default function GuideListItem({
    title,
    url,
    tags = [],
    date,
}: GuideListItemProps) {
    const { visibleTags, hiddenCount, isExpanded, expandTags } = useTagLimit({
        tags,
    });

    return (
        <Link
            to={url}
            className={clsx(
                "flex flex-col-reverse py-3 px-2 gap-1",
                "border-b border-black/10 dark:border-white/10 !text-inherit",
                "hover:!no-underline",
                "group relative",
                "!transition-all",
                "hover:bg-black/5 dark:hover:bg-white/5",
                "sm:flex-row sm:items-center sm:gap-4",
            )}
        >
            <div
                className={clsx(
                    "flex flex-col flex-1 flex-wrap justify-between min-w-0 gap-1",
                    "lg:flex-row lg:items-center lg:flex-nowrap",
                )}
            >
                <p
                    className={clsx(
                        "!mb-0 font-semibold leading-5",
                        "sm:shrink",
                    )}
                >
                    {title}
                </p>
                <div
                    className={clsx(
                        "flex gap-1 items-center flex-wrap whitespace-normal",
                        "sm:shrink-0",
                    )}
                >
                    {visibleTags.map((tag) => (
                        <Tag
                            key={tag.label}
                            size="xs"
                            permalink={tag.permalink}
                        >
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
                        >
                            +{hiddenCount} more
                        </Tag>
                    )}
                </div>
            </div>
            {date && (
                <p
                    className={clsx(
                        "!mb-0 text-xs",
                        "overflow-hidden truncate sm:text-right whitespace-normal",
                        "shrink-0",
                    )}
                >
                    {!isInternalUrl(url) && "External • "}
                    {date}
                </p>
            )}
        </Link>
    );
}
