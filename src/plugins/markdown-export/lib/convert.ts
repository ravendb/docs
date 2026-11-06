// MDX source -> one Markdown document per client language. Works on the MDX AST rather than the
// built HTML: the HTML holds only the default language, because LanguageContent renders one
// language on the server. Imported language partials are spliced in where they are rendered,
// every MDX-only node is rewritten into plain Markdown, and stringify runs without remark-mdx, so
// any JSX left over throws instead of reaching an agent.

import fs from "fs";
import path from "path";
import { load } from "js-yaml";
import { unified } from "unified";
import remarkParse from "remark-parse";
import remarkGfm from "remark-gfm";
import remarkMdx from "remark-mdx";
import remarkStringify from "remark-stringify";
import remarkComment from "@slorber/remark-comment";

import { panelAnchorId } from "../../remark-panel-headings/index.js";
import { defaultLanguageOf, markdownPath, stripTrailingSlash } from "./urls.js";

type JsxAttribute = {
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

/** A page the converter can link to. */
export interface DocTarget {
    route: string;
    languages: readonly string[];
    defaultLanguage: string | null;
}

export interface ConvertSite {
    /** Site origin without a trailing slash, e.g. https://docs.ravendb.net */
    origin: string;
    siteDir: string;
    currentVersion: string;
    /** First path segments that are not version-prefixed: cloud, guides, ... */
    unversionedRoots: ReadonlySet<string>;
    docBySource(absPath: string): DocTarget | undefined;
    /** route: pathname without a trailing slash */
    docByRoute(route: string): DocTarget | undefined;
    imageUrl(absPath: string): string | null;
}

export interface PageInput {
    file: string;
    /** The docs version directory, the second place bare `x.mdx` links are looked up. */
    contentRoot: string;
    route: string;
    version: string | null;
}

export interface SeeAlsoItem {
    title: string;
    url: string;
    path?: string;
}

export interface ConvertedPage {
    frontMatter: Record<string, unknown>;
    languages: string[];
    defaultLanguage: string | null;
    /** Body Markdown (no front matter, no title) for one language, or the only variant (null). */
    render(language: string | null): string;
    seeAlso(language: string | null): SeeAlsoItem[];
}

export type Warn = (kind: string, message: string, file: string) => void;

const THEME_ALLOWED = new Set(["Admonition", "Tabs", "TabItem", "CodeBlock", "IdealImage"]);

const ADMONITION_LABEL: Record<string, string> = {
    note: "Note",
    info: "Info",
    warning: "Warning",
    danger: "Danger",
    tip: "Tip",
    caution: "Warning",
};

const CODE_LANG_MAP: Record<string, string> = {
    js: "javascript",
    ts: "typescript",
    plain: "text",
    batch: "text",
    git: "text",
    log: "text",
    wkt: "text",
    py: "python",
    "c#": "csharp",
    rql: "sql", // the site highlights RQL as `sql`
};

/** Fence info strings that hold only Docusaurus options, not a language. */
const CODE_META_ONLY = new Set(["showlinenumbers"]);
const CODE_LANG_ALLOWED = new Set([
    "csharp",
    "java",
    "javascript",
    "typescript",
    "python",
    "php",
    "sql",
    "json",
    "bash",
    "powershell",
    "http",
    "text",
    "xml",
    "html",
    "yaml",
    "ini",
    "dockerfile",
    "toml",
    "properties",
    "shell",
    "sh",
    "go",
    "css",
    "graphql",
    "groovy",
]);

/** Components that render the whole of their content from TSX, with nothing to convert. */
const TSX_ONLY = new Set([
    "StartingPoints",
    "UseCases",
    "Features",
    "CloudUseCases",
    "CloudFeatures",
    "QuillHero",
    "QuillFlow",
    "SamplesHomePage",
    "GuidesHeader",
    "FeaturedGuides",
    "RecentGuides",
    "GetStartedGuides",
    "TagsGrid",
]);

/** Layout wrappers whose children are the content. */
const WRAPPERS = new Set(["ContentFrame", "ColGridUnwrapped", "SampleLayout"]);

/** Link cards: a title, a URL and a description in props. */
const CARDS = new Set(["CardWithImage", "CardWithImageHorizontal", "CardWithIcon"]);

/** Video components carry their YouTube link in `url` (see remark-video-embeds). */
const VIDEO_COMPONENTS = new Set(["Video", "QuillWalkthrough"]);

const HID_OPEN = "\u27eahid:";
const HID_CLOSE = "\u27eb";

const FRONTMATTER_RE = /^---\r?\n([\s\S]*?)\r?\n---[ \t]*\r?\n?/;
const IMPORT_RE = /^\s*import\s+([A-Za-z_$][\w$]*)\s+from\s+(['"])(.+?)\2\s*;?\s*$/;

// remarkComment parses the HTML comments mdx1Compat allows and drops them. It must follow
// remarkMdx, or the JSX extension claims `<` first.
const parser = unified().use(remarkParse).use(remarkGfm).use(remarkMdx).use(remarkComment);

const serializer = unified()
    .use(remarkStringify, {
        bullet: "-",
        fences: true,
        rule: "-",
        ruleRepetition: 3,
        ruleSpaces: false,
        emphasis: "*",
        strong: "*",
        listItemIndent: "one",
        resourceLink: false,
        handlers: {
            // The default emits a trailing backslash; the sources use the two-space form.
            break(_node: unknown, _parent: unknown, state: { stack: string[] }) {
                return state.stack.includes("tableCell") ? " " : "  \n";
            },
        },
    })
    .use(remarkGfm, { tablePipeAlign: false });

interface Context {
    file: string;
    page: PageInput;
    language: string | null;
    imports: Map<string, { spec: string; abs: string | null }>;
    shift: number;
    minDepth: number;
    inTableCell: boolean;
}

interface State {
    anchors: Map<string, number>;
    sectionDepth: number;
    inlined: Set<string>;
}

export function createConverter(site: ConvertSite, warn: Warn) {
    // ---- parsing ---------------------------------------------------------------------------

    function splitFrontmatter(raw: string, file: string): { data: Record<string, unknown>; body: string } {
        const src = raw.replace(/^\uFEFF/, "");
        const m = FRONTMATTER_RE.exec(src);
        if (!m) {
            return { data: {}, body: src };
        }
        let data: Record<string, unknown> = {};
        try {
            data = (load(m[1]) as Record<string, unknown> | null) ?? {};
        } catch (e) {
            warn("frontmatter", `YAML parse failed: ${(e as Error).message}`, file);
        }
        return { data, body: src.slice(m[0].length) };
    }

    /** `## Foo {#id}` makes acorn choke inside remark-mdx; park the id in a sentinel. Fence-aware. */
    function maskHeadingIds(body: string): string {
        const lines = body.split("\n");
        let fence: string | null = null;
        for (let i = 0; i < lines.length; i++) {
            const fm = /^\s{0,3}(`{3,}|~{3,})/.exec(lines[i]);
            if (fm) {
                if (fence === null) {
                    fence = fm[1];
                } else if (fm[1][0] === fence[0] && fm[1].length >= fence.length) {
                    fence = null;
                }
                continue;
            }
            if (fence !== null) {
                continue;
            }
            const hm = /^(\s{0,3}#{1,6}\s+.*?)\s*\{#([A-Za-z0-9_:.-]+)\}\s*$/.exec(lines[i]);
            if (hm) {
                lines[i] = `${hm[1]} ${HID_OPEN}${hm[2]}${HID_CLOSE}`;
            }
        }
        return lines.join("\n");
    }

    function parseFile(abs: string): { data: Record<string, unknown>; tree: MdNode } {
        const { data, body } = splitFrontmatter(fs.readFileSync(abs, "utf8"), abs);
        try {
            return { data, tree: parser.parse(maskHeadingIds(body)) as unknown as MdNode };
        } catch (e) {
            const err = e as { reason?: string; message: string };
            throw new Error(`parse failed for ${abs}: ${err.reason ?? err.message}`);
        }
    }

    /** mdxjsEsm nodes hold several statements each, so split on newlines. */
    function collectImports(tree: MdNode, abs: string): Context["imports"] {
        const table: Context["imports"] = new Map();
        for (const node of tree.children ?? []) {
            if (node.type !== "mdxjsEsm") {
                continue;
            }
            for (const line of String(node.value).split("\n")) {
                const m = IMPORT_RE.exec(line);
                if (!m) {
                    continue;
                }
                const [, binding, , spec] = m;
                let target: string | null = null;
                if (spec.startsWith(".")) {
                    target = path.resolve(path.dirname(abs), spec);
                } else if (spec.startsWith("@site/")) {
                    target = path.join(site.siteDir, spec.slice("@site/".length));
                }
                if (spec.startsWith("@theme/") && !THEME_ALLOWED.has(spec.slice("@theme/".length))) {
                    warn("esm", `unhandled @theme import '${spec}'`, abs);
                }
                table.set(binding, { spec, abs: target });
            }
        }
        return table;
    }

    // ---- attributes ------------------------------------------------------------------------

    function attr(node: MdNode, name: string): JsxAttribute | undefined {
        return (node.attributes ?? []).find((a) => a.type === "mdxJsxAttribute" && a.name === name);
    }

    /** string value, null for a boolean attribute, undefined when absent or an expression */
    function attrString(node: MdNode, name: string): string | null | undefined {
        const a = attr(node, name);
        if (!a) {
            return undefined;
        }
        if (a.value === null || a.value === undefined) {
            return null;
        }
        return typeof a.value === "string" ? a.value : undefined;
    }

    function attrExpression(node: MdNode, name: string): string | undefined {
        const a = attr(node, name);
        return a && typeof a.value === "object" && a.value ? a.value.value : undefined;
    }

    function normaliseText(s: string): string {
        return s
            .replace(/\u00a0/g, " ")
            .replace(/[ \t]+/g, " ")
            .trim();
    }

    // ---- code ------------------------------------------------------------------------------

    function codeLang(raw: string | null | undefined, file: string): string | null {
        if (raw === null || raw === undefined) {
            return null;
        }
        let t = String(raw).trim().split(/\s+/)[0].toLowerCase();
        t = t.replace(/[^a-z0-9+#_-]/g, "");
        if (!t || CODE_META_ONLY.has(t)) {
            return null;
        }
        t = CODE_LANG_MAP[t] ?? t;
        if (!CODE_LANG_ALLOWED.has(t)) {
            warn("code-lang", `unknown code language '${raw}', dropped the info string`, file);
            return null;
        }
        return t;
    }

    /** Unescape a JS template literal body in one pass; chained replaces mangle `\\{`. */
    function unescapeTemplate(s: string): string {
        let out = "";
        for (let i = 0; i < s.length; i++) {
            const c = s[i];
            if (c !== "\\" || i + 1 >= s.length) {
                out += c;
                continue;
            }
            const n = s[i + 1];
            if ("\\`{}$<>\"'".includes(n)) {
                out += n;
                i++;
            } else if (n === "n") {
                out += "\n";
                i++;
            } else if (n === "t") {
                out += "\t";
                i++;
            } else if (n === "r") {
                out += "\r";
                i++;
            } else {
                out += c;
            }
        }
        return out;
    }

    /** <CodeBlock language="x">{`...`}</CodeBlock> -> a fenced code node. */
    function codeBlockToCode(node: MdNode, file: string): MdNode {
        const lang = codeLang(attrString(node, "language"), file);
        const exprs = (node.children ?? []).filter(
            (c) => c.type === "mdxFlowExpression" || c.type === "mdxTextExpression"
        );
        if (exprs.length !== 1) {
            warn("codeblock", `expected one template literal child, found ${exprs.length}`, file);
        }
        let value = "";
        if (exprs.length) {
            const raw = String(exprs[0].value).trim();
            value =
                raw.length >= 2 && raw.startsWith("`") && raw.endsWith("`") ? unescapeTemplate(raw.slice(1, -1)) : raw;
        }
        return { type: "code", lang, meta: null, value: value.replace(/\n+$/, "") };
    }

    // ---- URLs ------------------------------------------------------------------------------

    /** Docusaurus order: the first `#` wins, then the first `?` in the rest. */
    function splitUrl(url: string): { pathname: string; query: string; hash: string } {
        const hi = url.indexOf("#");
        const hash = hi >= 0 ? url.slice(hi) : "";
        let rest = hi >= 0 ? url.slice(0, hi) : url;
        const qi = rest.indexOf("?");
        const query = qi >= 0 ? rest.slice(qi) : "";
        rest = qi >= 0 ? rest.slice(0, qi) : rest;
        return { pathname: rest, query, hash };
    }

    function isAbsolute(url: string): boolean {
        return /^[a-z][a-z0-9+.-]*:/i.test(url) || url.startsWith("//");
    }

    function pageUrl(ctx: Context): string {
        return `${site.origin}${ctx.page.route}`;
    }

    /**
     * A link to one of our pages becomes a link to its Markdown copy, in the reader's language
     * when that page has it. Anything else (other sites, legacy versions, assets) is unchanged.
     */
    function toMarkdownUrl(url: string, language: string | null): string {
        if (!url.startsWith(`${site.origin}/`)) {
            return url;
        }
        const { pathname, hash } = splitUrl(url.slice(site.origin.length));
        const target = site.docByRoute(stripTrailingSlash(pathname));
        if (!target) {
            return url;
        }
        const variant =
            language && target.languages.includes(language) && language !== target.defaultLanguage ? language : null;
        return `${site.origin}${markdownPath(target.route, variant)}${hash}`;
    }

    /** A link into the document being written keeps only its anchor. */
    function resolveLink(url: string, ctx: Context): string {
        const resolved = resolveLinkTarget(url, ctx);
        const self = site.docByRoute(ctx.page.route);
        const variant = ctx.language && ctx.language !== self?.defaultLanguage ? ctx.language : null;
        const selfUrl = `${site.origin}${markdownPath(ctx.page.route, variant)}`;
        return resolved.startsWith(`${selfUrl}#`) ? resolved.slice(selfUrl.length) : resolved;
    }

    /**
     * Resolves a link the way the site does: `.mdx` paths are relative to the file that holds the
     * text (a partial, not its page), extensionless ones are relative to the rendering page.
     */
    function resolveLinkTarget(url: string, ctx: Context): string {
        if (!url) {
            return url;
        }
        if (isAbsolute(url)) {
            return toMarkdownUrl(url, ctx.language);
        }
        const { pathname, query, hash } = splitUrl(url);
        if (pathname === "") {
            return hash || url; // same document
        }
        if (pathname.startsWith("/")) {
            const first = pathname.split("/").filter(Boolean)[0] ?? "";
            const versioned = /^\d+\.\d+$/.test(first) || site.unversionedRoots.has(first);
            const base = versioned ? pathname : `/${site.currentVersion}${pathname}`;
            return toMarkdownUrl(`${site.origin}${base}${query}${hash}`, ctx.language);
        }
        if (/\.mdx?$/i.test(pathname)) {
            let decoded = pathname;
            try {
                decoded = decodeURIComponent(pathname);
            } catch (_) {
                // keep the raw form
            }
            const bare = !decoded.startsWith("./") && !decoded.startsWith("../");
            const dirs = bare ? [path.dirname(ctx.file), ctx.page.contentRoot, site.siteDir] : [path.dirname(ctx.file)];
            for (const dir of dirs) {
                const target = site.docBySource(path.resolve(dir, decoded));
                if (target) {
                    return toMarkdownUrl(`${site.origin}${target.route}${hash}`, ctx.language);
                }
            }
            warn("link", `unresolved link '${url}'`, ctx.file);
            return url;
        }
        let resolved: URL;
        try {
            resolved = new URL(pathname + query + hash, pageUrl(ctx));
        } catch (_) {
            warn("link", `could not resolve '${url}'`, ctx.file);
            return url;
        }
        return toMarkdownUrl(resolved.href, ctx.language);
    }

    function resolveImage(url: string, ctx: Context): string {
        if (!url || isAbsolute(url)) {
            return url;
        }
        const { pathname, query, hash } = splitUrl(url);
        if (pathname.startsWith("/")) {
            return `${site.origin}${pathname}${query}${hash}`;
        }
        let decoded = pathname;
        try {
            decoded = decodeURIComponent(pathname);
        } catch (_) {
            // keep the raw form
        }
        const abs = path.resolve(path.dirname(ctx.file), decoded);
        const published = site.imageUrl(abs);
        if (!published) {
            warn("image", `no published image for '${url}'`, ctx.file);
            return url;
        }
        return published;
    }

    // ---- tree transform --------------------------------------------------------------------

    const clampDepth = (d: number) => Math.max(1, Math.min(6, d));

    function claimAnchor(state: State, id: string): string {
        const n = state.anchors.get(id) ?? 0;
        state.anchors.set(id, n + 1);
        return n === 0 ? id : `${id}-${n}`;
    }

    function minHeadingDepth(nodes: MdNode[] | undefined): number {
        let min = Infinity;
        const walk = (list: MdNode[] | undefined) => {
            for (const n of list ?? []) {
                if (n.type === "heading") {
                    min = Math.min(min, n.depth ?? 6);
                }
                if (n.type === "mdxJsxFlowElement" && n.name === "Panel") {
                    min = Math.min(min, panelDepth(n));
                }
                walk(n.children);
            }
        };
        walk(nodes);
        return min;
    }

    function panelDepth(node: MdNode): number {
        const raw = attrString(node, "headingLevel") ?? attrExpression(node, "headingLevel") ?? "";
        const depth = Number.parseInt(String(raw), 10);
        return depth >= 1 && depth <= 6 ? depth : 2;
    }

    /** Re-base a produced subtree so its shallowest heading is an h2. */
    function rebaseToH2(nodes: MdNode[]): void {
        const headings: MdNode[] = [];
        const walk = (list: MdNode[] | undefined) => {
            for (const n of list ?? []) {
                if (n.type === "heading") {
                    headings.push(n);
                }
                walk(n.children);
            }
        };
        walk(nodes);
        const min = Math.min(...headings.map((h) => h.depth ?? 6));
        if (!Number.isFinite(min) || min === 2) {
            return;
        }
        for (const h of headings) {
            h.depth = clampDepth((h.depth ?? 2) + 2 - min);
        }
    }

    function heading(depth: number, text: string, anchorId: string | null, state: State): MdNode {
        const children: MdNode[] = [{ type: "text", value: text }];
        if (anchorId) {
            children.push({ type: "text", value: ` {#${claimAnchor(state, anchorId)}}` });
        }
        state.sectionDepth = depth;
        return { type: "heading", depth, children };
    }

    /** Turns the parked `{#id}` sentinel back into the heading id. Copies: the parsed tree is shared by every language. */
    function restoreHeadingIds(children: MdNode[], state: State): MdNode[] {
        const re = new RegExp(`${HID_OPEN}([A-Za-z0-9_:.-]+)${HID_CLOSE}`, "g");
        return children.map((c) =>
            c.type === "text" && typeof c.value === "string" && c.value.includes(HID_OPEN)
                ? { ...c, value: c.value.replace(re, (_, id: string) => `{#${claimAnchor(state, id)}}`) }
                : c
        );
    }

    function isBlank(n: MdNode): boolean {
        if (n.type === "text") {
            return !String(n.value).trim();
        }
        if (n.type === "paragraph") {
            return (n.children ?? []).every(isBlank);
        }
        return false;
    }

    /** An unwrapped container must not move a `---` to the top of the document. */
    function dropLeadingRule(nodes: MdNode[]): MdNode[] {
        return nodes.length && nodes[0].type === "thematicBreak" ? nodes.slice(1) : nodes;
    }

    function transform(nodes: MdNode[] | undefined, ctx: Context, state: State): MdNode[] {
        const out: MdNode[] = [];
        for (const node of nodes ?? []) {
            out.push(...transformNode(node, ctx, state));
        }
        return out;
    }

    function transformNode(node: MdNode, ctx: Context, state: State): MdNode[] {
        switch (node.type) {
            case "yaml":
            case "mdxjsEsm":
            case "comment":
            case "mdxFlowExpression": // CodeBlock bodies, consumed by the CodeBlock handler
                return [];

            case "mdxTextExpression": {
                const v = String(node.value).trim();
                return v ? [{ type: "text", value: v }] : [];
            }

            case "heading": {
                const depth = clampDepth(Math.max((node.depth ?? 2) + ctx.shift, ctx.minDepth));
                const children = restoreHeadingIds(transform(node.children, ctx, state), state);
                if (JSON.stringify(children).replace(/[^\w]/g, "").length === 0) {
                    return []; // `####` with no text, or only an `<a id>` target
                }
                state.sectionDepth = depth;
                return [{ ...node, depth, children }];
            }

            case "code": {
                const harm =
                    /^\s*<\/?(Tabs|TabItem|CodeBlock|Admonition|Panel|ContentFrame|LanguageContent)[\s/>]/m.exec(
                        String(node.value)
                    );
                if (harm) {
                    warn(
                        "fence",
                        `code block contains '${harm[0].trim()}': an unclosed fence swallowed markup`,
                        ctx.file
                    );
                }
                return [{ type: "code", lang: codeLang(node.lang, ctx.file), meta: null, value: node.value }];
            }

            case "link":
                return [
                    {
                        ...node,
                        url: resolveLink(String(node.url), ctx),
                        children: transform(node.children, ctx, state),
                    },
                ];
            case "definition":
                return [{ ...node, url: resolveLink(String(node.url), ctx) }];
            case "image":
                return [{ ...node, url: resolveImage(String(node.url), ctx) }];

            case "tableCell":
                return [{ ...node, children: transform(node.children, { ...ctx, inTableCell: true }, state) }];

            case "mdxJsxFlowElement":
            case "mdxJsxTextElement":
                return transformJsx(node, ctx, state);

            default:
                return node.children ? [{ ...node, children: transform(node.children, ctx, state) }] : [node];
        }
    }

    function transformJsx(node: MdNode, ctx: Context, state: State): MdNode[] {
        const name = node.name;
        if (!name) {
            return transform(node.children, ctx, state); // fragment
        }
        if (/^[a-z]/.test(name)) {
            return transformHtml(node, ctx, state);
        }
        const imported = ctx.imports.get(name);
        if (imported?.abs && /\.mdx?$/i.test(imported.spec)) {
            return inlinePartial(imported.abs, ctx, state);
        }

        switch (name) {
            case "LanguageSwitcher":
                return [];

            case "LanguageContent":
                // Only reached when a LanguageContent is nested below the page root; the page
                // level is handled by render().
                return attrString(node, "language") === ctx.language || ctx.language === null
                    ? transform(node.children, ctx, state)
                    : [];

            case "Panel": {
                const raw = attrString(node, "heading");
                if (typeof raw !== "string") {
                    warn("panel", "Panel without a string heading", ctx.file);
                    return transform(node.children, ctx, state);
                }
                const text = normaliseText(raw);
                const depth = clampDepth(Math.max(panelDepth(node) + ctx.shift, ctx.minDepth));
                // Markdown h2s inside a Panel are its subsections, not its siblings.
                const minRaw = minHeadingDepth(node.children);
                const extra = minRaw !== Infinity && minRaw <= depth ? depth + 1 - minRaw : 0;
                const h = heading(depth, text, panelAnchorId(text), state);
                const inner = transform(
                    node.children,
                    { ...ctx, shift: ctx.shift + extra, minDepth: Math.max(ctx.minDepth, depth + 1) },
                    state
                );
                return [h, ...dropLeadingRule(inner)];
            }

            case "Admonition": {
                const type = String(attrString(node, "type") || "note").toLowerCase();
                const label = ADMONITION_LABEL[type] ?? "Note";
                const titleRaw = attrString(node, "title");
                const title = typeof titleRaw === "string" ? normaliseText(titleRaw) : "";
                const id = attrString(node, "id");
                const labelKids: MdNode[] = [
                    { type: "strong", children: [{ type: "text", value: title ? `${label}:` : label }] },
                ];
                if (title) {
                    labelKids.push({ type: "text", value: ` ${title}` });
                }
                if (typeof id === "string" && id) {
                    labelKids.push({ type: "text", value: ` {#${claimAnchor(state, id)}}` });
                }
                return [
                    {
                        type: "blockquote",
                        children: [{ type: "paragraph", children: labelKids }, ...transform(node.children, ctx, state)],
                    },
                ];
            }

            case "Tabs": {
                const items = (node.children ?? []).filter(
                    (c) => c.type === "mdxJsxFlowElement" && c.name === "TabItem"
                );
                // Tab headings hang one level below the enclosing section, never below the previous
                // tab group, or consecutive groups ratchet down to h6.
                const outer = state.sectionDepth;
                const tabDepth = items.length < 2 ? 0 : clampDepth(Math.max(outer + 1, ctx.minDepth));
                const res: MdNode[] = [];
                for (const it of node.children ?? []) {
                    if (it.type === "mdxJsxFlowElement" && it.name === "TabItem") {
                        res.push(...renderTabItem(it, ctx, state, tabDepth));
                    } else if (!isBlank(it)) {
                        res.push(...transformNode(it, ctx, state));
                    }
                }
                state.sectionDepth = outer;
                return res;
            }

            case "TabItem":
                return renderTabItem(node, ctx, state, 0);

            case "CodeBlock":
                return [codeBlockToCode(node, ctx.file)];

            case "ColGrid": {
                const items: MdNode[] = [];
                const rest: MdNode[] = [];
                for (const c of node.children ?? []) {
                    if (c.type === "mdxJsxFlowElement" && CARDS.has(String(c.name))) {
                        items.push(cardListItem(c, ctx));
                    } else if (!isBlank(c)) {
                        rest.push(...transformNode(c, ctx, state));
                    }
                }
                return [
                    ...(items.length ? [{ type: "list", ordered: false, spread: false, children: items }] : []),
                    ...rest,
                ];
            }

            // @theme/IdealImage: <Image img={require("./x.webp")} alt="..." />
            case "Image": {
                const source =
                    attrString(node, "img") ??
                    /require\(\s*["']([^"']+)["']\s*\)/.exec(attrExpression(node, "img") ?? "")?.[1];
                const alt = attrString(node, "alt");
                return typeof source === "string"
                    ? [
                          {
                              type: "paragraph",
                              children: [
                                  {
                                      type: "image",
                                      url: resolveImage(source, ctx),
                                      alt: typeof alt === "string" ? alt : "",
                                      title: null,
                                  },
                              ],
                          },
                      ]
                    : [];
            }

            case "FeatureAccordion": {
                const title = normaliseText(attrString(node, "title") || "");
                const description = normaliseText(attrString(node, "description") || "");
                const lead: MdNode[] = [{ type: "strong", children: [{ type: "text", value: title }] }];
                if (description) {
                    lead.push({ type: "text", value: `: ${description}` });
                }
                return [{ type: "paragraph", children: lead }, ...transform(node.children, ctx, state)];
            }

            default:
                if (WRAPPERS.has(name)) {
                    return dropLeadingRule(transform(node.children, ctx, state));
                }
                if (CARDS.has(name)) {
                    return [{ type: "list", ordered: false, spread: false, children: [cardListItem(node, ctx)] }];
                }
                if (TSX_ONLY.has(name)) {
                    return [];
                }
                if (VIDEO_COMPONENTS.has(name)) {
                    const url = attrString(node, "url");
                    const title = attrString(node, "title");
                    return typeof url === "string"
                        ? [
                              {
                                  type: "paragraph",
                                  children: [
                                      { type: "text", value: "Video: " },
                                      {
                                          type: "link",
                                          url,
                                          title: null,
                                          children: [{ type: "text", value: typeof title === "string" ? title : url }],
                                      },
                                  ],
                              },
                          ]
                        : [];
                }
                warn("jsx", `unhandled component <${name}>, kept its children`, ctx.file);
                return transform(node.children, ctx, state);
        }
    }

    function renderTabItem(node: MdNode, ctx: Context, state: State, depth: number): MdNode[] {
        const label = normaliseText(attrString(node, "label") || "");
        if (!depth || !label) {
            return transform(node.children, ctx, state);
        }
        const saved = state.sectionDepth;
        state.sectionDepth = depth;
        const body = transform(node.children, { ...ctx, minDepth: Math.max(ctx.minDepth, depth + 1) }, state);
        state.sectionDepth = saved;
        return [{ type: "heading", depth, children: [{ type: "text", value: label }] }, ...body];
    }

    function cardListItem(node: MdNode, ctx: Context): MdNode {
        const title = normaliseText(attrString(node, "title") || "");
        const url = attrString(node, "url");
        const kids: MdNode[] = [];
        if (typeof url === "string") {
            const href = resolveLink(url, ctx);
            kids.push({ type: "link", url: href, title: null, children: [{ type: "text", value: title || href }] });
        } else {
            kids.push({ type: "text", value: title });
        }
        const desc = attrString(node, "description") ?? attrExpression(node, "description");
        if (typeof desc === "string") {
            const text = normaliseText(desc.replace(/<\/?[A-Za-z][^>]*\/?>/g, " "));
            if (text) {
                kids.push({ type: "text", value: ` - ${text}` });
            }
        }
        return { type: "listItem", spread: false, children: [{ type: "paragraph", children: kids }] };
    }

    function htmlEscape(s: string): string {
        return s.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    }

    /** Inline HTML, only where Markdown has no syntax: lists and line breaks inside table cells. */
    function inlineToHtml(nodes: MdNode[] | undefined, ctx: Context): string {
        let out = "";
        for (const n of nodes ?? []) {
            switch (n.type) {
                case "text":
                    out += htmlEscape(String(n.value));
                    break;
                case "inlineCode":
                    out += `<code>${htmlEscape(String(n.value))}</code>`;
                    break;
                case "strong":
                    out += `<strong>${inlineToHtml(n.children, ctx)}</strong>`;
                    break;
                case "emphasis":
                    out += `<em>${inlineToHtml(n.children, ctx)}</em>`;
                    break;
                case "break":
                    out += "<br />";
                    break;
                case "link":
                    out += `<a href="${htmlEscape(resolveLink(String(n.url), ctx))}">${inlineToHtml(n.children, ctx)}</a>`;
                    break;
                case "list": {
                    const tag = n.ordered ? "ol" : "ul";
                    out += `<${tag}>${inlineToHtml(n.children, ctx)}</${tag}>`;
                    break;
                }
                case "listItem":
                    out += `<li>${inlineToHtml(n.children, ctx)}</li>`;
                    break;
                case "mdxJsxTextElement":
                case "mdxJsxFlowElement": {
                    const name = n.name;
                    if (name && ["br", "hr", "img", "wbr"].includes(name)) {
                        out += `<${name} />`;
                    } else if (name && /^[a-z]/.test(name)) {
                        out += `<${name}>${inlineToHtml(n.children, ctx)}</${name}>`;
                    } else {
                        out += inlineToHtml(n.children, ctx);
                    }
                    break;
                }
                default:
                    if (n.children) {
                        out += inlineToHtml(n.children, ctx);
                    } else if (typeof n.value === "string") {
                        out += htmlEscape(n.value);
                    }
            }
        }
        return out;
    }

    /** The phrasing content of a block that may hold paragraphs, for a context that takes only phrasing. */
    function wrapPhrasing(nodes: MdNode[]): MdNode[] {
        return nodes.flatMap((n) => (n.type === "paragraph" ? (n.children ?? []) : [n]));
    }

    function wrapFlow(nodes: MdNode[]): MdNode[] {
        const phrasing = new Set([
            "text",
            "inlineCode",
            "strong",
            "emphasis",
            "link",
            "break",
            "image",
            "html",
            "delete",
        ]);
        const out: MdNode[] = [];
        let buf: MdNode[] = [];
        const flush = () => {
            if (buf.length) {
                out.push({ type: "paragraph", children: buf });
                buf = [];
            }
        };
        for (const n of nodes) {
            if (phrasing.has(n.type)) {
                buf.push(n);
            } else {
                flush();
                out.push(n);
            }
        }
        flush();
        return out;
    }

    function transformHtml(node: MdNode, ctx: Context, state: State): MdNode[] {
        const name = String(node.name);
        const inline = node.type === "mdxJsxTextElement";
        switch (name) {
            case "br":
                if (ctx.inTableCell) {
                    return [{ type: "html", value: "<br />" }];
                }
                return inline ? [{ type: "break" }] : [];
            case "hr":
                return inline ? [] : [{ type: "thematicBreak" }];
            case "a": {
                const href = attrString(node, "href");
                const kids = transform(node.children, ctx, state);
                if (typeof href === "string" && href) {
                    return [{ type: "link", url: resolveLink(href, ctx), title: null, children: kids }];
                }
                return kids; // `<a id="x">` is an anchor target
            }
            case "strong":
            case "b":
                return [{ type: "strong", children: transform(node.children, ctx, state) }];
            case "em":
            case "i":
                return [{ type: "emphasis", children: transform(node.children, ctx, state) }];
            case "code":
                return [{ type: "inlineCode", value: inlineToHtml(node.children, ctx).replace(/<[^>]*>/g, "") }];
            case "summary":
                return inline
                    ? [{ type: "strong", children: transform(node.children, ctx, state) }]
                    : [
                          {
                              type: "paragraph",
                              children: [
                                  { type: "strong", children: wrapPhrasing(transform(node.children, ctx, state)) },
                              ],
                          },
                      ];
            case "h1":
            case "h2":
            case "h3":
            case "h4":
            case "h5":
            case "h6": {
                const depth = clampDepth(Math.max(Number(name.slice(1)) + ctx.shift, ctx.minDepth));
                state.sectionDepth = depth;
                return [{ type: "heading", depth, children: wrapPhrasing(transform(node.children, ctx, state)) }];
            }
            case "iframe": {
                const src = attrString(node, "src");
                return typeof src === "string"
                    ? [
                          {
                              type: "paragraph",
                              children: [
                                  { type: "link", url: src, title: null, children: [{ type: "text", value: "Video" }] },
                              ],
                          },
                      ]
                    : [];
            }
        }
        if (ctx.inTableCell) {
            return [{ type: "html", value: inlineToHtml([node], ctx) }];
        }
        if (name === "ul" || name === "ol") {
            const items = (node.children ?? [])
                .filter((c) => (c.type === "mdxJsxFlowElement" || c.type === "mdxJsxTextElement") && c.name === "li")
                .map((c) => ({
                    type: "listItem",
                    spread: false,
                    children: wrapFlow(transform(c.children, ctx, state)),
                }));
            return items.length
                ? [{ type: "list", ordered: name === "ol", spread: false, children: items }]
                : transform(node.children, ctx, state);
        }
        if (!["u", "span", "p", "main", "div", "sup", "sub", "pre", "small", "section", "details"].includes(name)) {
            warn("html", `unhandled tag <${name}>, kept its children`, ctx.file);
        }
        return transform(node.children, ctx, state);
    }

    function inlinePartial(abs: string, ctx: Context, state: State): MdNode[] {
        if (!fs.existsSync(abs)) {
            warn("partial", `partial not found: ${abs}`, ctx.file);
            return [];
        }
        if (state.inlined.has(abs)) {
            warn("partial", `partial inlined more than once: ${abs}`, ctx.file);
        }
        state.inlined.add(abs);
        const { tree } = parseFile(abs);
        // `.mdx` links are relative to the partial; extensionless ones stay relative to the page.
        return transform(tree.children, { ...ctx, file: abs, imports: collectImports(tree, abs) }, state);
    }

    // ---- see also --------------------------------------------------------------------------

    /** Mirrors src/components/SeeAlso/useVersionedLink. */
    function seeAlsoUrl(link: string, source: string, page: PageInput): string {
        if (isAbsolute(link)) {
            return link;
        }
        const l = link.startsWith("/") ? link : `/${link}`;
        if (source !== "docs" || /^\/\d+\.\d+\//.test(l) || site.unversionedRoots.has(l.split("/")[1] ?? "")) {
            return `${site.origin}${l}`;
        }
        return `${site.origin}/${page.version ?? site.currentVersion}${l}`;
    }

    // ---- page ------------------------------------------------------------------------------

    /** The languages a page renders, in page order. Cheap enough to run for every page up front. */
    function languagesOf(tree: MdNode): string[] {
        const found: string[] = [];
        for (const n of tree.children ?? []) {
            if (n.type === "mdxJsxFlowElement" && n.name === "LanguageContent") {
                const l = attrString(n, "language");
                if (typeof l === "string" && l && !found.includes(l)) {
                    found.push(l);
                }
            }
        }
        return found;
    }

    function scan(file: string): {
        languages: string[];
        defaultLanguage: string | null;
        frontMatter: Record<string, unknown>;
    } {
        const { data, tree } = parseFile(file);
        const languages = languagesOf(tree);
        const declared = Array.isArray(data.supported_languages) ? data.supported_languages.map(String) : [];
        return { languages, defaultLanguage: defaultLanguageOf(declared, languages), frontMatter: data };
    }

    function convert(page: PageInput): ConvertedPage {
        const { data, tree } = parseFile(page.file);
        const imports = collectImports(tree, page.file);
        const languages = languagesOf(tree);
        const declared = Array.isArray(data.supported_languages) ? data.supported_languages.map(String) : [];
        for (const d of declared) {
            if (languages.length && !languages.includes(d)) {
                warn("languages", `declares '${d}' but renders no LanguageContent for it`, page.file);
            }
        }
        for (const l of languages) {
            if (!declared.includes(l)) {
                warn("languages", `renders '${l}' but supported_languages does not declare it`, page.file);
            }
        }

        function render(language: string | null): string {
            const state: State = { anchors: new Map(), sectionDepth: 1, inlined: new Set() };
            const base: Context = {
                file: page.file,
                page,
                language,
                imports,
                shift: 0,
                minDepth: 2,
                inTableCell: false,
            };
            const body: MdNode[] = [];
            for (const node of tree.children ?? []) {
                if (node.type === "mdxJsxFlowElement" && node.name === "LanguageContent") {
                    if (attrString(node, "language") !== language) {
                        continue;
                    }
                    const produced = transform(node.children, { ...base, minDepth: 1 }, state);
                    rebaseToH2(produced);
                    body.push(...produced);
                } else {
                    body.push(...transformNode(node, base, state));
                }
            }
            return String(serializer.stringify({ type: "root", children: body } as never)).replace(/^\n+/, "");
        }

        function seeAlso(language: string | null): SeeAlsoItem[] {
            const items = Array.isArray(data.see_also) ? (data.see_also as Record<string, unknown>[]) : [];
            return items.map((item) => ({
                title: String(item.title ?? ""),
                url: toMarkdownUrl(seeAlsoUrl(String(item.link ?? ""), String(item.source ?? "docs"), page), language),
                path: typeof item.path === "string" ? item.path : undefined,
            }));
        }

        return {
            frontMatter: data,
            languages,
            defaultLanguage: defaultLanguageOf(declared, languages),
            render,
            seeAlso,
        };
    }

    return { scan, convert, toMarkdownUrl };
}
