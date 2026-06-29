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
    // Render a divider before this pill: a separate scope, excluded from "All".
    separated?: boolean;
}

export const SEARCH_FILTERS: SearchFilter[] = [
    { label: "All", kind: null, pluginId: null },
    { label: "Docs", kind: "docs", pluginId: "default" },
    { label: "Guides", kind: "guides", pluginId: "guides" },
    { label: "Cloud", kind: "cloud", pluginId: "cloud" },
    { label: "Samples", kind: "samples", pluginId: "samples", separated: true },
];

// Plugin instances kept out of the default "All" scope; each is reachable via its own separated pill.
export const ALL_SCOPE_EXCLUDED_PLUGIN_IDS = SEARCH_FILTERS.filter((f) => f.separated && f.pluginId).map(
    (f) => f.pluginId as string
);
