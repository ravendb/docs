import { TOCItem } from "@docusaurus/mdx-loader/lib/remark/toc/types";
import { useLanguage } from "@site/src/components/LanguageStore";
import { useEffect, useState } from "react";

// By default, Markdown headings within hideable areas are added to the TOC.
// With our current language switching implementation, this leads to default TOC being filled with headings from
// *all* languages, instead of only currently selected one. To tackle this, we filter out TOC ourselves.
//
// As per https://github.com/facebook/docusaurus/issues/3915#issuecomment-2052403930,
// the TOC is computed statically at MDX compilation time, which means we can't do this via React.
//
// Docusaurus team has an open issue for this problem:
// https://github.com/facebook/docusaurus/issues/6201

export default function useFilteredToc(originalToc: readonly TOCItem[]): readonly TOCItem[] {
    const { language } = useLanguage();
    const [filteredToc, setFilteredToc] = useState<readonly TOCItem[]>([]);

    useEffect(() => {
        setFilteredToc(getFilteredToc(originalToc));
    }, [language, originalToc]);

    return filteredToc;
}

function getFilteredToc(originalToc: readonly TOCItem[]): readonly TOCItem[] {
    const uniqueIds = new Set(originalToc.map((item) => item.id));

    const markdownEl = document.querySelector(".theme-doc-markdown");
    if (!markdownEl) {
        return originalToc;
    }

    const filteredToc: TOCItem[] = [];

    uniqueIds.forEach((id) => {
        // eslint-disable-next-line no-undef
        const headingEl = markdownEl.querySelector(`#${CSS.escape(id)}`);

        if (headingEl) {
            filteredToc.push(originalToc.find((item) => item.id === id));
        }
    });

    return filteredToc;
}
