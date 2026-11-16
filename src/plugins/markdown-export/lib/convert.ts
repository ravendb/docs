// Works on MDX, not built HTML: the HTML holds only the default language.

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
import {
    attrExpression,
    attrString,
    blockquote,
    descendants,
    dropLeadingRule,
    emphasis,
    heading,
    html,
    image,
    isBlank,
    link,
    list,
    listItem,
    normaliseText,
    paragraph,
    strong,
    text,
    wrapFlow,
    wrapPhrasing,
    type MdNode,
} from "./mdast.js";
import { defaultLanguageOf, markdownPath, stripTrailingSlash } from "./urls.js";

export interface DocTarget {
    route: string;
    languages: readonly string[];
    defaultLanguage: string | null;
}

export interface ConvertSite {
    origin: string;
    siteDir: string;
    currentVersion: string;
    unversionedRoots: ReadonlySet<string>;
    docBySource(absPath: string): DocTarget | undefined;
    docByRoute(route: string): DocTarget | undefined;
    imageUrl(absPath: string): string | null;
}

export interface PageInput {
    file: string;
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

const WRAPPERS = new Set(["ContentFrame", "ColGridUnwrapped", "SampleLayout"]);

const CARDS = new Set(["CardWithImage", "CardWithImageHorizontal", "CardWithIcon"]);

const VIDEO_COMPONENTS = new Set(["Video", "QuillWalkthrough"]);

const HTML_WRAPPERS = new Set(["u", "span", "p", "main", "div", "sup", "sub", "pre", "small", "section", "details"]);

const HID_OPEN = "\u27eahid:";
const HID_CLOSE = "\u27eb";

const FRONTMATTER_RE = /^---\r?\n([\s\S]*?)\r?\n---[ \t]*\r?\n?/;
const IMPORT_RE = /^\s*import\s+([A-Za-z_$][\w$]*)\s+from\s+(['"])(.+?)\2\s*;?\s*$/;
const SWALLOWED_MARKUP = /^\s*<\/?(Tabs|TabItem|CodeBlock|Admonition|Panel|ContentFrame|LanguageContent)[\s/>]/m;

// remarkComment drops HTML comments; it must follow remarkMdx, which would claim `<` first.
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
            break(_node: unknown, _parent: unknown, state: { stack: string[] }) {
                return state.stack.includes("tableCell") ? " " : "  \n";
            },
        },
    })
    .use(remarkGfm, { tablePipeAlign: false });

interface Env {
    site: ConvertSite;
    warn: Warn;
}

type Imports = Map<string, { spec: string; abs: string | null }>;

interface Context {
    env: Env;
    file: string;
    page: PageInput;
    language: string | null;
    imports: Imports;
    shift: number;
    minDepth: number;
    inTableCell: boolean;
}

interface State {
    anchors: Map<string, number>;
    sectionDepth: number;
    inlined: Set<string>;
}

type Render = (node: MdNode, ctx: Context, state: State) => MdNode[];

export function createConverter(site: ConvertSite, warn: Warn) {
    const env: Env = { site, warn };
    return {
        scan: (file: string) => scanPage(env, file),
        convert: (page: PageInput) => convertPage(env, page),
        toMarkdownUrl: (url: string, language: string | null) => toMarkdownUrl(site, url, language),
    };
}

function scanPage(env: Env, file: string) {
    const { data, tree } = parseFile(env, file);
    const languages = languagesOf(tree);
    return { languages, defaultLanguage: defaultLanguageOf(declaredLanguages(data), languages), frontMatter: data };
}

function convertPage(env: Env, page: PageInput): ConvertedPage {
    const { data, tree } = parseFile(env, page.file);
    const imports = collectImports(env, tree, page.file);
    const languages = languagesOf(tree);
    const declared = declaredLanguages(data);
    for (const d of declared.filter((l) => languages.length && !languages.includes(l))) {
        env.warn("languages", `declares '${d}' but renders no LanguageContent for it`, page.file);
    }
    for (const l of languages.filter((l) => !declared.includes(l))) {
        env.warn("languages", `renders '${l}' but supported_languages does not declare it`, page.file);
    }
    return {
        frontMatter: data,
        languages,
        defaultLanguage: defaultLanguageOf(declared, languages),
        render: (language) => renderPage({ env, file: page.file, page, language, imports }, tree),
        seeAlso: (language) => seeAlsoOf(env.site, page, data, language),
    };
}

