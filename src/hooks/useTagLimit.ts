import { useState } from "react";

export interface UseTagLimitOptions {
    tags: Array<{ label: string; permalink: string }>;
    defaultLimit?: number;
}

export interface UseTagLimitReturn {
    visibleTags: Array<{ label: string; permalink: string }>;
    hiddenCount: number;
    isExpanded: boolean;
    expandTags: () => void;
}

export function useTagLimit({ tags, defaultLimit = 3 }: UseTagLimitOptions): UseTagLimitReturn {
    const [isExpanded, setIsExpanded] = useState(false);
    const [limit, setLimit] = useState(defaultLimit);

    const visibleTags = isExpanded ? tags : tags.slice(0, limit);
    const hiddenCount = tags.length - visibleTags.length;

    const expandTags = () => {
        setIsExpanded(true);
        setLimit(tags.length);
    };

    return {
        visibleTags,
        hiddenCount,
        isExpanded,
        expandTags,
    };
}
