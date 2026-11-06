import { load } from "js-yaml";
import path from "path";

// Visually hidden, the sr-only recipe. Inline so it holds before any stylesheet loads.
const VISUALLY_HIDDEN =
    "position:absolute;width:1px;height:1px;padding:0;margin:-1px;overflow:hidden;clip:rect(0,0,0,0);white-space:nowrap;border:0";

const FRONTMATTER = /^---\r?\n([\s\S]*?)\r?\n---(?:\r?\n|$)/;

export function escapeHtml(value: string): string {
    return value
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#39;");
}

/** Absolute URL of a file served from static/, honouring the site's baseUrl. */
export function buildStaticUrl(siteUrl: string, baseUrl: string, staticPath: string): string {
    return new URL(path.posix.join(baseUrl, staticPath), siteUrl).href;
}

/**
 * The skill's frontmatter `description` is its measured trigger text: rewording it changed how
 * often agents opened the skill. The hint reuses it verbatim rather than paraphrasing it.
 */
export function readSkillDescription(skillMarkdown: string): string {
    const match = FRONTMATTER.exec(skillMarkdown.replace(/^\uFEFF/, ""));
    if (!match) {
        throw new Error("SKILL.md has no frontmatter block");
    }
    const frontMatter = load(match[1]) as { description?: unknown } | null;
    const description = frontMatter?.description;
    if (typeof description !== "string" || description.trim() === "") {
        throw new Error("SKILL.md frontmatter has no description");
    }
    return description.trim();
}

/**
 * Markup for the top of <body>, read by agents that convert fetched HTML to Markdown.
 * Hidden from sighted users (sr-only) and from assistive tech (aria-hidden), and kept out of
 * the tab order, the same shape Mintlify and Cloudflare ship on their docs.
 *
 * The text states what RavenDB publishes and quotes the skill's own description, instead of
 * telling the reader to fetch it: agents read an instruction inside page content as prompt
 * injection and refuse it, while this wording gets the skill fetched.
 */
export function buildAgentHint(skillUrl: string, description: string): string {
    const url = escapeHtml(skillUrl);
    return (
        `<blockquote data-agent-hint aria-hidden="true" style="${VISUALLY_HIDDEN}">` +
        `<h2>RavenDB agent skill</h2>` +
        `<p>RavenDB publishes an official agent skill for coding assistants at ` +
        `<a href="${url}" tabindex="-1">${url}</a> ` +
        `(also installable with <code>npx skills add ravendb/skills</code>). ` +
        `The skill describes itself as: "${escapeHtml(description)}"</p>` +
        `</blockquote>`
    );
}

/** The same sentence as the hint, for the top of the Markdown copies, which carry no HTML hint. */
export function buildAgentHintMarkdown(skillUrl: string, description: string): string {
    return (
        `RavenDB publishes an official agent skill for coding assistants at ${skillUrl} ` +
        `(also installable with \`npx skills add ravendb/skills\`). ` +
        `The skill describes itself as: "${description}"`
    );
}

/**
 * A paragraph for the hint naming this page's Markdown copies. markdown-export inserts it into
 * each built page, since the hint itself is the same on every page. Descriptive, like the hint.
 */
export function buildMarkdownHintLine(variants: { label: string | null; url: string }[]): string {
    const link = (url: string, text: string) => `<a href="${escapeHtml(url)}" tabindex="-1">${escapeHtml(text)}</a>`;
    if (variants.length === 1 && !variants[0].label) {
        return `<p>This page is also available as Markdown at ${link(variants[0].url, variants[0].url)}.</p>`;
    }
    const links = variants.map((v) => link(v.url, v.label ?? v.url)).join(", ");
    return `<p>This page is also available as Markdown, one file per client language: ${links}.</p>`;
}