function renderPage(base: Pick<Context, "env" | "file" | "page" | "language" | "imports">, tree: MdNode): string {
    const state: State = { anchors: new Map(), sectionDepth: 1, inlined: new Set() };
    const ctx: Context = { ...base, shift: 0, minDepth: 2, inTableCell: false };
    const body: MdNode[] = [];
    for (const node of tree.children ?? []) {
        if (!isLanguageContent(node)) {
            body.push(...transformNode(node, ctx, state));
        } else if (attrString(node, "language") === ctx.language) {
            const produced = transform(node.children, { ...ctx, minDepth: 1 }, state);
            rebaseToH2(produced);
            body.push(...produced);
        }
    }
    return String(serializer.stringify({ type: "root", children: body } as never)).replace(/^\n+/, "");
}

function isLanguageContent(node: MdNode): boolean {
    return node.type === "mdxJsxFlowElement" && node.name === "LanguageContent";
}

function languagesOf(tree: MdNode): string[] {
    const found = (tree.children ?? []).filter(isLanguageContent).map((n) => attrString(n, "language"));
    return [...new Set(found.filter((l): l is string => typeof l === "string" && l !== ""))];
}

function declaredLanguages(data: Record<string, unknown>): string[] {
    return Array.isArray(data.supported_languages) ? data.supported_languages.map(String) : [];
}

function seeAlsoOf(
    site: ConvertSite,
    page: PageInput,
    data: Record<string, unknown>,
    language: string | null
): SeeAlsoItem[] {
    const items = Array.isArray(data.see_also) ? (data.see_also as Record<string, unknown>[]) : [];
    return items.map((item) => ({
        title: String(item.title ?? ""),
        url: toMarkdownUrl(
            site,
            seeAlsoUrl(site, String(item.link ?? ""), String(item.source ?? "docs"), page),
            language
        ),
        path: typeof item.path === "string" ? item.path : undefined,
    }));
}

/** Mirrors src/components/SeeAlso/useVersionedLink. */
function seeAlsoUrl(site: ConvertSite, link: string, source: string, page: PageInput): string {
    if (isAbsolute(link)) {
        return link;
    }
    const l = link.startsWith("/") ? link : `/${link}`;
    if (source !== "docs" || /^\/\d+\.\d+\//.test(l) || site.unversionedRoots.has(l.split("/")[1] ?? "")) {
        return `${site.origin}${l}`;
    }
    return `${site.origin}/${page.version ?? site.currentVersion}${l}`;
}

function parseFile(env: Env, abs: string): { data: Record<string, unknown>; tree: MdNode } {
    const { data, body } = splitFrontmatter(env, fs.readFileSync(abs, "utf8"), abs);
    try {
        return { data, tree: parser.parse(maskHeadingIds(body)) as unknown as MdNode };
    } catch (e) {
        const err = e as { reason?: string; message: string };
        throw new Error(`parse failed for ${abs}: ${err.reason ?? err.message}`);
    }
}

function splitFrontmatter(env: Env, raw: string, file: string): { data: Record<string, unknown>; body: string } {
    const src = raw.replace(/^\uFEFF/, "");
    const m = FRONTMATTER_RE.exec(src);
    if (!m) {
        return { data: {}, body: src };
    }
    try {
        return { data: (load(m[1]) as Record<string, unknown> | null) ?? {}, body: src.slice(m[0].length) };
    } catch (e) {
        env.warn("frontmatter", `YAML parse failed: ${(e as Error).message}`, file);
        return { data: {}, body: src.slice(m[0].length) };
    }
}

