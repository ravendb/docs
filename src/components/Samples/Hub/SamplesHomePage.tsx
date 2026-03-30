import React, { useState, useMemo, useEffect } from "react";
import { SamplesHeader, SamplesFilter, SamplesGrid } from "@site/src/components/Samples";
import { usePluginData } from "@docusaurus/useGlobalData";
import { useHistory, useLocation } from "@docusaurus/router";
import type { Tag, PluginData } from "../types";
import Drawer from "@site/src/components/Common/Drawer";
import useBoolean from "@site/src/hooks/useBoolean";
import Button from "@site/src/components/Common/Button";
import clsx from "clsx";

const categoryLabels: Record<string, string> = {
    "tech-stack": "Tech Stack",
    "challenges-solutions": "Challenges & Solutions",
    "feature": "Features",
    "other": "Other",
};

export default function SamplesHomePage() {
    const pluginData = usePluginData("recent-samples-plugin") as PluginData | undefined;
    const history = useHistory();
    const location = useLocation();

    const [selectedTags, setSelectedTags] = useState<Set<string>>(() => {
        // eslint-disable-next-line no-undef
        const params = new URLSearchParams(location.search);
        const tagsParam = params.get("tags");
        return tagsParam ? new Set(tagsParam.split(",")) : new Set();
    });

    const [matchLogic, setMatchLogic] = useState<"any" | "all">(() => {
        // eslint-disable-next-line no-undef
        const params = new URLSearchParams(location.search);
        const logicParam = params.get("match");
        return logicParam === "all" ? "all" : "any";
    });

    const samples = pluginData?.samples || [];
    const allTags = pluginData?.tags || [];

    const categories = useMemo(() => {
        const categoryMap: Record<string, { name: string; label: string; tags: Tag[] }> = {};

        allTags.forEach((tag) => {
            const category = tag.category || "other";
            if (!categoryMap[category]) {
                categoryMap[category] = {
                    name: category,
                    label: categoryLabels[category] || "Other",
                    tags: [],
                };
            }
            categoryMap[category].tags.push({
                key: tag.key,
                label: tag.label,
                count: tag.count,
            });
        });

        return Object.values(categoryMap);
    }, [allTags]);

    useEffect(() => {
        // eslint-disable-next-line no-undef
        const params = new URLSearchParams();

        if (selectedTags.size > 0) {
            params.set("tags", Array.from(selectedTags).join(","));
        }

        if (matchLogic !== "any") {
            params.set("match", matchLogic);
        }

        const newSearch = params.toString();
        const currentSearch = location.search.replace(/^\?/, "");

        if (newSearch !== currentSearch) {
            history.replace({
                pathname: location.pathname,
                search: newSearch ? `?${newSearch}` : "",
            });
        }
    }, [selectedTags, matchLogic, history, location.pathname, location.search]);

    const handleTagToggle = (tagKey: string) => {
        const newSelected = new Set(selectedTags);
        if (newSelected.has(tagKey)) {
            newSelected.delete(tagKey);
        } else {
            newSelected.add(tagKey);
        }
        setSelectedTags(newSelected);
    };

    const handleTagClick = (tagKey: string) => {
        const newSelected = new Set(selectedTags);
        if (newSelected.has(tagKey)) {
            newSelected.delete(tagKey);
        } else {
            newSelected.add(tagKey);
        }
        setSelectedTags(newSelected);
    };

    const { value: isFilterDrawerOpen, setTrue: openFilterDrawer, setFalse: closeFilterDrawer } = useBoolean(false);

    const hasActiveFilters = selectedTags.size > 0 || matchLogic !== "any";

    return (
        <>
            <SamplesHeader />
            <div className="lg:hidden mb-4">
                <Button
                    onClick={openFilterDrawer}
                    variant="secondary"
                    iconName="filter"
                    className={clsx("w-full relative", hasActiveFilters && "!border-primary !text-primary")}
                >
                    Filters
                    {hasActiveFilters && <span className="absolute -top-1 -right-1 w-3 h-3 bg-primary rounded-full" />}
                </Button>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-4 gap-8 mb-8">
                <div className="hidden lg:block lg:col-span-1">
                    <SamplesFilter
                        categories={categories}
                        selectedTags={selectedTags}
                        onTagToggle={handleTagToggle}
                        matchLogic={matchLogic}
                        onMatchLogicChange={setMatchLogic}
                        onClearFilters={() => setSelectedTags(new Set())}
                    />
                </div>

                <Drawer
                    open={isFilterDrawerOpen}
                    onClose={closeFilterDrawer}
                    title="Filters"
                    headerAction={
                        hasActiveFilters && (
                            <Button
                                onClick={() => {
                                    setSelectedTags(new Set());
                                    setMatchLogic("any");
                                }}
                                variant="secondary"
                                size="xs"
                            >
                                Clear all
                            </Button>
                        )
                    }
                >
                    <div className="flex flex-col gap-4">
                        <SamplesFilter
                            categories={categories}
                            selectedTags={selectedTags}
                            onTagToggle={handleTagToggle}
                            matchLogic={matchLogic}
                            onMatchLogicChange={setMatchLogic}
                            onClearFilters={() => setSelectedTags(new Set())}
                            showHeader={false}
                        />
                        <Button onClick={closeFilterDrawer} variant="default" className="w-full sticky bottom-0">
                            Apply Filters
                        </Button>
                    </div>
                </Drawer>

                <div className="lg:col-span-3">
                    <SamplesGrid
                        samples={samples.filter((s) => s.id !== "home")}
                        selectedTags={selectedTags}
                        matchLogic={matchLogic}
                        onTagClick={handleTagClick}
                    />
                </div>
            </div>
        </>
    );
}
