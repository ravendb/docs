import React, { useMemo } from "react";
import CardWithImage from "../Common/CardWithImage";
import Heading from "@theme/Heading";
import { usePluginData } from "@docusaurus/useGlobalData";
import type { Guide } from "@site/src/plugins/recent-guides-plugin";

interface FeaturedGuidesProps {
    guidesTitles?: [string, string];
}

export default function FeaturedGuides({ guidesTitles }: FeaturedGuidesProps) {
    const pluginData = usePluginData("recent-guides-plugin") as {
        guides: Guide[];
    };
    const allGuides = pluginData?.guides || [];

    const featuredGuides = useMemo(() => {
        if (!guidesTitles) {
            return allGuides.slice(0, 2);
        }

        const filtered = guidesTitles
            .map((title) => allGuides.find((guide) => guide.title === title))
            .filter((guide): guide is Guide => guide !== undefined)
            .slice(0, 2);

        return filtered;
    }, [allGuides, guidesTitles]);

    return (
        <div className="flex flex-col gap-6">
            <Heading as="h2" className="!mb-0">
                Featured guides
            </Heading>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {featuredGuides.map((guide, index) => (
                    <CardWithImage
                        key={guide.permalink}
                        title={guide.title}
                        description={guide.description}
                        imgSrc={guide.image}
                        imgIcon={guide.icon}
                        url={guide.permalink}
                        tags={guide.tags}
                        animationDelay={index * 50}
                    />
                ))}
            </div>
        </div>
    );
}
