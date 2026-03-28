import React, { type ReactNode } from "react";
import Head from "@docusaurus/Head";
import authorsData from "@site/docs/authors.json";

/** Converts a Docusaurus lastUpdatedAt timestamp to YYYY-MM-DD. */
function toDateString(timestamp: number): string {
    // Docusaurus may provide seconds or milliseconds — normalize to ms
    const ms = timestamp > 1e12 ? timestamp : timestamp * 1000;
    return new Date(ms).toISOString().split("T")[0];
}

export interface DocPageMetadataProps {
    // Shared (required)
    title: string;
    description: string;
    canonicalUrl: string;
    ogImageUrl: string;
    // Shared (optional)
    lastUpdatedAt?: number;
    // Guide-only (optional)
    proficiencyLevel?: string;
    authorKey?: string;
    publishedAt?: string;
    keywords?: string[];
}

export default function DocPageMetadata({
    title,
    description,
    canonicalUrl,
    ogImageUrl,
    lastUpdatedAt,
    proficiencyLevel,
    authorKey,
    publishedAt,
    keywords,
}: DocPageMetadataProps): ReactNode {
    const authorInfo = authorKey ? authorsData[authorKey as keyof typeof authorsData] : null;

    const techArticleJsonLd = JSON.stringify({
        "@context": "https://schema.org",
        "@type": "TechArticle",
        headline: title,
        description,
        url: canonicalUrl,
        mainEntityOfPage: { "@type": "WebPage", "@id": canonicalUrl },
        image: { "@type": "ImageObject", url: ogImageUrl, width: 1200, height: 630 },
        ...(publishedAt ? { datePublished: publishedAt } : {}),
        ...(lastUpdatedAt
            ? { dateModified: toDateString(lastUpdatedAt) }
            : publishedAt
              ? { dateModified: publishedAt }
              : {}),
        inLanguage: "en",
        ...(keywords?.length ? { keywords } : {}),
        ...(authorInfo
            ? {
                  author: {
                      "@type": "Person",
                      name: authorInfo.name,
                      ...(authorInfo.url ? { url: authorInfo.url } : {}),
                      ...(authorInfo.job_title
                          ? {
                                jobTitle: authorInfo.job_title,
                                worksFor: {
                                    "@type": "Organization",
                                    name: "RavenDB",
                                    url: "https://ravendb.net",
                                },
                            }
                          : {}),
                  },
              }
            : {}),
        publisher: {
            "@type": "Organization",
            name: "RavenDB",
            url: "https://ravendb.net",
            logo: { "@type": "ImageObject", url: ogImageUrl },
        },
        ...(proficiencyLevel ? { proficiencyLevel } : {}),
        about: {
            "@type": "SoftwareApplication",
            name: "RavenDB",
            url: "https://ravendb.net/",
        },
        isPartOf: {
            "@type": "WebSite",
            name: "RavenDB Documentation",
            url: "https://docs.ravendb.net/",
        },
    });

    return (
        <Head>
            <script type="application/ld+json">{techArticleJsonLd}</script>
        </Head>
    );
}
