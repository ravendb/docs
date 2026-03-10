import React, { type ReactNode } from "react";
import Metadata from "@theme-original/DocItem/Metadata";
import type MetadataType from "@theme/DocItem/Metadata";
import type { WrapperProps } from "@docusaurus/types";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import Head from "@docusaurus/Head";

type Props = WrapperProps<typeof MetadataType>;

export default function MetadataWrapper(props: Props): ReactNode {
    const { siteConfig } = useDocusaurusContext();
    const { metadata } = useDoc();
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

    return (
        <>
            <Head>
                <link rel="canonical" href={canonicalUrl} />
            </Head>
            <Metadata {...props} />
        </>
    );
}
