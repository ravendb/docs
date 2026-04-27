import React, { useMemo } from "react";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import { usePluginData } from "@docusaurus/useGlobalData";
import clsx from "clsx";
import Heading from "@theme/Heading";
import Tag from "@site/src/theme/Tag";
import type { PluginData } from "@site/src/components/Samples/types";
import ActionsCard from "./ActionsCard";
import RelatedResource from "./RelatedResource";

export interface SampleMetadataColumnProps {
    className?: string;
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

export default function SampleMetadataColumn({ className }: SampleMetadataColumnProps) {
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

    const challengesSolutionsTagKeys = frontMatter.challenges_solutions_tags;
    const featureTagKeys = frontMatter.feature_tags;
    const techStackTagKeys = frontMatter.tech_stack_tags;
    const category = frontMatter.category;
    const license = frontMatter.license;
    const licenseUrl = frontMatter.license_url;
    const repositoryUrl = frontMatter.repository_url;
    const demoUrl = frontMatter.demo_url;
    const relatedResourceItems = frontMatter.related_resources;

    const challengesSolutionsTags = getTagsWithLabels(challengesSolutionsTagKeys, challengesSolutionsTagsData);
    const featureTags = getTagsWithLabels(featureTagKeys, featureTagsData);
    const techStackTags = getTagsWithLabels(techStackTagKeys, techStackTagsData);

    return (
        <div className={clsx("sticky top-[90px] flex flex-col gap-4", className)}>
            {(repositoryUrl || demoUrl) && <ActionsCard githubUrl={repositoryUrl} demoUrl={demoUrl} />}

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
                    <p className="text-sm text-black dark:text-white !mb-0">
                        {licenseUrl ? (
                            <a href={licenseUrl} target="_blank" rel="noopener noreferrer">
                                {license}
                            </a>
                        ) : (
                            license
                        )}
                    </p>
                </div>
            )}

            {relatedResourceItems && relatedResourceItems.length > 0 && (
                <div className="flex flex-col gap-2">
                    <Heading as="h5" className="!mb-0 text-sm font-semibold">
                        Related Resources
                    </Heading>
                    <div className="flex flex-col gap-1">
                        {relatedResourceItems.map((resource, index) => (
                            <RelatedResource
                                key={index}
                                type={resource.type}
                                documentationType={resource.documentation_type}
                                subtitle={resource.subtitle}
                                articleKey={resource.article_key}
                                externalUrl={resource.url}
                            />
                        ))}
                    </div>
                </div>
            )}
        </div>
    );
}
