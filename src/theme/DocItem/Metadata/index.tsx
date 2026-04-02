import React, { type ReactNode } from "react";
import Metadata from "@theme-original/DocItem/Metadata";
import type MetadataType from "@theme/DocItem/Metadata";
import type { WrapperProps } from "@docusaurus/types";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import Head from "@docusaurus/Head";
import DocPageMetadata from "./DocPageMetadata";

type Props = WrapperProps<typeof MetadataType>;

export default function MetadataWrapper(props: Props): ReactNode {
    const { siteConfig } = useDocusaurusContext();
    const { metadata, frontMatter } = useDoc();
    const permalink = metadata.permalink;
    const source = (metadata as Record<string, unknown>).source as string | undefined;

    // Use file path (source) for page type detection — more reliable than permalink
    // which may vary depending on Docusaurus trailingSlash configuration.
    const isGuide = source?.startsWith("@site/guides/") || source?.startsWith("guides/") || false;
    const isCloud = source?.startsWith("@site/cloud/") || source?.startsWith("cloud/") || false;
    const isTemplate = source?.startsWith("@site/templates/") || source?.startsWith("templates/") || false;
    const isDocumentationPage = !isGuide && !isCloud && !isTemplate;

    // Exclude landing pages (e.g. guides/home.mdx) from guide-specific metadata
    const fileName = source?.split("/").pop();
    const isGuidePage = isGuide && fileName !== "home.mdx";

    // Strip trailing slash from base URL to avoid double slashes
    const baseUrl = (siteConfig.url as string).replace(/\/$/, "");
    const canonicalUrl = isDocumentationPage
        ? `${baseUrl}/${siteConfig.customFields.latestVersion}${metadata.slug}`
        : `${baseUrl}${permalink}`;

    const isHomepage = metadata.slug === "/";
    const description = metadata.description || frontMatter.description || "";
    const title = metadata.title || "";
    const ogImageUrl = `${baseUrl}/img/social-card.jpg`;

    return (
        <>
            <Head>
                <link rel="canonical" href={canonicalUrl} />
                {description && <meta property="og:description" content={description} />}
                {title && <meta property="og:title" content={title} />}
                <meta property="og:type" content={isHomepage ? "website" : "article"} />
                <meta property="og:url" content={canonicalUrl} />
                <meta property="og:image" content={ogImageUrl} />
                <meta property="og:image:alt" content={title || "RavenDB Documentation"} />
                <meta property="og:site_name" content="RavenDB Documentation" />
                <meta name="twitter:card" content="summary_large_image" />
                {title && <meta name="twitter:title" content={title} />}
                {description && <meta name="twitter:description" content={description} />}
                <meta name="twitter:image" content={ogImageUrl} />
                <meta name="twitter:site" content="@RavenDB" />
            </Head>
            {isGuidePage && (
                <ValidatedGuideDocPageMetadata
                    title={title}
                    description={description}
                    lastUpdatedAt={metadata.lastUpdatedAt}
                    frontMatter={frontMatter}
                    canonicalUrl={canonicalUrl}
                    ogImageUrl={ogImageUrl}
                    permalink={permalink}
                />
            )}
            {(isDocumentationPage || isCloud) && !isHomepage && (
                <DocPageMetadata
                    title={title}
                    description={description}
                    canonicalUrl={canonicalUrl}
                    ogImageUrl={ogImageUrl}
                    lastUpdatedAt={metadata.lastUpdatedAt}
                />
            )}
            <Metadata {...props} />
        </>
    );
}

/**
 * Validates required guide frontmatter fields at build time and passes
 * pre-validated data to DocPageMetadata so it doesn't deal with optional types.
 */
function ValidatedGuideDocPageMetadata({
    title,
    description,
    lastUpdatedAt,
    frontMatter,
    canonicalUrl,
    ogImageUrl,
    permalink,
}: {
    title: string;
    description: string;
    lastUpdatedAt?: number;
    frontMatter: ReturnType<typeof useDoc>["frontMatter"];
    canonicalUrl: string;
    ogImageUrl: string;
    permalink: string;
}): ReactNode {
    if (!title) {
        throw new Error(`Guide "${permalink}" is missing a required "title" in frontmatter.`);
    }
    if (!frontMatter.proficiencyLevel) {
        throw new Error(`Guide "${permalink}" is missing a required "proficiencyLevel" in frontmatter.`);
    }

    return (
        <DocPageMetadata
            title={title}
            description={description || (frontMatter.description as string) || ""}
            proficiencyLevel={frontMatter.proficiencyLevel}
            canonicalUrl={canonicalUrl}
            ogImageUrl={ogImageUrl}
            authorKey={frontMatter.author}
            publishedAt={frontMatter.publishedAt}
            keywords={frontMatter.keywords}
            lastUpdatedAt={lastUpdatedAt}
        />
    );
}
