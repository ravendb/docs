import { IconName } from "@site/src/typescript/iconName";

// Shared by See Also, sample related-resources, and search results.
export type ContentSource = "docs" | "cloud" | "guides" | "samples" | "external";

export const CONTENT_SOURCES: Record<ContentSource, { label: string; icon: IconName }> = {
    docs: { label: "Docs", icon: "document2" },
    cloud: { label: "Cloud", icon: "cloud" },
    guides: { label: "Guide", icon: "guides" },
    samples: { label: "Sample", icon: "code" },
    external: { label: "External", icon: "newtab" },
};

export const getContentSourceIcon = (source: ContentSource): IconName => CONTENT_SOURCES[source].icon;

export const getContentSourceLabel = (source: ContentSource): string => CONTENT_SOURCES[source].label;
