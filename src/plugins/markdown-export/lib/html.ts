import { escapeHtml } from "../../agent-hint-plugin/lib/hint.js";

export interface AlternateLink {
    url: string;
    label: string | null;
}

export function alternateTags(links: AlternateLink[]): string {
    return links
        .map(
            (l) =>
                `<link rel="alternate" type="text/markdown" href="${escapeHtml(l.url)}"` +
                (l.label ? ` title="${escapeHtml(l.label)}"` : "") +
                ">"
        )
        .join("");
}

/** The hint is found by its data attribute, which survives minification. */
export function addMarkdownLinks(html: string, alternates: string, hintLine: string): { html: string; hint: boolean } {
    if (html.includes('type="text/markdown"')) {
        return { html, hint: html.includes("available as Markdown") };
    }
    const head = html.indexOf("</head>");
    let out = head >= 0 ? html.slice(0, head) + alternates + html.slice(head) : html;
    const marker = out.indexOf("data-agent-hint");
    const end = marker >= 0 ? out.indexOf("</blockquote>", marker) : -1;
    if (end < 0) {
        return { html: out, hint: false };
    }
    out = out.slice(0, end) + hintLine + out.slice(end);
    return { html: out, hint: true };
}

const ENTITIES: Record<string, string> = { amp: "&", lt: "<", gt: ">", quot: '"', "#39": "'", nbsp: " " };

export function decodeEntities(s: string): string {
    return s.replace(
        /&(amp|lt|gt|quot|#39|nbsp|#(\d+)|#x([0-9a-f]+));/gi,
        (m, name: string, dec?: string, hex?: string) => {
            if (dec) {
                return String.fromCodePoint(Number(dec));
            }
            if (hex) {
                return String.fromCodePoint(parseInt(hex, 16));
            }
            return ENTITIES[name.toLowerCase()] ?? m;
        }
    );
}

function attributes(tag: string): Record<string, string> {
    const out: Record<string, string> = {};
    const re = /([\w:-]+)(?:\s*=\s*(?:"([^"]*)"|'([^']*)'|([^\s"'>]+)))?/g;
    for (const m of tag.replace(/^<\w+|\/?>$/g, "").matchAll(re)) {
        out[m[1].toLowerCase()] = decodeEntities(m[2] ?? m[3] ?? m[4] ?? "");
    }
    return out;
}

function plainText(html: string): string {
    return decodeEntities(html.replace(/<[^>]*>/g, " "))
        .replace(/\s+/g, " ")
        .trim();
}

export interface PageSummary {
    title: string;
    description?: string;
    links: { text: string; href: string }[];
}

export function summarizePage(html: string): PageSummary {
    const titleMatch = /<title[^>]*>([\s\S]*?)<\/title>/i.exec(html);
    const title = titleMatch ? plainText(titleMatch[1]).replace(/\s+\|\s+RavenDB Documentation$/, "") : "";
    let description: string | undefined;
    for (const m of html.matchAll(/<meta\s[^>]*>/gi)) {
        const a = attributes(m[0]);
        if (a.name === "description" && a.content) {
            description = a.content;
            break;
        }
    }
    const mainStart = html.search(/<main[\s>]/i);
    const mainEnd = html.lastIndexOf("</main>");
    const main = mainStart >= 0 && mainEnd > mainStart ? html.slice(mainStart, mainEnd) : "";
    const links: PageSummary["links"] = [];
    const seen = new Set<string>();
    for (const m of main.matchAll(/<a\s([^>]*)>([\s\S]*?)<\/a>/gi)) {
        const href = attributes(`<a ${m[1]}>`).href;
        const text = plainText(m[2]);
        if (!href || href.startsWith("#") || !text || seen.has(href)) {
            continue;
        }
        seen.add(href);
        links.push({ text, href });
    }
    return { title, description, links };
}
