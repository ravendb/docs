import type { CreateFeedItemsFn } from "@docusaurus/plugin-content-blog";
import path from "path";

export type ExcerptBlock =
    | { type: "heading"; text: string }
    | { type: "paragraph"; text: string }
    | { type: "feature"; name: string; text: string }
    | { type: "image"; url: string };

const truncateMarker = /<!--\s*truncate\s*-->|\{\/\*\s*truncate\s*\*\/\}/;
const markdownImage = /!\[[^\]]*\]\(\s*<?([^)\s>]+)[^)]*\)/g;
const boldOnly = /^(\*\*|__)([^*_]+?)\1$/;
const boldPrefix = /^(\*\*|__)(.+?)\1\s*:?\s*/;
const listItem = /^\s*[-*]\s+/;
const htmlImageSources = /<img[^>]+src="([^"]+)"/g;

// The Cloud Portal shows what the list page shows, so an item carries its excerpt as feature bullets, paragraphs and images, in order.
export const createWhatsNewFeedItems: CreateFeedItemsFn = async ({ defaultCreateFeedItems, ...params }) => {
    const items = await defaultCreateFeedItems(params);

    return items.map((item, index) => ({
        ...item,
        extensions: [
            { name: "_ravendb", objects: { excerpt: toExcerptBlocks(params.blogPosts[index].content, item.content) } },
        ],
    }));
};

export function toExcerptBlocks(source: string, contentHtml: string | undefined): ExcerptBlock[] {
    const [excerpt] = source.split(truncateMarker);
    const builtSources = [...(contentHtml ?? "").matchAll(htmlImageSources)].map((match) => match[1]);

    return excerpt.split(/\n\s*\n/).flatMap((chunk): ExcerptBlock[] => {
        if (isList(chunk)) {
            return toFeatureBlocks(chunk);
        }

        const images = [...chunk.matchAll(markdownImage)]
            .map((match) => toBuiltSource(match[1], builtSources))
            .filter((url): url is string => url !== undefined)
            .map((url): ExcerptBlock => ({ type: "image", url }));
        const markdown = chunk.replace(markdownImage, " ").trim();

        return [...toTextBlocks(markdown), ...images];
    });
}

function isList(chunk: string): boolean {
    const firstLine = chunk.split("\n").find((line) => line.trim().length > 0);
    return firstLine !== undefined && listItem.test(firstLine);
}

function toFeatureBlocks(chunk: string): ExcerptBlock[] {
    const items: string[] = [];
    for (const line of chunk.split("\n")) {
        const trimmed = line.trim();
        if (!trimmed) {
            continue;
        }
        if (listItem.test(trimmed)) {
            items.push(trimmed.replace(listItem, ""));
        } else if (items.length > 0) {
            items[items.length - 1] += ` ${trimmed}`;
        }
    }

    return items.map((item): ExcerptBlock => {
        const bold = item.match(boldPrefix);
        if (bold) {
            return {
                type: "feature",
                name: toPlainText(bold[2].replace(/:\s*$/, "")),
                text: toPlainText(item.slice(bold[0].length)),
            };
        }
        return { type: "feature", name: "", text: toPlainText(item) };
    });
}

function toTextBlocks(markdown: string): ExcerptBlock[] {
    const heading = markdown.match(boldOnly);
    if (heading) {
        return [{ type: "heading", text: toPlainText(heading[2]) }];
    }

    const text = toPlainText(markdown);
    return text ? [{ type: "paragraph", text }] : [];
}

function toPlainText(markdown: string): string {
    return markdown
        .replace(/\[([^\]]*)\]\([^)]*\)/g, "$1")
        .replace(/(\*\*|__|\*|_|`)(.+?)\1/g, "$2")
        .replace(/\s+/g, " ")
        .trim();
}

function toBuiltSource(imagePath: string, builtSources: string[]): string | undefined {
    if (/^https?:\/\//.test(imagePath)) {
        return imagePath;
    }

    const { name, ext } = path.parse(imagePath);
    const builtName = new RegExp(`^${name.replace(/[.*+?^${}()|[\]\\]/g, "\\$&")}-[0-9a-f]+\\${ext}$`);
    return builtSources.find((src) => builtName.test(path.posix.basename(new URL(src).pathname)));
}
