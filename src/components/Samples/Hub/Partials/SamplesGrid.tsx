import React, { useMemo } from "react";
import clsx from "clsx";
import { SampleCard } from "@site/src/components/Samples";
import Heading from "@theme/Heading";
import type { Sample } from "../../types";

interface SamplesGridProps {
    samples: Sample[];
    selectedTags: Set<string>;
    matchLogic: "any" | "all";
    onTagClick?: (tagKey: string) => void;
}

export default function SamplesGrid({ samples, selectedTags, matchLogic, onTagClick }: SamplesGridProps) {
    const filteredSamples = useMemo(() => {
        if (selectedTags.size === 0) {
            return samples;
        }

        return samples.filter((sample) => {
            const sampleTagKeys = new Set(sample.tags.map((t) => t.key));

            if (matchLogic === "any") {
                return Array.from(selectedTags).some((tag) => sampleTagKeys.has(tag));
            } else {
                return Array.from(selectedTags).every((tag) => sampleTagKeys.has(tag));
            }
        });
    }, [samples, selectedTags, matchLogic]);

    if (filteredSamples.length === 0) {
        return (
            <div className="flex items-center justify-center py-12">
                <p className="text-center text-black/60 dark:text-white/60">No samples found matching your filters.</p>
            </div>
        );
    }

    return (
        <div className="flex flex-col gap-4">
            <div className="flex items-center gap-4">
                <Heading as="h2" className="!mb-0">
                    Samples
                </Heading>
                <span
                    className={clsx(
                        "flex items-center justify-center rounded-full font-bold leading-none",
                        "w-8 h-8 text-[12px]",
                        "bg-black/10",
                        "dark:bg-white/20 dark:text-white"
                    )}
                >
                    {filteredSamples.length}
                </span>
            </div>
            <div className={clsx("grid grid-cols-1 md:grid-cols-2 gap-4", "animate-in fade-in")}>
                {filteredSamples.map((sample) => (
                    <SampleCard
                        key={sample.id}
                        title={sample.title}
                        description={sample.description}
                        url={sample.permalink}
                        imgSrc={sample.image}
                        imgAlt={sample.img_alt}
                        tags={sample.tags as any}
                        onTagClick={onTagClick}
                        selectedTags={selectedTags}
                    />
                ))}
            </div>
        </div>
    );
}
