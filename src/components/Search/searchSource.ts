import { ContentSource } from "@site/src/components/Common/contentSource";

// docusaurus_tag is `docs-<pluginId>-<version>`; main docs has the implicit id "default".
const SOURCE_BY_PLUGIN_ID: Record<string, ContentSource> = {
    default: "docs",
    cloud: "cloud",
    guides: "guides",
    samples: "samples",
};

export function getSearchResultSource(docusaurusTag: string | string[] | undefined | null): ContentSource | null {
    const tags = Array.isArray(docusaurusTag) ? docusaurusTag : docusaurusTag ? [docusaurusTag] : [];

    for (const tag of tags) {
        const parts = tag.split("-");
        if (parts[0] === "docs" && parts.length >= 3) {
            const source = SOURCE_BY_PLUGIN_ID[parts[1]];
            if (source) {
                return source;
            }
        }
    }

    return null;
}

// docusaurus_tag and breadcrumb are indexed but not returned unless explicitly requested.
export const SEARCH_ATTRIBUTES_TO_RETRIEVE = [
    "hierarchy.lvl0",
    "hierarchy.lvl1",
    "hierarchy.lvl2",
    "hierarchy.lvl3",
    "hierarchy.lvl4",
    "hierarchy.lvl5",
    "hierarchy.lvl6",
    "content",
    "type",
    "url",
    "docusaurus_tag",
    "breadcrumb",
];

export interface SearchFilter {
    label: string;
    kind: ContentSource | null;
    pluginId: string | null;
}

// Filter pills, grouped. The UI draws a divider between groups, and every group after the
// first is a separate scope excluded from the default "All".
export const SEARCH_FILTER_GROUPS: SearchFilter[][] = [
    [
        { label: "All", kind: null, pluginId: null },
        { label: "Docs", kind: "docs", pluginId: "default" },
        { label: "Guides", kind: "guides", pluginId: "guides" },
        { label: "Cloud", kind: "cloud", pluginId: "cloud" },
    ],
    [{ label: "Samples", kind: "samples", pluginId: "samples" }],
];

export const SEARCH_FILTERS: SearchFilter[] = SEARCH_FILTER_GROUPS.flat();

// Plugin instances kept out of the default "All" scope (every group after the first).
export const ALL_SCOPE_EXCLUDED_PLUGIN_IDS = SEARCH_FILTER_GROUPS.slice(1)
    .flat()
    .map((f) => f.pluginId)
    .filter((id): id is string => id !== null);