/** Hides `{#id}` heading ids from acorn, outside code fences. */
function maskHeadingIds(body: string): string {
    const lines = body.split("\n");
    let fence: string | null = null;
    for (let i = 0; i < lines.length; i++) {
        const fm = /^\s{0,3}(`{3,}|~{3,})/.exec(lines[i]);
        if (fm) {
            const closes = fence !== null && fm[1][0] === fence[0] && fm[1].length >= fence.length;
            fence = fence === null ? fm[1] : closes ? null : fence;
            continue;
        }
        const hm = fence === null ? /^(\s{0,3}#{1,6}\s+.*?)\s*\{#([A-Za-z0-9_:.-]+)\}\s*$/.exec(lines[i]) : null;
        if (hm) {
            lines[i] = `${hm[1]} ${HID_OPEN}${hm[2]}${HID_CLOSE}`;
        }
    }
    return lines.join("\n");
}

function collectImports(env: Env, tree: MdNode, abs: string): Imports {
    const table: Imports = new Map();
    const esm = (tree.children ?? []).filter((node) => node.type === "mdxjsEsm");
    for (const line of esm.flatMap((node) => String(node.value).split("\n"))) {
        const m = IMPORT_RE.exec(line);
        if (!m) {
            continue;
        }
        const [, binding, , spec] = m;
        if (spec.startsWith("@theme/") && !THEME_ALLOWED.has(spec.slice("@theme/".length))) {
            env.warn("esm", `unhandled @theme import '${spec}'`, abs);
        }
        table.set(binding, { spec, abs: importTarget(env, abs, spec) });
    }
    return table;
}

function importTarget(env: Env, importer: string, spec: string): string | null {
    if (spec.startsWith(".")) {
        return path.resolve(path.dirname(importer), spec);
    }
    if (spec.startsWith("@site/")) {
        return path.join(env.site.siteDir, spec.slice("@site/".length));
    }
    return null;
}

function codeLang(env: Env, raw: string | null | undefined, file: string): string | null {
    if (raw === null || raw === undefined) {
        return null;
    }
    const token = String(raw)
        .trim()
        .split(/\s+/)[0]
        .toLowerCase()
        .replace(/[^a-z0-9+#_-]/g, "");
    if (!token || CODE_META_ONLY.has(token)) {
        return null;
    }
    const lang = CODE_LANG_MAP[token] ?? token;
    if (!CODE_LANG_ALLOWED.has(lang)) {
        env.warn("code-lang", `unknown code language '${raw}', dropped the info string`, file);
        return null;
    }
    return lang;
}

const TEMPLATE_ESCAPES: Record<string, string> = { n: "\n", t: "\t", r: "\r" };

function unescapeTemplate(s: string): string {
    return s.replace(/\\([\s\S])/g, (escape, c: string) => {
        if ("\\`{}$<>\"'".includes(c)) {
            return c;
        }
        return TEMPLATE_ESCAPES[c] ?? escape;
    });
}

function codeBlockToCode(node: MdNode, ctx: Context): MdNode {
    const exprs = (node.children ?? []).filter((c) => c.type === "mdxFlowExpression" || c.type === "mdxTextExpression");
    if (exprs.length !== 1) {
        ctx.env.warn("codeblock", `expected one template literal child, found ${exprs.length}`, ctx.file);
    }
    const raw = exprs.length ? String(exprs[0].value).trim() : "";
    const isTemplate = raw.length >= 2 && raw.startsWith("`") && raw.endsWith("`");
    const value = isTemplate ? unescapeTemplate(raw.slice(1, -1)) : raw;
    const lang = codeLang(ctx.env, attrString(node, "language"), ctx.file);
    return { type: "code", lang, meta: null, value: value.replace(/\n+$/, "") };
}

function renderCode(node: MdNode, ctx: Context): MdNode[] {
    const swallowed = SWALLOWED_MARKUP.exec(String(node.value));
    if (swallowed) {
        const message = `code block contains '${swallowed[0].trim()}': an unclosed fence swallowed markup`;
        ctx.env.warn("fence", message, ctx.file);
    }
    return [{ type: "code", lang: codeLang(ctx.env, node.lang, ctx.file), meta: null, value: node.value }];
}

function splitUrl(url: string): { pathname: string; query: string; hash: string } {
    const hi = url.indexOf("#");
    const hash = hi >= 0 ? url.slice(hi) : "";
    const beforeHash = hi >= 0 ? url.slice(0, hi) : url;
    const qi = beforeHash.indexOf("?");
    const query = qi >= 0 ? beforeHash.slice(qi) : "";
    return { pathname: qi >= 0 ? beforeHash.slice(0, qi) : beforeHash, query, hash };
}

function isAbsolute(url: string): boolean {
    return /^[a-z][a-z0-9+.-]*:/i.test(url) || url.startsWith("//");
}

function decodePath(pathname: string): string {
    try {
        return decodeURIComponent(pathname);
    } catch (_) {
        return pathname;
    }
}

