import React from "react";
import Link from "@docusaurus/Link";
import clsx from "clsx";
import { useTagLimit } from "@site/src/hooks/useTagLimit";
import Tag from "@site/src/theme/Tag";

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
                "flex items-center py-3 px-2",
                "border-b border-black/10 dark:border-white/10 !text-inherit",
                "hover:!no-underline",
                "group relative",
                "!transition-all",
                "hover:bg-black/5 dark:hover:bg-white/5",
            )}
        >
            <div className="flex flex-1 items-center justify-between flex-wrap min-w-0 gap-1">
                <p
                    className={clsx(
                        "!mb-0 text-base leading-5",
                        "overflow-hidden text-ellipsis whitespace-nowrap shrink-0",
                    )}
                >
                    {title}
                </p>
                <div className="flex gap-1 items-center flex-wrap">
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
                        "overflow-hidden text-ellipsis text-right whitespace-nowrap",
                        "shrink-0",
                        "w-[92px]",
                    )}
                >
                    {date}
                </p>
            )}
        </Link>
    );
}
