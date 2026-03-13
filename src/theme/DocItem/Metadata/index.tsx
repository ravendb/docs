import React, { type ReactNode } from "react";
import Metadata from "@theme-original/DocItem/Metadata";
import type MetadataType from "@theme/DocItem/Metadata";
import type { WrapperProps } from "@docusaurus/types";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import Head from "@docusaurus/Head";
import GuideMetadata from "./GuideMetadata";

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

    const guidesLandingPaths = ["/guides", "/guides/"];
    const isGuide = permalink.startsWith("/guides/") && !guidesLandingPaths.includes(permalink);
    const description = metadata.description || frontMatter.description || "";
    const title = metadata.title || "";
    const ogImageUrl = `${baseUrl}/img/social-card.jpg`;

    return (
        <>
            <Head>
                <link rel="canonical" href={canonicalUrl} />
                {description && <meta property="og:description" content={description} />}
                {title && <meta property="og:title" content={title} />}
                <meta property="og:type" content="article" />
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
            {isGuide && <GuideMetadata canonicalUrl={canonicalUrl} ogImageUrl={ogImageUrl} permalink={permalink} />}
            <Metadata {...props} />
        </>
    );
}