function toMarkdownUrl(site: ConvertSite, url: string, language: string | null): string {
    if (!url.startsWith(`${site.origin}/`)) {
        return url;
    }
    const { pathname, hash } = splitUrl(url.slice(site.origin.length));
    const target = site.docByRoute(stripTrailingSlash(pathname));
    if (!target) {
        return url;
    }
    const hasLanguage = language && target.languages.includes(language) && language !== target.defaultLanguage;
    return `${site.origin}${markdownPath(target.route, hasLanguage ? language : null)}${hash}`;
}

function resolveLink(url: string, ctx: Context): string {
    const { site } = ctx.env;
    const resolved = resolveLinkTarget(url, ctx);
    const self = site.docByRoute(ctx.page.route);
    const variant = ctx.language && ctx.language !== self?.defaultLanguage ? ctx.language : null;
    const selfUrl = `${site.origin}${markdownPath(ctx.page.route, variant)}`;
    return resolved.startsWith(`${selfUrl}#`) ? resolved.slice(selfUrl.length) : resolved;
}

/** As the site does: `.mdx` links from the file holding the text, others from the page URL. */
function resolveLinkTarget(url: string, ctx: Context): string {
    const { site } = ctx.env;
    if (!url) {
        return url;
    }
    if (isAbsolute(url)) {
        return toMarkdownUrl(site, url, ctx.language);
    }
    const { pathname, query, hash } = splitUrl(url);
    if (pathname === "") {
        return hash || url; // same document
    }
    if (pathname.startsWith("/")) {
        const first = pathname.split("/").filter(Boolean)[0] ?? "";
        const versioned = /^\d+\.\d+$/.test(first) || site.unversionedRoots.has(first);
        const base = versioned ? pathname : `/${site.currentVersion}${pathname}`;
        return toMarkdownUrl(site, `${site.origin}${base}${query}${hash}`, ctx.language);
    }
    if (/\.mdx?$/i.test(pathname)) {
        return resolveSourceLink(url, pathname, hash, ctx);
    }
    try {
        const resolved = new URL(pathname + query + hash, `${site.origin}${ctx.page.route}`);
        return toMarkdownUrl(site, resolved.href, ctx.language);
    } catch (_) {
        ctx.env.warn("link", `could not resolve '${url}'`, ctx.file);
        return url;
    }
}

function resolveSourceLink(url: string, pathname: string, hash: string, ctx: Context): string {
    const { site } = ctx.env;
    const decoded = decodePath(pathname);
    const bare = !decoded.startsWith("./") && !decoded.startsWith("../");
    const dirs = bare ? [path.dirname(ctx.file), ctx.page.contentRoot, site.siteDir] : [path.dirname(ctx.file)];
    const target = dirs.map((dir) => site.docBySource(path.resolve(dir, decoded))).find(Boolean);
    if (!target) {
        ctx.env.warn("link", `unresolved link '${url}'`, ctx.file);
        return url;
    }
    return toMarkdownUrl(site, `${site.origin}${target.route}${hash}`, ctx.language);
}

function resolveImage(url: string, ctx: Context): string {
    const { site } = ctx.env;
    if (!url || isAbsolute(url)) {
        return url;
    }
    const { pathname, query, hash } = splitUrl(url);
    if (pathname.startsWith("/")) {
        return `${site.origin}${pathname}${query}${hash}`;
    }
    const published = site.imageUrl(path.resolve(path.dirname(ctx.file), decodePath(pathname)));
    if (!published) {
        ctx.env.warn("image", `no published image for '${url}'`, ctx.file);
        return url;
    }
    return published;
}

const clampDepth = (d: number) => Math.max(1, Math.min(6, d));

function claimAnchor(state: State, id: string): string {
    const n = state.anchors.get(id) ?? 0;
    state.anchors.set(id, n + 1);
    return n === 0 ? id : `${id}-${n}`;
}

function panelDepth(node: MdNode): number {
    const raw = attrString(node, "headingLevel") ?? attrExpression(node, "headingLevel") ?? "";
    const depth = Number.parseInt(String(raw), 10);
    return depth >= 1 && depth <= 6 ? depth : 2;
}

function minHeadingDepth(nodes: MdNode[] | undefined): number {
    const depths = descendants(nodes).map((n) => {
        if (n.type === "heading") {
            return n.depth ?? 6;
        }
        return n.type === "mdxJsxFlowElement" && n.name === "Panel" ? panelDepth(n) : Infinity;
    });
    return Math.min(Infinity, ...depths);
}

