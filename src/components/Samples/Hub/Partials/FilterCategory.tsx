import React, { useEffect } from "react";
import { motion, AnimatePresence } from "motion/react";
import { Icon } from "@site/src/components/Common/Icon";
import Checkbox from "@site/src/components/Common/Checkbox";
import useBoolean from "@site/src/hooks/useBoolean";

interface FilterTag {
    key: string;
    label: string;
}

interface FilterCategoryProps {
    name: string;
    label: string;
    tags: FilterTag[];
    selectedTags: Set<string>;
    onTagToggle: (tagKey: string) => void;
    isExpanded: boolean;
    onToggleExpanded: () => void;
}

export default function FilterCategory({
    label,
    tags,
    selectedTags,
    onTagToggle,
    isExpanded,
    onToggleExpanded,
}: FilterCategoryProps) {
    const {
        value: isTagsExpanded,
        setTrue: expandTags,
        setFalse: collapseTags,
    } = useBoolean(false);
    const [manuallyCollapsed, setManuallyCollapsed] = React.useState(false);

    const visibleTags = isTagsExpanded ? tags : tags.slice(0, 5);
    const hiddenCount = Math.max(0, tags.length - 5);

    useEffect(() => {
        if (!isTagsExpanded && !manuallyCollapsed && tags.length > 5) {
            const hiddenTags = tags.slice(5);
            const hasSelectedHiddenTag = hiddenTags.some((tag) => selectedTags.has(tag.key));
            if (hasSelectedHiddenTag) {
                expandTags();
            }
        }
    }, [selectedTags, tags, isTagsExpanded, expandTags, manuallyCollapsed]);

    const handleToggleTagsExpanded = () => {
        if (isTagsExpanded) {
            setManuallyCollapsed(true);
            collapseTags();
        } else {
            setManuallyCollapsed(false);
            expandTags();
        }
    };

    return (
        <div className="flex flex-col gap-2">
            <button
                onClick={onToggleExpanded}
                className="flex items-center justify-between text-sm font-semibold text-black dark:text-white hover:opacity-80 transition-opacity cursor-pointer"
            >
                <span className="text-start">{label}</span>
                <motion.div
                    animate={{ rotate: isExpanded ? 0 : -180 }}
                    transition={{ duration: 0.2, ease: "easeInOut" }}
                >
                    <Icon icon="chevron-down" size="xs" />
                </motion.div>
            </button>

            <AnimatePresence initial={false}>
                {isExpanded && (
                    <motion.div
                        initial={{ height: 0, opacity: 0 }}
                        animate={{ height: "auto", opacity: 1 }}
                        exit={{ height: 0, opacity: 0 }}
                        transition={{ duration: 0.2, ease: "easeInOut" }}
                        className="overflow-hidden"
                    >
                        <div className="flex flex-col gap-2 pl-2">
                            {visibleTags.map((tag) => (
                                <Checkbox
                                    key={tag.key}
                                    checked={selectedTags.has(tag.key)}
                                    onChange={() => onTagToggle(tag.key)}
                                    label={tag.label}
                                />
                            ))}

                            {hiddenCount > 0 && (
                                <button
                                    onClick={handleToggleTagsExpanded}
                                    className="flex items-center gap-2 text-xs hover:text-black dark:hover:text-white cursor-pointer mt-1"
                                >
                                    <Icon
                                        icon={isTagsExpanded ? "minus" : "plus"}
                                        size="2xs"
                                        className="text-black/30 dark:text-white/30"
                                    />
                                    <span>
                                        {isTagsExpanded ? "Less" : "More "}
                                        {!isTagsExpanded && (
                                            <span className="text-black/30 dark:text-white/30">({hiddenCount})</span>
                                        )}
                                    </span>
                                </button>
                            )}
                        </div>
                    </motion.div>
                )}
            </AnimatePresence>
        </div>
    );
}
