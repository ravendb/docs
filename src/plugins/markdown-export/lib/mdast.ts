export type JsxAttribute = {
    type: string;
    name?: string;
    value?: string | { value?: string; data?: { estree?: unknown } } | null;
};

export interface MdNode {
    type: string;
    children?: MdNode[];
    value?: string;
    depth?: number;
    name?: string | null;
    attributes?: JsxAttribute[];
    url?: string;
    title?: string | null;
    lang?: string | null;
    meta?: string | null;
    alt?: string;
    ordered?: boolean;
    spread?: boolean;
}

export const text = (value: string): MdNode => ({ type: "text", value });
export const html = (value: string): MdNode => ({ type: "html", value });
export const paragraph = (...children: MdNode[]): MdNode => ({ type: "paragraph", children });
export const strong = (...children: MdNode[]): MdNode => ({ type: "strong", children });
export const emphasis = (...children: MdNode[]): MdNode => ({ type: "emphasis", children });
export const blockquote = (...children: MdNode[]): MdNode => ({ type: "blockquote", children });
export const heading = (depth: number, ...children: MdNode[]): MdNode => ({ type: "heading", depth, children });
export const link = (url: string, ...children: MdNode[]): MdNode => ({ type: "link", url, title: null, children });
export const image = (url: string, alt: string): MdNode => ({ type: "image", url, alt, title: null });
export const listItem = (...children: MdNode[]): MdNode => ({ type: "listItem", spread: false, children });
export const list = (items: MdNode[], ordered = false): MdNode => ({
    type: "list",
    ordered,
    spread: false,
    children: items,
});

export function descendants(nodes: MdNode[] | undefined, out: MdNode[] = []): MdNode[] {
    for (const node of nodes ?? []) {
        out.push(node);
        descendants(node.children, out);
    }
    return out;
}

export function isBlank(node: MdNode): boolean {
    if (node.type === "text") {
        return !String(node.value).trim();
    }
    return node.type === "paragraph" && (node.children ?? []).every(isBlank);
}

/** A leading `---` would read as front matter. */
export function dropLeadingRule(nodes: MdNode[]): MdNode[] {
    return nodes.length && nodes[0].type === "thematicBreak" ? nodes.slice(1) : nodes;
}

export function wrapPhrasing(nodes: MdNode[]): MdNode[] {
    return nodes.flatMap((n) => (n.type === "paragraph" ? (n.children ?? []) : [n]));
}

const PHRASING = new Set(["text", "inlineCode", "strong", "emphasis", "link", "break", "image", "html", "delete"]);

export function wrapFlow(nodes: MdNode[]): MdNode[] {
    const out: MdNode[] = [];
    let run: MdNode[] = [];
    for (const node of nodes) {
        if (PHRASING.has(node.type)) {
            run.push(node);
            continue;
        }
        if (run.length) {
            out.push(paragraph(...run));
            run = [];
        }
        out.push(node);
    }
    if (run.length) {
        out.push(paragraph(...run));
    }
    return out;
}

function attribute(node: MdNode, name: string): JsxAttribute | undefined {
    return (node.attributes ?? []).find((a) => a.type === "mdxJsxAttribute" && a.name === name);
}

export function attrString(node: MdNode, name: string): string | null | undefined {
    const a = attribute(node, name);
    if (!a) {
        return undefined;
    }
    if (a.value === null || a.value === undefined) {
        return null;
    }
    return typeof a.value === "string" ? a.value : undefined;
}

export function attrExpression(node: MdNode, name: string): string | undefined {
    const a = attribute(node, name);
    return a && typeof a.value === "object" && a.value ? a.value.value : undefined;
}

export function normaliseText(s: string): string {
    return s
        .replace(/\u00a0/g, " ")
        .replace(/[ \t]+/g, " ")
        .trim();
}
