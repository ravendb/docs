import React from "react";
import Link from "@docusaurus/Link";
import { usePluginData } from "@docusaurus/useGlobalData";
import Heading from "@theme/Heading";
import Tag from "../../theme/Tag";
import { Icon } from "../Common/Icon";

export default function TagsGrid() {
    const pluginData = usePluginData("recent-guides-plugin") as {
        tags: Array<{
            key: string;
            label: string;
            permalink: string;
            description?: string;
            count: number;
        }>;
    };

    const tags = (pluginData?.tags || [])
        .filter((tag) => tag.count > 0)
        .sort((a, b) => {
            if (b.count !== a.count) {
                return b.count - a.count;
            }

            return a.label.localeCompare(b.label);
        })
        .slice(0, 25);

    return (
        <div className="flex flex-col gap-6">
            <div className="flex items-center justify-between shrink-0">
                <Heading as="h2" className="!mb-0">
                    Browse by topic
                </Heading>
                <Link
                    to="/guides/tags"
                    className="flex items-center gap-2 text-sm font-semibold text-primary hover:underline group"
                >
                    See all
                    <Icon icon="arrow-thin-right" size="xs" />
                </Link>
            </div>
            <div className="flex flex-wrap gap-2 overflow-hidden max-h-[230px] md:max-h-[115px]">
                {tags.map((tag) => (
                    <Tag key={tag.key} permalink={`/guides/tags${tag.permalink}`} count={tag.count}>
                        {tag.label}
                    </Tag>
                ))}
            </div>
        </div>
    );
}
