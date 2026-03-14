import React, { type ReactNode } from "react";
import Head from "@docusaurus/Head";
import authorsData from "@site/docs/authors.json";

/** Pre-validated guide frontmatter — all required fields are non-optional. */
export interface GuideMetadataProps {
    title: string;
    description: string;
    proficiencyLevel: string;
    canonicalUrl: string;
    ogImageUrl: string;
    authorKey?: string;
    publishedAt?: string;
    keywords?: string[];
    lastUpdatedAt?: number;
}

export default function GuideMetadata({
    title,
    description,
    proficiencyLevel,
    canonicalUrl,
    ogImageUrl,
    authorKey,
    publishedAt,
    keywords,
    lastUpdatedAt,
}: GuideMetadataProps): ReactNode {
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
            ? { dateModified: new Date(lastUpdatedAt * 1000).toISOString().split("T")[0] }
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
        proficiencyLevel,
    });

    const breadcrumbJsonLd = JSON.stringify({
        "@context": "https://schema.org",
        "@type": "BreadcrumbList",
        itemListElement: [
            {
                "@type": "ListItem",
                position: 1,
                name: "Guides",
                item: `${canonicalUrl.split("/guides/")[0]}/guides/`,
            },
            {
                "@type": "ListItem",
                position: 2,
                name: title,
                item: canonicalUrl,
            },
        ],
    });

    return (
        <Head>
            <script type="application/ld+json">{techArticleJsonLd}</script>
            <script type="application/ld+json">{breadcrumbJsonLd}</script>
        </Head>
    );
}
