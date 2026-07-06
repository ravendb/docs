import { useMemo } from "react";
import { usePluginData } from "@docusaurus/useGlobalData";

type GuidesData = { guides?: Array<{ permalink?: string; external_url?: string }> };

function normalizePath(url: string): string {
    const path = url.replace(/^https?:\/\/[^/]+/, "").split(/[?#]/)[0];
    return path.replace(/\/+$/, "") || "/";
}

// External guides render a shim page (reachable by direct link or search); map each one's
// permalink to its external_url so search results can redirect straight to the article.
export function useExternalGuideUrls(): Map<string, string> {
    const data = usePluginData("recent-guides-plugin") as GuidesData | undefined;
    return useMemo(() => {
        const map = new Map<string, string>();
        for (const guide of data?.guides ?? []) {
            if (guide.permalink && guide.external_url) {
                map.set(normalizePath(guide.permalink), guide.external_url);
            }
        }
        return map;
    }, [data]);
}

export function resolveExternalGuideUrl(map: Map<string, string>, url: string | undefined | null): string | null {
    return url ? (map.get(normalizePath(url)) ?? null) : null;
}
