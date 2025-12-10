import React, { useEffect, type ReactNode } from "react";
import TOC from "@theme-original/TOC";
import type TOCType from "@theme/TOC";
import type { WrapperProps } from "@docusaurus/types";
import { TOCItem } from "@docusaurus/mdx-loader/lib/remark/toc/types";
import { useLanguage } from "@site/src/components/LanguageStore";

type Props = WrapperProps<typeof TOCType>;

export default function TOCWrapper(props: Props): ReactNode {
    const { language } = useLanguage();
    const [toc, setToc] = React.useState<readonly TOCItem[]>([]);

    useEffect(() => {
        const filteredToc = getFilteredToc(props.toc);
        setToc(filteredToc);
    }, [language]);

    return (
        <div className="sticky top-[160px]">
            <h5 className="!mb-1">In this article</h5>
            <TOC {...props} toc={toc} />
        </div>
    );
}

// By default, Markdown headings within hideable areas are added to the TOC.
// With our current language switching implementation, this leads to default TOC being filled with headings from
// *all* languages, instead of only currently selected one. To tackle this, we filter out TOC ourselves.
//
// As per https://github.com/facebook/docusaurus/issues/3915#issuecomment-2052403930,
// the TOC is computed statically at MDX compilation time, which means we can't do this via React.
//
// Docusaurus team has an open issue for this problem:
// https://github.com/facebook/docusaurus/issues/6201
function getFilteredToc(originalToc: readonly TOCItem[]): readonly TOCItem[] {
    const uniqueIds = new Set(originalToc.map((item) => item.id));

    const markdownEl = document.querySelector(".theme-doc-markdown");
    if (!markdownEl) {
        return originalToc;
    }

    const filteredToc: TOCItem[] = [];

    uniqueIds.forEach((id) => {
        const headingEl = markdownEl.querySelector(`#${id}`);

        if (headingEl) {
            filteredToc.push(originalToc.find((item) => item.id === id));
        }
    });

    return filteredToc;
}
