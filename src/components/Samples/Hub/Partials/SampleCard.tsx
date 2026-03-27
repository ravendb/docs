import React, { ReactNode } from "react";
import Link from "@docusaurus/Link";
import Heading from "@theme/Heading";
import LazyImage from "@site/src/components/Common/LazyImage";
import clsx from "clsx";
import Tag from "@site/src/theme/Tag";
import LanguageTag from "@site/src/components/Samples/Hub/Partials/LanguageTag";

interface TagWithCategory {
    label: string;
    key: string;
    category?: string;
}

export interface SampleCardProps {
    title: string;
    description: ReactNode;
    imgSrc?: string;
    imgAlt?: string;
    imgWidth?: number;
    imgHeight?: number;
    url: string;
    tags?: TagWithCategory[];
    animationDelay?: number;
    onTagClick?: (tagKey: string) => void;
    selectedTags?: Set<string>;
}

export default function SampleCard({
    title,
    description,
    imgSrc,
    imgAlt = "",
    url,
    tags = [],
    animationDelay = 0,
    onTagClick,
    selectedTags,
}: SampleCardProps) {
    const hasImage = Boolean(imgSrc);

    const challengesSolutionsTags = tags.filter((t) => t.category === "challenges-solutions");
    const featureTags = tags.filter((t) => t.category === "feature");
    const techStackTags = tags.filter((t) => t.category === "tech-stack");

    const languageTags = techStackTags.filter((t) => ["csharp", "java", "python", "php", "nodejs"].includes(t.key));

    const handleTagClick = (e: React.MouseEvent, tag: TagWithCategory) => {
        e.preventDefault();
        e.stopPropagation();
        onTagClick?.(tag.key);
    };

    const isTagSelected = (tag: TagWithCategory) => {
        if (!selectedTags || selectedTags.size === 0) {
            return true;
        }
        return selectedTags.has(tag.key);
    };

    return (
        <div
            className={clsx(
                "card-wrapper group vstack",
                "card flex h-full flex-col relative",
                "overflow-hidden rounded-2xl",
                "border border-black/10 dark:border-white/10",
                "!bg-black/5 dark:!bg-white/5 text-inherit group-hover:no-underline",
                "group-hover:border-black/20 dark:group-hover:border-white/20",
                "group-hover:!bg-black/10 dark:group-hover:!bg-white/10",
                "!transition-all",
                "animate-in fade-in slide-in-from-bottom-4"
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
                    "rounded-t-xl rounded-b-none overflow-hidden",
                    "relative aspect-[79/24]"
                )}
            >
                {hasImage && (
                    <>
                        <LazyImage
                            imgSrc={imgSrc}
                            alt={imgAlt}
                            className={clsx(
                                "pointer-events-none",
                                "w-full h-full object-cover object-center",
                                "!transition-transform origin-bottom",
                                "group-hover:scale-105 group-hover:translate-y-1"
                            )}
                            isRounded={false}
                        />
                        {languageTags.length > 0 && (
                            <div className="absolute end-1 top-1 flex gap-1">
                                {languageTags.map((tag) => (
                                    <LanguageTag
                                        key={tag.key}
                                        languageKey={tag.key}
                                        onClick={onTagClick ? (e) => handleTagClick(e, tag) : undefined}
                                        className={clsx(!isTagSelected(tag) && "opacity-50")}
                                    />
                                ))}
                            </div>
                        )}
                    </>
                )}
            </div>
            <article className="p-4">
                <Link to={url} className={clsx("absolute inset-0 z-1", "!transition-all")} />
                <div>
                    <div className="flex flex-col gap-0.5">
                        <Heading as="h4" className="!mb-0 !text-base !font-bold !leading-5 !break-normal">
                            {title}
                        </Heading>
                    </div>

                    <p className="!mb-0 text-sm pt-2 flex-grow">{description}</p>

                    <div className="flex flex-col gap-2 z-2 relative">
                        {challengesSolutionsTags.length > 0 && (
                            <div>
                                <span className="text-xs">Challenges & Solutions</span>
                                <div className="flex flex-wrap gap-1">
                                    {challengesSolutionsTags.map((tag) => (
                                        <Tag
                                            key={tag.label}
                                            size="xs"
                                            onClick={onTagClick ? (e) => handleTagClick(e, tag) : undefined}
                                            className={clsx(
                                                onTagClick && "cursor-pointer",
                                                !isTagSelected(tag) && "opacity-50"
                                            )}
                                        >
                                            {tag.label}
                                        </Tag>
                                    ))}
                                </div>
                            </div>
                        )}

                        {featureTags.length > 0 && (
                            <div>
                                <span className="text-xs">Features</span>
                                <div className="flex flex-wrap gap-1">
                                    {featureTags.map((tag) => (
                                        <Tag
                                            key={tag.label}
                                            size="xs"
                                            onClick={onTagClick ? (e) => handleTagClick(e, tag) : undefined}
                                            className={clsx(
                                                onTagClick && "cursor-pointer",
                                                !isTagSelected(tag) && "opacity-50"
                                            )}
                                        >
                                            {tag.label}
                                        </Tag>
                                    ))}
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            </article>
        </div>
    );
}
