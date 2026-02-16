import React, { ReactNode } from "react";
import Link from "@docusaurus/Link";
import Heading from "@theme/Heading";
import { Icon } from "@site/src/components/Common/Icon";
import { IconName } from "@site/src/typescript/iconName";
import Badge from "@site/src/components/Common/Badge";
import LazyImage from "@site/src/components/Common/LazyImage";
import isInternalUrl from "@docusaurus/isInternalUrl";
import clsx from "clsx";
import { useTagLimit } from "@site/src/hooks/useTagLimit";
import Tag from "@site/src/theme/Tag";

export interface CardWithImageProps {
    title: string;
    description: ReactNode;
    imgSrc?: string | { light: string; dark: string };
    imgAlt?: string;
    url: string;
    imgIcon?: IconName;
    tags?: Array<{ label: string; permalink: string }>;
    date?: string;
    animationDelay?: number;
}

export default function CardWithImage({
    title,
    description,
    imgSrc,
    imgAlt = "",
    url,
    imgIcon,
    tags = [],
    date,
    animationDelay = 0,
}: CardWithImageProps) {
    const hasImage = Boolean(imgSrc);
    const hasTags = tags.length > 0;
    const hasDate = date !== undefined;

    const { visibleTags, hiddenCount, isExpanded, expandTags } = useTagLimit({
        tags,
    });

    return (
        <Link to={url} className="card-wrapper">
            <div
                className={clsx(
                    "card group flex h-full flex-col",
                    "p-4 overflow-hidden rounded-2xl",
                    "border border-black/10 dark:border-white/10",
                    "!bg-black/5 dark:!bg-white/5 text-inherit hover:no-underline",
                    "hover:border-black/20 dark:hover:border-white/20",
                    "hover:!bg-black/10 dark:hover:!bg-white/10",
                    "!transition-all",
                    "animate-in fade-in slide-in-from-bottom-4",
                )}
                style={{
                    animationDelay: `${animationDelay}ms`,
                    animationDuration: "400ms",
                    animationFillMode: "backwards",
                }}
            >
                <div
                    className={clsx(
                        "flex items-center justify-center",
                        "rounded-xl mb-4 overflow-hidden",
                        "relative aspect-[79/24]",
                        !hasImage &&
                            "bg-gradient-to-b from-[#204879] to-[#0F1425] to-[70%]",
                    )}
                >
                    {hasImage ? (
                        <LazyImage
                            imgSrc={imgSrc}
                            alt={imgAlt}
                            className={clsx(
                                "pointer-events-none",
                                "w-full h-full object-cover object-center",
                                "!transition-transform origin-bottom",
                                "group-hover:scale-105 group-hover:translate-y-1",
                            )}
                        />
                    ) : (
                        <Icon
                            icon={imgIcon ?? "default"}
                            size="xl"
                            className="filter brightness-0 invert !transition-transform group-hover:scale-110"
                        />
                    )}
                    {!isInternalUrl(url) && (
                        <Badge
                            className="absolute top-2 right-2"
                            variant="default"
                            size="sm"
                        >
                            External
                        </Badge>
                    )}
                </div>
                <div className="flex flex-col gap-0.5">
                    <Heading
                        as="h4"
                        className="!mb-0 !text-base !font-bold !leading-5 !break-normal"
                    >
                        {title}
                    </Heading>
                </div>
                <p className="!mb-0 text-sm pt-2">{description}</p>
                {(hasTags || hasDate) && (
                    <div className="flex flex-wrap justify-between pt-2 gap-3">
                        {hasTags && (
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
                        )}
                        {hasDate && (
                            <p className="!mb-0 text-xs flex-shrink-0 leading-[20px]">
                                {date}
                            </p>
                        )}
                    </div>
                )}
            </div>
        </Link>
    );
}
