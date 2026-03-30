import React, { useState, useEffect } from "react";
import Heading from "@theme/Heading";
import clsx from "clsx";
import Toggle from "@site/src/components/Common/Toggle";
import FilterCategory from "./FilterCategory";
import Button from "@site/src/components/Common/Button";

interface FilterTag {
    key: string;
    label: string;
}

interface FilterCategoryData {
    name: string;
    label: string;
    tags: FilterTag[];
}

interface SamplesFilterProps {
    categories: FilterCategoryData[];
    selectedTags: Set<string>;
    onTagToggle: (tagKey: string) => void;
    matchLogic: "any" | "all";
    onMatchLogicChange: (logic: "any" | "all") => void;
    onClearFilters?: () => void;
    showHeader?: boolean;
}

export default function SamplesFilter({
    categories,
    selectedTags,
    onTagToggle,
    matchLogic,
    onMatchLogicChange,
    onClearFilters,
    showHeader = true,
}: SamplesFilterProps) {
    const [expandedCategories, setExpandedCategories] = useState<Set<string>>(new Set(categories.map((c) => c.name)));
    const [searchQuery, setSearchQuery] = useState("");

    useEffect(() => {
        const categoriesToExpand = new Set(expandedCategories);
        let hasChanges = false;

        categories.forEach((category) => {
            const hasSelectedTag = category.tags.some((tag) => selectedTags.has(tag.key));
            if (hasSelectedTag && !categoriesToExpand.has(category.name)) {
                categoriesToExpand.add(category.name);
                hasChanges = true;
            }
        });

        if (hasChanges) {
            setExpandedCategories(categoriesToExpand);
        }
    }, [selectedTags, categories]);

    const toggleCategory = (categoryName: string) => {
        const newExpanded = new Set(expandedCategories);
        if (newExpanded.has(categoryName)) {
            newExpanded.delete(categoryName);
        } else {
            newExpanded.add(categoryName);
        }
        setExpandedCategories(newExpanded);
    };

    const filteredCategories = categories
        .map((category) => ({
            ...category,
            tags: category.tags.filter((tag) => tag.label.toLowerCase().includes(searchQuery.toLowerCase())),
        }))
        .filter((category) => category.tags.length > 0);

    const handleClearAll = () => {
        setSearchQuery("");
        onMatchLogicChange("any");
        onClearFilters?.();
    };

    const hasActiveFilters = selectedTags.size > 0 || searchQuery.length > 0 || matchLogic !== "any";

    return (
        <div className="flex flex-col gap-4 lg:sticky lg:top-4">
            {showHeader && (
                <div className="flex items-center justify-between">
                    <Heading as="h2" className="!mb-0">
                        Filters
                    </Heading>
                    {hasActiveFilters && (
                        <Button onClick={handleClearAll} variant="secondary" size="xs">
                            Clear all
                        </Button>
                    )}
                </div>
            )}
            <div className="flex flex-col gap-2">
                <input
                    type="search"
                    placeholder="Search filters"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                    className={clsx(
                        "px-3 py-2 rounded-full",
                        "border border-black/10 dark:border-white/10",
                        "text-sm text-black dark:text-white",
                        "placeholder-black/30 dark:placeholder-white/30"
                    )}
                />
            </div>

            <div className="flex flex-col gap-2">
                <Heading as="h5" className="!mb-0 text-sm">
                    Match logic
                </Heading>
                <Toggle
                    options={[
                        { value: "any", label: "Any" },
                        { value: "all", label: "All" },
                    ]}
                    value={matchLogic}
                    onChange={onMatchLogicChange}
                />
            </div>

            {filteredCategories.length > 0 ? (
                filteredCategories.map((category) => (
                    <FilterCategory
                        key={category.name}
                        name={category.name}
                        label={category.label}
                        tags={category.tags}
                        selectedTags={selectedTags}
                        onTagToggle={onTagToggle}
                        isExpanded={expandedCategories.has(category.name)}
                        onToggleExpanded={() => toggleCategory(category.name)}
                    />
                ))
            ) : (
                <div className="text-sm text-black/30 dark:text-white/30 py-4">Nothing found</div>
            )}
        </div>
    );
}
