const UTM_SOURCE = "docs";
const UTM_MEDIUM = "samples";
const UTM_CAMPAIGN = "sample_apps";

export function withSamplesUtm(url: string | undefined, sampleName?: string): string | undefined {
    if (!url || !/^https?:\/\//i.test(url)) {
        return url;
    }

    try {
        const parsed = new URL(url);
        parsed.searchParams.set("utm_source", UTM_SOURCE);
        parsed.searchParams.set("utm_medium", UTM_MEDIUM);
        parsed.searchParams.set("utm_campaign", UTM_CAMPAIGN);
        if (sampleName) {
            parsed.searchParams.set("utm_content", sampleName);
        }
        return parsed.toString();
    } catch {
        return url;
    }
}

export function sampleSlugFromPermalink(permalink: string | undefined): string {
    if (!permalink) {
        return "";
    }
    return permalink
        .replace(/^\/+/, "")
        .replace(/^samples\/?/, "")
        .replace(/\/+$/, "");
}

type DataLayerPayload = Record<string, unknown> & { event: string };

function pushEvent(payload: DataLayerPayload): void {
    if (typeof window === "undefined") {
        return;
    }
    const target = window as unknown as { dataLayer?: unknown[] };
    target.dataLayer = target.dataLayer || [];
    target.dataLayer.push(payload);
}

export interface SampleCardClickParams {
    sample_name: string;
    tech_stack: string;
    card_position: number;
}

export function trackSampleCardClick(params: SampleCardClickParams): void {
    pushEvent({ event: "sample_card_click", ...params });
}

export interface SampleRepoClickParams {
    sample_name: string;
    tech_stack: string;
    destination_url: string;
}

export function trackSampleRepoClick(params: SampleRepoClickParams): void {
    pushEvent({ event: "sample_repo_click", ...params });
}

export interface FilterAppliedParams {
    filter_category: string;
    filter_value: string;
    match_logic: string;
}

export function trackFilterApplied(params: FilterAppliedParams): void {
    pushEvent({ event: "filter_applied", ...params });
}

export interface SampleOutboundClickParams {
    destination_url: string;
    link_text: string;
    sample_name?: string;
}

export function trackSampleOutboundClick(params: SampleOutboundClickParams): void {
    pushEvent({ event: "sample_outbound_click", ...params });
}
