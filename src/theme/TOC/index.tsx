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

    return <TOC {...props} toc={toc} />;
}

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
