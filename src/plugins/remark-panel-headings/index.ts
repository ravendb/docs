// Docusaurus builds the TOC at MDX compile time from Markdown heading nodes only, never from JSX
// props, so <Panel heading="..."> never reached it (RDoc-3631). Registered as a
// beforeDefaultRemarkPlugins entry, so the default headings/toc plugins see an ordinary heading.
// Hand-rolled walk: unist-util-visit is only a transitive Docusaurus dependency.

type MdxAttributeExpression = { value?: string };

type JsxAttribute = {
    type: string;
    name?: string;
    value?: string | MdxAttributeExpression | null;
};

export type MdastNode = {
    type: string;
    name?: string | null;
    attributes?: JsxAttribute[];
    children?: MdastNode[];
    depth?: number;
    value?: string;
    data?: { hProperties?: Record<string, unknown> };
};

const DEFAULT_HEADING_DEPTH = 2;

export default function remarkPanelHeadings() {
    return (root: MdastNode, file?: { path?: string }) => {
        const filePath = file?.path ?? "unknown file";
        forEachPanel(root, (panel) => hoistHeadingIntoContent(panel, filePath));
    };
}

function forEachPanel(node: MdastNode, visitPanel: (panel: MdastNode) => void): void {
    for (const child of node.children ?? []) {
        if (child.type === "mdxJsxFlowElement" && child.name === "Panel") {
            visitPanel(child);
        }
        forEachPanel(child, visitPanel);
    }
}

function hoistHeadingIntoContent(panel: MdastNode, filePath: string): void {
    const attributes = panel.attributes ?? [];
    const headingAttribute = attributes.find(({ name }) => name === "heading");
    if (!headingAttribute) {
        return;
    }

    const text = plainString(headingAttribute.value).trim();
    const anchorId = panelAnchorId(text);
    if (!anchorId) {
        console.warn(
            `[remark-panel-headings] ${filePath}: a <Panel> heading is not a plain string that can become an anchor, so it will neither render nor reach the TOC.`
        );
        return;
    }

    panel.attributes = [
        ...attributes.filter((attribute) => attribute !== headingAttribute),
        { type: "mdxJsxAttribute", name: "headingInContent", value: null },
    ];
    panel.children = [markdownHeading(text, anchorId, headingDepth(attributes)), ...(panel.children ?? [])];
}

function markdownHeading(text: string, anchorId: string, depth: number): MdastNode {
    return {
        type: "heading",
        depth,
        children: [{ type: "text", value: text }],
        data: { hProperties: { id: anchorId, className: "panel__heading" } },
    };
}

// Identical to the id the component used to compute at render time, so that live anchors keep
// working: github-slugger, which Docusaurus would otherwise apply, rewrites 175 of them.
function panelAnchorId(heading: string): string {
    return heading
        .toLowerCase()
        .replace(/[^\w]+/g, "-")
        .replace(/^-|-$/g, "");
}

function headingDepth(attributes: JsxAttribute[]): number {
    const value = attributes.find(({ name }) => name === "headingLevel")?.value;
    const depth = Number.parseInt(plainString(value) || expressionSource(value), 10);
    return depth >= 1 && depth <= 6 ? depth : DEFAULT_HEADING_DEPTH;
}

function plainString(value: JsxAttribute["value"]): string {
    return typeof value === "string" ? value : "";
}

function expressionSource(value: JsxAttribute["value"]): string {
    return typeof value === "string" ? "" : (value?.value ?? "");
}
