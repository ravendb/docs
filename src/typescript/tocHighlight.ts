/** A heading the table of contents links to, with its position in the viewport (px). */
export interface AnchorBox {
    id: string;
    top: number;
    bottom: number;
}

export interface TocViewport {
    /** Height of the sticky navbar: a heading above this line has been scrolled past. */
    anchorTopOffset: number;
    height: number;
    /** The page cannot scroll any further down. */
    atBottom: boolean;
    /** The heading id in the URL hash, set when a table of contents link was clicked. */
    hashId: string | null;
}

/**
 * The heading whose table of contents link is highlighted.
 *
 * Docusaurus' rule: the first heading below the navbar is active once it reaches the top half of the viewport, and
 * until then the one before it is. A short last section can never get its heading that high, so clicking it in the
 * table of contents left the previous one highlighted. At the bottom of the page the clicked heading wins, or else
 * the last heading in view.
 */
export function activeAnchorId(anchors: AnchorBox[], viewport: TocViewport): string | null {
    if (viewport.atBottom) {
        const inView = anchors.filter(
            (anchor) => anchor.top >= viewport.anchorTopOffset && anchor.top < viewport.height
        );
        if (inView.length > 0) {
            return (inView.find((anchor) => anchor.id === viewport.hashId) ?? inView[inView.length - 1]).id;
        }
    }

    const next = anchors.find((anchor) => anchor.top >= viewport.anchorTopOffset);
    if (next) {
        if (next.top > 0 && next.bottom < viewport.height / 2) {
            return next.id;
        }
        return anchors[anchors.indexOf(next) - 1]?.id ?? null;
    }
    return anchors[anchors.length - 1]?.id ?? null;
}
