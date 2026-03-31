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

export function useTagLimit({ tags, defaultLimit = 2 }: UseTagLimitOptions): UseTagLimitReturn {
    const [isExpanded, setIsExpanded] = useState(false);
    const [limit, setLimit] = useState(defaultLimit);

    const effectiveLimit = !isExpanded && tags.length === limit + 1 ? limit + 1 : limit;
    const visibleTags = isExpanded ? tags : tags.slice(0, effectiveLimit);
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