function rebaseToH2(nodes: MdNode[]): void {
    const headings = descendants(nodes).filter((n) => n.type === "heading");
    const min = Math.min(...headings.map((h) => h.depth ?? 6));
    if (!Number.isFinite(min) || min === 2) {
        return;
    }
    for (const h of headings) {
        h.depth = clampDepth((h.depth ?? 2) + 2 - min);
    }
}

function anchoredHeading(depth: number, title: string, anchorId: string, state: State): MdNode {
    state.sectionDepth = depth;
    return anchorId
        ? heading(depth, text(title), text(` {#${claimAnchor(state, anchorId)}}`))
        : heading(depth, text(title));
}

/** Copies the nodes: every language shares the parsed tree. */
function restoreHeadingIds(children: MdNode[], state: State): MdNode[] {
    const re = new RegExp(`${HID_OPEN}([A-Za-z0-9_:.-]+)${HID_CLOSE}`, "g");
    return children.map((c) =>
        c.type === "text" && typeof c.value === "string" && c.value.includes(HID_OPEN)
            ? { ...c, value: c.value.replace(re, (_, id: string) => `{#${claimAnchor(state, id)}}`) }
            : c
    );
}

function transform(nodes: MdNode[] | undefined, ctx: Context, state: State): MdNode[] {
    return (nodes ?? []).flatMap((node) => transformNode(node, ctx, state));
}

