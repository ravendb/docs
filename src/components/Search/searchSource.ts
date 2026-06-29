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

// docusaurus_tag is faceted but not returned unless explicitly requested.
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
];

const BREADCRUMB_ACRONYMS = new Set([
    "ai",
    "api",
    "rest",
    "sql",
    "rql",
    "etl",
    "olap",
    "snmp",
    "kb",
    "faq",
    "url",
    "cli",
    "ui",
    "aws",
    "gcp",
    "sdk",
    "http",
    "json",
    "csv",
    "dns",
    "ssl",
    "tls",
    "jwt",
    "s3",
    "sqs",
    "eks",
    "aks",
    "gke",
]);

const BREADCRUMB_SKIP_ROOTS = new Set(["guides", "cloud", "samples", "templates"]);

function humanizeSegment(segment: string): string {
    return segment
        .split("-")
        .map((word) =>
            BREADCRUMB_ACRONYMS.has(word) ? word.toUpperCase() : word.charAt(0).toUpperCase() + word.slice(1)
        )
        .join(" ");
}

// Category chain from the URL (the crawler's hierarchy is too shallow), e.g.
// /7.2/ai-integration/vector-search/overview -> ["AI Integration", "Vector Search"].
export function getResultBreadcrumb(url: string | undefined | null): string[] {
    if (!url) {
        return [];
    }
    let segments = url.split(/[?#]/)[0].split("/").filter(Boolean);
    if (segments.length && /^\d+\.\d+$/.test(segments[0])) {
        segments = segments.slice(1);
    }
    if (segments.length && BREADCRUMB_SKIP_ROOTS.has(segments[0])) {
        segments = segments.slice(1);
    }
    return segments.slice(0, -1).map(humanizeSegment);
}

export interface SearchFilter {
    label: string;
    kind: ContentSource | null;
    pluginId: string | null;
    tag: string | null;
    // Render a divider before this pill: a separate scope, excluded from "All".
    separated?: boolean;
}

export const SEARCH_FILTERS: SearchFilter[] = [
    { label: "All", kind: null, pluginId: null, tag: null },
    { label: "Docs", kind: "docs", pluginId: "default", tag: "docs-default-current" },
    { label: "Guides", kind: "guides", pluginId: "guides", tag: "docs-guides-current" },
    { label: "Cloud", kind: "cloud", pluginId: "cloud", tag: "docs-cloud-current" },
    { label: "Samples", kind: "samples", pluginId: "samples", tag: "docs-samples-current", separated: true },
];

// Tags kept out of the default "All" scope, derived from the separated pills so the
// divider, the pill, and the exclusion never drift.
export const ALL_SCOPE_EXCLUDED_TAGS = SEARCH_FILTERS.filter((f) => f.separated && f.tag).map((f) => f.tag as string);
