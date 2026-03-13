import React, { type ReactNode } from "react";
import Metadata from "@theme-original/DocItem/Metadata";
import type MetadataType from "@theme/DocItem/Metadata";
import type { WrapperProps } from "@docusaurus/types";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import Head from "@docusaurus/Head";
import authorsData from "@site/docs/authors.json";

type Props = WrapperProps<typeof MetadataType>;

export default function MetadataWrapper(props: Props): ReactNode {
    const { siteConfig } = useDocusaurusContext();
    const { metadata, frontMatter } = useDoc();
    const permalink = metadata.permalink;
    // Strip trailing slash from base URL to avoid double slashes
    const baseUrl = (siteConfig.url as string).replace(/\/$/, "");
    const isVersionedDoc =
        !permalink.startsWith("/guides/") && !permalink.startsWith("/cloud/") && !permalink.startsWith("/templates/");
    let canonicalUrl = isVersionedDoc
        ? `${baseUrl}/${siteConfig.customFields.latestVersion}${metadata.slug}`
        : `${baseUrl}${permalink}`;

    if (canonicalUrl.endsWith("/") == false) {
        canonicalUrl = canonicalUrl.concat("/");
    }

    const isGuide = permalink.startsWith("/guides/") && !permalink.endsWith("/guides/");
    const description = metadata.description || (frontMatter.description as string) || "";
    const title = metadata.title || "";

    const authorKey = frontMatter.author as string | undefined;
    const authorInfo = authorKey ? authorsData[authorKey as keyof typeof authorsData] : null;
    const publishedAt = frontMatter.publishedAt as string | undefined;
    const keywords = frontMatter.keywords as string[] | undefined;

    const ogImageUrl = `${baseUrl}/img/social-card.jpg`;

    const techArticleJsonLd =
        isGuide && title
            ? JSON.stringify({
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
                                ...(authorInfo.title
                                    ? {
                                          jobTitle: authorInfo.title.replace(/ @ .*$/, ""),
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
                  proficiencyLevel: "Intermediate",
              })
            : null;

    const breadcrumbJsonLd = isGuide
        ? JSON.stringify({
              "@context": "https://schema.org",
              "@type": "BreadcrumbList",
              itemListElement: [
                  {
                      "@type": "ListItem",
                      position: 1,
                      name: "Guides",
                      item: `${baseUrl}/guides/`,
                  },
                  {
                      "@type": "ListItem",
                      position: 2,
                      name: title,
                      item: canonicalUrl,
                  },
              ],
          })
        : null;

    return (
        <>
            <Head>
                <link rel="canonical" href={canonicalUrl} />
                {description && <meta property="og:description" content={description} />}
                {title && <meta property="og:title" content={title} />}
                <meta property="og:type" content="article" />
                <meta property="og:url" content={canonicalUrl} />
                <meta property="og:image" content={ogImageUrl} />
                <meta name="twitter:card" content="summary_large_image" />
                {title && <meta name="twitter:title" content={title} />}
                {description && <meta name="twitter:description" content={description} />}
                <meta name="twitter:image" content={ogImageUrl} />
                {techArticleJsonLd && (
                    <script type="application/ld+json">{techArticleJsonLd}</script>
                )}
                {breadcrumbJsonLd && (
                    <script type="application/ld+json">{breadcrumbJsonLd}</script>
                )}
            </Head>
            <Metadata {...props} />
        </>
    );
}
