import React, { type ReactNode } from "react";
import { ThemeClassNames } from "@docusaurus/theme-common";
import { useDoc } from "@docusaurus/plugin-content-docs/client";
import TOC from "@theme/TOC";
import useFilteredToc from "@site/src/theme/DocItem/TOC/useFilteredToc";

export default function DocItemTOCDesktop(): ReactNode {
    const { toc, frontMatter } = useDoc();
    const filteredToc = useFilteredToc(toc);

    return (
        <div className="sticky top-[160px]">
            <h5 className="!mb-1">In this article</h5>
            <TOC
                toc={filteredToc}
                minHeadingLevel={frontMatter.toc_min_heading_level}
                maxHeadingLevel={frontMatter.toc_max_heading_level}
                className={ThemeClassNames.docs.docTocDesktop}
            />
        </div>
    );
}
