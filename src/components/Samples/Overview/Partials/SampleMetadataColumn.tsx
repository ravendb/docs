import React, { ReactNode, useMemo } from "react";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import { usePluginData } from "@docusaurus/useGlobalData";
import clsx from "clsx";
import Heading from "@theme/Heading";
import Tag from "@site/src/theme/Tag";
import type { PluginData } from "@site/src/components/Samples/types";

export interface SampleMetadataColumnProps {
    className?: string;
    actionsCard?: ReactNode;
    relatedResources?: ReactNode;
}

interface TagData {
    [key: string]: {
        label: string;
    };
}

function createFilterUrl(tagKey: string): string {
    return `/samples?tags=${tagKey}`;
}

function getTagsWithLabels(tagKeys: string[] | undefined, tagData: TagData) {
    if (!tagKeys || tagKeys.length === 0) {
        return [];
    }

    return tagKeys.map((key) => ({
        key,
        label: tagData[key]?.label || key,
    }));
}

interface TagSectionProps {
    title: string;
    tags: Array<{ key: string; label: string }>;
}

function TagSection({ title, tags }: TagSectionProps) {
    if (tags.length === 0) {
        return null;
    }

    return (
        <div className="flex flex-col gap-2">
            <Heading as="h5" className="!mb-0 text-sm font-semibold">
                {title}
            </Heading>
            <div className="flex flex-wrap gap-1">
                {tags.map((tag) => (
                    <Tag key={tag.key} size="xs" permalink={createFilterUrl(tag.key)}>
                        {tag.label}
                    </Tag>
                ))}
            </div>
        </div>
    );
}

export default function SampleMetadataColumn({ className, actionsCard, relatedResources }: SampleMetadataColumnProps) {
    const { frontMatter } = useDoc();
    const pluginData = usePluginData("recent-samples-plugin") as PluginData | undefined;

    const { challengesSolutionsTagsData, featureTagsData, techStackTagsData } = useMemo(() => {
        const allTags = pluginData?.tags || [];

        const challengesSolutionsTags: TagData = {};
        const featureTags: TagData = {};
        const techStackTags: TagData = {};

        allTags.forEach((tag) => {
            const tagEntry = { label: tag.label };
            if (tag.category === "challenges-solutions") {
                challengesSolutionsTags[tag.key] = tagEntry;
            } else if (tag.category === "feature") {
                featureTags[tag.key] = tagEntry;
            } else if (tag.category === "tech-stack") {
                techStackTags[tag.key] = tagEntry;
            }
        });

        return {
            challengesSolutionsTagsData: challengesSolutionsTags,
            featureTagsData: featureTags,
            techStackTagsData: techStackTags,
        };
    }, [pluginData]);

    const challengesSolutionsTagKeys = ((frontMatter as any).challengesSolutionsTags ||
        (frontMatter as any).challengesSolutionsTags) as string[];
    const featureTagKeys = (frontMatter as any).featureTags as string[];
    const techStackTagKeys = (frontMatter as any).techStackTags as string[];
    const category = (frontMatter as any).category as string;
    const license = (frontMatter as any).license as string;

    const challengesSolutionsTags = getTagsWithLabels(challengesSolutionsTagKeys, challengesSolutionsTagsData);
    const featureTags = getTagsWithLabels(featureTagKeys, featureTagsData);
    const techStackTags = getTagsWithLabels(techStackTagKeys, techStackTagsData);

    return (
        <div className={clsx("sticky top-[90px] flex flex-col gap-4", className)}>
            {actionsCard}

            <TagSection title="Challenges & Solutions" tags={challengesSolutionsTags} />
            <TagSection title="Feature" tags={featureTags} />
            <TagSection title="Tech stack" tags={techStackTags} />

            {category && (
                <div className="flex flex-col gap-2">
                    <Heading as="h5" className="!mb-0 text-sm font-semibold">
                        Category
                    </Heading>
                    <p className="text-sm text-black dark:text-white !mb-0">{category}</p>
                </div>
            )}

            {license && (
                <div className="flex flex-col gap-2">
                    <Heading as="h5" className="!mb-0 text-sm font-semibold">
                        License
                    </Heading>
                    <p className="text-sm text-black dark:text-white !mb-0">{license}</p>
                </div>
            )}

            {relatedResources && (
                <div className="flex flex-col gap-2">
                    <Heading as="h5" className="!mb-0 text-sm font-semibold">
                        Related Resources
                    </Heading>
                    <div className="flex flex-col gap-1">{relatedResources}</div>
                </div>
            )}
        </div>
    );
}
