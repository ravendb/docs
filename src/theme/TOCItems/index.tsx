import React, { useEffect, useMemo, type ReactNode } from "react";
import { useThemeConfig } from "@docusaurus/theme-common";
import { useFilteredAndTreeifiedTOC } from "@docusaurus/theme-common/internal";
import TOCItemTree from "@theme/TOCItems/Tree";
import type { Props } from "@theme/TOCItems";

import { activeAnchorId, type AnchorBox } from "../../typescript/tocHighlight";

interface HighlightConfig {
    linkClassName: string;
    linkActiveClassName: string;
    minHeadingLevel: number;
    maxHeadingLevel: number;
}

/** An anchor with no height is only a marker; its parent holds the heading text. */
function visibleRect(element: Element): DOMRect {
    const rect = element.getBoundingClientRect();
    return rect.top === rect.bottom && element.parentElement ? visibleRect(element.parentElement) : rect;
}

function anchorBoxes({ minHeadingLevel, maxHeadingLevel }: HighlightConfig): AnchorBox[] {
    const selectors: string[] = [];
    for (let level = minHeadingLevel; level <= maxHeadingLevel; level += 1) {
        selectors.push(`h${level}.anchor`);
    }
    return Array.from(document.querySelectorAll(selectors.join())).map((anchor) => {
        const rect = visibleRect(anchor);
        return { id: anchor.id, top: rect.top, bottom: rect.bottom };
    });
}

const linkAnchorId = (link: HTMLAnchorElement) => decodeURIComponent(link.href.substring(link.href.indexOf("#") + 1));

/**
 * Docusaurus' useTOCHighlight with one more rule (see activeAnchorId): at the bottom of the page the clicked heading,
 * or the last one in view, is highlighted, so a short last section gets its link highlighted too.
 */
function useTOCHighlight(config: HighlightConfig | undefined): void {
    const { navbar } = useThemeConfig();

    useEffect(() => {
        if (!config) {
            return undefined;
        }
        const highlight = config;
        const navbarElement = document.querySelector<HTMLElement>(".navbar");
        const anchorTopOffset = navbar.hideOnScroll ? 0 : (navbarElement?.clientHeight ?? 0);

        function update() {
            const root = document.documentElement;
            const activeId = activeAnchorId(anchorBoxes(highlight), {
                anchorTopOffset,
                height: window.innerHeight,
                atBottom: window.scrollY + window.innerHeight >= root.scrollHeight - 1,
                hashId: window.location.hash ? decodeURIComponent(window.location.hash.slice(1)) : null,
            });
            const links = Array.from(document.getElementsByClassName(highlight.linkClassName)) as HTMLAnchorElement[];
            for (const link of links) {
                link.classList.toggle(
                    highlight.linkActiveClassName,
                    activeId !== null && linkAnchorId(link) === activeId
                );
            }
        }

        document.addEventListener("scroll", update);
        window.addEventListener("resize", update);
        // Clicking a heading that is already in view at the bottom of the page changes the hash without scrolling.
        window.addEventListener("hashchange", update);
        update();
        return () => {
            document.removeEventListener("scroll", update);
            window.removeEventListener("resize", update);
            window.removeEventListener("hashchange", update);
        };
    }, [config, navbar.hideOnScroll]);
}

export default function TOCItems({
    toc,
    className = "table-of-contents table-of-contents__left-border",
    linkClassName = "table-of-contents__link",
    linkActiveClassName = undefined,
    minHeadingLevel: minHeadingLevelOption,
    maxHeadingLevel: maxHeadingLevelOption,
    ...props
}: Props): ReactNode {
    const themeConfig = useThemeConfig();
    const minHeadingLevel = minHeadingLevelOption ?? themeConfig.tableOfContents.minHeadingLevel;
    const maxHeadingLevel = maxHeadingLevelOption ?? themeConfig.tableOfContents.maxHeadingLevel;
    const tocTree = useFilteredAndTreeifiedTOC({ toc, minHeadingLevel, maxHeadingLevel });

    const highlightConfig = useMemo(
        () =>
            linkClassName && linkActiveClassName
                ? { linkClassName, linkActiveClassName, minHeadingLevel, maxHeadingLevel }
                : undefined,
        [linkClassName, linkActiveClassName, minHeadingLevel, maxHeadingLevel]
    );
    useTOCHighlight(highlightConfig);

    return <TOCItemTree toc={tocTree} className={className} linkClassName={linkClassName} {...props} />;
}
