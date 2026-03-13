import React, { type ReactNode } from "react";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import Head from "@docusaurus/Head";
import authorsData from "@site/docs/authors.json";

interface GuideMetadataProps {
    canonicalUrl: string;
    ogImageUrl: string;
    permalink: string;
}

export default function GuideMetadata({ canonicalUrl, ogImageUrl, permalink }: GuideMetadataProps): ReactNode {
    const { metadata, frontMatter } = useDoc();

    if (!metadata.title) {
        throw new Error(`Guide "${permalink}" is missing a required "title" in frontmatter.`);
    }

    if (!frontMatter.proficiencyLevel) {
        throw new Error(`Guide "${permalink}" is missing a required "proficiencyLevel" in frontmatter.`);
    }

    const title = metadata.title;
    const description = metadata.description || frontMatter.description || "";
    const authorKey = frontMatter.author;
    const authorInfo = authorKey ? authorsData[authorKey as keyof typeof authorsData] : null;
    const publishedAt = frontMatter.publishedAt;
    const keywords = frontMatter.keywords;
    const proficiencyLevel = frontMatter.proficiencyLevel;

    const techArticleJsonLd = JSON.stringify({
        "@context": "https://schema.org",
        "@type": "TechArticle",
        headline: title,
        description: description,
        url: canonicalUrl,
        mainEntityOfPage: { "@type": "WebPage", "@id": canonicalUrl },
        image: { "@type": "ImageObject", url: ogImageUrl, width: 1200, height: 630 },
        ...(publishedAt ? { datePublished: publishedAt } : {}),
        ...(metadata.lastUpdatedAt
            ? { dateModified: new Date(metadata.lastUpdatedAt * 1000).toISOString().split("T")[0] }
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
