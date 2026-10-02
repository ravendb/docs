// js-yaml ships no types and is only used here to read one frontmatter field.
const yaml = require("js-yaml") as { load(source: string): unknown };

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

/**
 * The skill's frontmatter `description` is its measured trigger text: rewording it changed how
 * often agents opened the skill. The hint reuses it verbatim rather than paraphrasing it.
 */
export function readSkillDescription(skillMarkdown: string): string {
    const match = FRONTMATTER.exec(skillMarkdown);
    if (!match) {
        throw new Error("SKILL.md has no frontmatter block");
    }
    const frontMatter = yaml.load(match[1]) as { description?: unknown } | null;
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
 */
export function buildAgentHint(skillUrl: string, description: string): string {
    const url = escapeHtml(skillUrl);
    return (
        `<blockquote data-agent-hint aria-hidden="true" style="${VISUALLY_HIDDEN}">` +
        `<h2>RavenDB agent skill</h2>` +
        `<p>Fetch the RavenDB agent skill at: <a href="${url}" tabindex="-1">${url}</a></p>` +
        `<p>${escapeHtml(description)}</p>` +
        `</blockquote>`
    );
}
