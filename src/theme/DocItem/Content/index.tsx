import React, { type ReactNode } from "react";
import clsx from "clsx";
import { ThemeClassNames } from "@docusaurus/theme-common";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import Heading from "@theme/Heading";
import MDXContent from "@theme/MDXContent";
import type { Props } from "@theme/DocItem/Content";
import DocItemAuthors from "@site/src/theme/DocItem/Authors";
import BannerImage, { BannerImageProps } from "../BannerImage";
import Tag from "@site/src/theme/Tag";

export { BannerImage };
export type { BannerImageProps };

/**
 Title can be declared inside md content or declared through
 front matter and added manually. To make both cases consistent,
 the added title is added under the same div.markdown block
 See https://github.com/facebook/docusaurus/pull/4882#issuecomment-853021120

 We render a "synthetic title" if:
 - user doesn't ask to hide it with front matter
 - the markdown content does not already contain a top-level h1 heading
*/
function useSyntheticTitle(): string | null {
    const { metadata, frontMatter, contentTitle } = useDoc();
    const shouldRender = !frontMatter.hide_title && typeof contentTitle === "undefined";
    if (!shouldRender) {
        return null;
    }
    return metadata.title;
}

export default function DocItemContent({ children }: Props): ReactNode {
    const syntheticTitle = useSyntheticTitle();
    const { metadata } = useDoc();
    const { tags } = metadata;
    const canDisplayTagsRow = tags.length > 0;
    const isGuide = metadata.source?.includes("/guides/");

    return (
        <div className={clsx(ThemeClassNames.docs.docMarkdown, "markdown")}>
            {syntheticTitle && (
                <header>
                    <Heading as="h1">{syntheticTitle}</Heading>
                    {isGuide && (
                        <>
                            <DocItemAuthors />
                            <BannerImage />
                            {canDisplayTagsRow && (
                                <div className="flex flex-wrap gap-2 mb-8">
                                    <strong>Tags:</strong>
                                    {tags.map((tag) => (
                                        <Tag key={tag.label} permalink={tag.permalink}>
                                            {tag.label}
                                        </Tag>
                                    ))}
                                </div>
                            )}
                        </>
                    )}
                </header>
            )}
            <MDXContent>{children}</MDXContent>
        </div>
    );
}