function transformNode(node: MdNode, ctx: Context, state: State): MdNode[] {
    switch (node.type) {
        case "yaml":
        case "mdxjsEsm":
        case "comment":
        case "mdxFlowExpression": // CodeBlock bodies, consumed by the CodeBlock handler
            return [];
        case "mdxTextExpression": {
            const value = String(node.value).trim();
            return value ? [text(value)] : [];
        }
        case "heading":
            return renderMarkdownHeading(node, ctx, state);
        case "code":
            return renderCode(node, ctx);
        case "link":
            return [
                { ...node, url: resolveLink(String(node.url), ctx), children: transform(node.children, ctx, state) },
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

function renderMarkdownHeading(node: MdNode, ctx: Context, state: State): MdNode[] {
    const depth = clampDepth(Math.max((node.depth ?? 2) + ctx.shift, ctx.minDepth));
    const children = restoreHeadingIds(transform(node.children, ctx, state), state);
    if (children.length === 0) {
        return []; // `####` with no text, or only an `<a id>` target
    }
    state.sectionDepth = depth;
    return [{ ...node, depth, children }];
}

const COMPONENTS = new Map<string, Render>([
    ["LanguageSwitcher", () => []],
    ["LanguageContent", renderNestedLanguageContent],
    ["Panel", renderPanel],
    ["Admonition", renderAdmonition],
    ["Tabs", renderTabs],
    ["TabItem", (node, ctx, state) => renderTabItem(node, ctx, state, 0)],
    ["CodeBlock", (node, ctx) => [codeBlockToCode(node, ctx)]],
    ["ColGrid", renderColGrid],
    ["Image", renderIdealImage],
    ["FeatureAccordion", renderFeatureAccordion],
]);

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
    const render = COMPONENTS.get(name);
    if (render) {
        return render(node, ctx, state);
    }
    if (WRAPPERS.has(name)) {
        return dropLeadingRule(transform(node.children, ctx, state));
    }
    if (CARDS.has(name)) {
        return [list([cardListItem(node, ctx)])];
    }
    if (TSX_ONLY.has(name)) {
        return [];
    }
    if (VIDEO_COMPONENTS.has(name)) {
        return renderVideo(node);
    }
    ctx.env.warn("jsx", `unhandled component <${name}>, kept its children`, ctx.file);
    return transform(node.children, ctx, state);
}

function inlinePartial(abs: string, ctx: Context, state: State): MdNode[] {
    if (!fs.existsSync(abs)) {
        ctx.env.warn("partial", `partial not found: ${abs}`, ctx.file);
        return [];
    }
    if (state.inlined.has(abs)) {
        ctx.env.warn("partial", `partial inlined more than once: ${abs}`, ctx.file);
    }
    state.inlined.add(abs);
    const { tree } = parseFile(ctx.env, abs);
    return transform(tree.children, { ...ctx, file: abs, imports: collectImports(ctx.env, tree, abs) }, state);
}

function renderNestedLanguageContent(node: MdNode, ctx: Context, state: State): MdNode[] {
    const matches = ctx.language === null || attrString(node, "language") === ctx.language;
    return matches ? transform(node.children, ctx, state) : [];
}

function renderPanel(node: MdNode, ctx: Context, state: State): MdNode[] {
    const raw = attrString(node, "heading");
    if (typeof raw !== "string") {
        ctx.env.warn("panel", "Panel without a string heading", ctx.file);
        return transform(node.children, ctx, state);
    }
    const title = normaliseText(raw);
    const depth = clampDepth(Math.max(panelDepth(node) + ctx.shift, ctx.minDepth));
    const minRaw = minHeadingDepth(node.children);
    const extra = minRaw !== Infinity && minRaw <= depth ? depth + 1 - minRaw : 0;
    const h = anchoredHeading(depth, title, panelAnchorId(title), state);
    const innerCtx = { ...ctx, shift: ctx.shift + extra, minDepth: Math.max(ctx.minDepth, depth + 1) };
    return [h, ...dropLeadingRule(transform(node.children, innerCtx, state))];
}

function renderAdmonition(node: MdNode, ctx: Context, state: State): MdNode[] {
    const type = String(attrString(node, "type") || "note").toLowerCase();
    const label = ADMONITION_LABEL[type] ?? "Note";
    const title = normaliseText(attrString(node, "title") ?? "");
    const id = attrString(node, "id");
    const lead = [strong(text(title ? `${label}:` : label))];
    if (title) {
        lead.push(text(` ${title}`));
    }
    if (id) {
        lead.push(text(` {#${claimAnchor(state, id)}}`));
    }
    return [blockquote(paragraph(...lead), ...transform(node.children, ctx, state))];
}

/** Tab headings sit one level below the enclosing section, so consecutive groups don't ratchet down. */
function renderTabs(node: MdNode, ctx: Context, state: State): MdNode[] {
    const children = node.children ?? [];
    const isTab = (c: MdNode) => c.type === "mdxJsxFlowElement" && c.name === "TabItem";
    const outer = state.sectionDepth;
    const tabDepth = children.filter(isTab).length < 2 ? 0 : clampDepth(Math.max(outer + 1, ctx.minDepth));
    const out = children
        .filter((c) => isTab(c) || !isBlank(c))
        .flatMap((c) => (isTab(c) ? renderTabItem(c, ctx, state, tabDepth) : transformNode(c, ctx, state)));
    state.sectionDepth = outer;
    return out;
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
    return [heading(depth, text(label)), ...body];
}

function renderColGrid(node: MdNode, ctx: Context, state: State): MdNode[] {
    const children = node.children ?? [];
    const isCard = (c: MdNode) => c.type === "mdxJsxFlowElement" && CARDS.has(String(c.name));
    const cards = children.filter(isCard).map((c) => cardListItem(c, ctx));
    const rest = children.filter((c) => !isCard(c) && !isBlank(c)).flatMap((c) => transformNode(c, ctx, state));
    return cards.length ? [list(cards), ...rest] : rest;
}

function cardListItem(node: MdNode, ctx: Context): MdNode {
    const title = normaliseText(attrString(node, "title") || "");
    const url = attrString(node, "url");
    const href = typeof url === "string" ? resolveLink(url, ctx) : null;
    const lead = href !== null ? link(href, text(title || href)) : text(title);
    const desc = attrString(node, "description") ?? attrExpression(node, "description");
    const description = typeof desc === "string" ? normaliseText(desc.replace(/<\/?[A-Za-z][^>]*\/?>/g, " ")) : "";
    return listItem(paragraph(lead, ...(description ? [text(` - ${description}`)] : [])));
}

/** @theme/IdealImage: <Image img={require("./x.webp")} alt="..." /> */
function renderIdealImage(node: MdNode, ctx: Context): MdNode[] {
    const required = /require\(\s*["']([^"']+)["']\s*\)/.exec(attrExpression(node, "img") ?? "")?.[1];
    const source = attrString(node, "img") ?? required;
    const alt = attrString(node, "alt");
    return typeof source === "string" ? [paragraph(image(resolveImage(source, ctx), alt ?? ""))] : [];
}

function renderFeatureAccordion(node: MdNode, ctx: Context, state: State): MdNode[] {
    const title = normaliseText(attrString(node, "title") || "");
    const description = normaliseText(attrString(node, "description") || "");
    const lead = [strong(text(title)), ...(description ? [text(`: ${description}`)] : [])];
    return [paragraph(...lead), ...transform(node.children, ctx, state)];
}

function renderVideo(node: MdNode): MdNode[] {
    const url = attrString(node, "url");
    const title = attrString(node, "title");
    return typeof url === "string" ? [paragraph(text("Video: "), link(url, text(title ?? url)))] : [];
}

function transformHtml(node: MdNode, ctx: Context, state: State): MdNode[] {
    const name = String(node.name);
    const inline = node.type === "mdxJsxTextElement";
    const children = () => transform(node.children, ctx, state);
    switch (name) {
        case "br":
            if (ctx.inTableCell) {
                return [html("<br />")];
            }
            return inline ? [{ type: "break" }] : [];
        case "hr":
            return inline ? [] : [{ type: "thematicBreak" }];
        case "a": {
            const href = attrString(node, "href");
            return href ? [link(resolveLink(href, ctx), ...children())] : children(); // `<a id>` is a target
        }
        case "strong":
        case "b":
            return [strong(...children())];
        case "em":
        case "i":
            return [emphasis(...children())];
        case "code":
            return [{ type: "inlineCode", value: inlineToHtml(node.children, ctx).replace(/<[^>]*>/g, "") }];
        case "summary":
            return inline ? [strong(...children())] : [paragraph(strong(...wrapPhrasing(children())))];
        case "iframe": {
            const src = attrString(node, "src");
            return typeof src === "string" ? [paragraph(link(src, text("Video")))] : [];
        }
    }
    if (/^h[1-6]$/.test(name)) {
        const depth = clampDepth(Math.max(Number(name.slice(1)) + ctx.shift, ctx.minDepth));
        state.sectionDepth = depth;
        return [heading(depth, ...wrapPhrasing(children()))];
    }
    if (ctx.inTableCell) {
        return [html(inlineToHtml([node], ctx))];
    }
    if (name === "ul" || name === "ol") {
        return renderHtmlList(node, ctx, state, name === "ol");
    }
    if (!HTML_WRAPPERS.has(name)) {
        ctx.env.warn("html", `unhandled tag <${name}>, kept its children`, ctx.file);
    }
    return children();
}

function renderHtmlList(node: MdNode, ctx: Context, state: State, ordered: boolean): MdNode[] {
    const isItem = (c: MdNode) => (c.type === "mdxJsxFlowElement" || c.type === "mdxJsxTextElement") && c.name === "li";
    const items = (node.children ?? [])
        .filter(isItem)
        .map((c) => listItem(...wrapFlow(transform(c.children, ctx, state))));
    return items.length ? [list(items, ordered)] : transform(node.children, ctx, state);
}

function htmlEscape(s: string): string {
    return s.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
}

const INLINE_HTML: Record<string, (n: MdNode, inner: string, ctx: Context) => string> = {
    text: (n) => htmlEscape(String(n.value)),
    inlineCode: (n) => `<code>${htmlEscape(String(n.value))}</code>`,
    strong: (_, inner) => `<strong>${inner}</strong>`,
    emphasis: (_, inner) => `<em>${inner}</em>`,
    break: () => "<br />",
    link: (n, inner, ctx) => `<a href="${htmlEscape(resolveLink(String(n.url), ctx))}">${inner}</a>`,
    list: (n, inner) => (n.ordered ? `<ol>${inner}</ol>` : `<ul>${inner}</ul>`),
    listItem: (_, inner) => `<li>${inner}</li>`,
    mdxJsxTextElement: jsxToHtml,
    mdxJsxFlowElement: jsxToHtml,
};

function jsxToHtml(n: MdNode, inner: string): string {
    const name = n.name;
    if (name && ["br", "hr", "img", "wbr"].includes(name)) {
        return `<${name} />`;
    }
    return name && /^[a-z]/.test(name) ? `<${name}>${inner}</${name}>` : inner;
}

function inlineToHtml(nodes: MdNode[] | undefined, ctx: Context): string {
    return (nodes ?? [])
        .map((n) => {
            const inner = n.children ? inlineToHtml(n.children, ctx) : "";
            const render = INLINE_HTML[n.type];
            if (render) {
                return render(n, inner, ctx);
            }
            return n.children ? inner : typeof n.value === "string" ? htmlEscape(n.value) : "";
        })
        .join("");
}
