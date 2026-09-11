import assert from "node:assert/strict";
import { describe, it } from "node:test";
import remarkPanelHeadings, { type MdastNode } from "../index";

type Attribute = NonNullable<MdastNode["attributes"]>[number];

function attribute(name: string, value: Attribute["value"] = null): Attribute {
    return { type: "mdxJsxAttribute", name, value };
}

function expression(source: string): Attribute["value"] {
    return { value: source };
}

function panel(attributes: Attribute[], children: MdastNode[] = []): MdastNode {
    return { type: "mdxJsxFlowElement", name: "Panel", attributes, children };
}

function paragraph(text: string): MdastNode {
    return { type: "paragraph", children: [{ type: "text", value: text }] };
}

function runPlugin(...children: MdastNode[]): MdastNode {
    const root: MdastNode = { type: "root", children };
    remarkPanelHeadings()(root, { path: "test.mdx" });
    return root;
}

function captureWarnings(run: () => void): string[] {
    const warnings: string[] = [];
    const originalWarn = console.warn;
    console.warn = (message: string) => warnings.push(message);
    try {
        run();
    } finally {
        console.warn = originalWarn;
    }
    return warnings;
}

describe("remark-panel-headings", () => {
    it("hoists the heading prop into a Markdown heading with the legacy anchor id", () => {
        const root = runPlugin(panel([attribute("heading", "Sample data")], [paragraph("body")]));

        const [hoisted] = root.children!;
        assert.deepEqual(hoisted.attributes, [attribute("headingInContent")]);
        assert.deepEqual(hoisted.children, [
            {
                type: "heading",
                depth: 2,
                children: [{ type: "text", value: "Sample data" }],
                data: { hProperties: { id: "sample-data", className: "panel__heading" } },
            },
            paragraph("body"),
        ]);
    });

    it("keeps ids that github-slugger would rewrite", () => {
        const root = runPlugin(panel([attribute("heading", "Define the connection string - from Studio")]));

        assert.equal(root.children![0].children![0].data!.hProperties!.id, "define-the-connection-string-from-studio");
    });

    it("honours headingLevel, written as an expression or as a string", () => {
        const root = runPlugin(
            panel([attribute("heading", "Expression"), attribute("headingLevel", expression("3"))]),
            panel([attribute("heading", "String"), attribute("headingLevel", "4")]),
            panel([attribute("heading", "Out of range"), attribute("headingLevel", "9")])
        );

        assert.deepEqual(
            root.children!.map((node) => node.children![0].depth),
            [3, 4, 2]
        );
    });

    it("hoists nested panels and leaves other components alone", () => {
        const root = runPlugin(
            {
                type: "mdxJsxFlowElement",
                name: "Admonition",
                attributes: [],
                children: [panel([attribute("heading", "Nested")])],
            },
            {
                type: "mdxJsxFlowElement",
                name: "ContentFrame",
                attributes: [attribute("heading", "Not a panel")],
                children: [],
            }
        );

        assert.equal(root.children![0].children![0].children![0].data!.hProperties!.id, "nested");
        assert.deepEqual(root.children![1].children, []);
        assert.deepEqual(root.children![1].attributes, [attribute("heading", "Not a panel")]);
    });

    it("warns and leaves the panel alone when the heading cannot become an anchor", () => {
        let root: MdastNode;
        const warnings = captureWarnings(() => {
            root = runPlugin(
                panel([attribute("heading", expression("title"))]),
                panel([attribute("heading", "   ")]),
                panel([attribute("heading", "***")])
            );
        });

        assert.equal(warnings.length, 3);
        assert.match(warnings[0], /test\.mdx/);
        for (const node of root!.children!) {
            assert.deepEqual(node.children, []);
            assert.equal(
                node.attributes!.some(({ name }) => name === "headingInContent"),
                false
            );
        }
    });

    it("leaves a panel without a heading untouched", () => {
        const root = runPlugin(panel([attribute("flush")], [paragraph("body")]));

        assert.deepEqual(root.children![0].attributes, [attribute("flush")]);
        assert.deepEqual(root.children![0].children, [paragraph("body")]);
    });

    it("is idempotent", () => {
        const root = runPlugin(panel([attribute("heading", "Syntax")], [paragraph("body")]));
        remarkPanelHeadings()(root, { path: "test.mdx" });

        assert.equal(root.children![0].children!.length, 2);
        assert.deepEqual(root.children![0].attributes, [attribute("headingInContent")]);
    });
});
