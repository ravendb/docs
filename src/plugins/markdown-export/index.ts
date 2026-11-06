// Markdown-export plugin: writes a Markdown copy of every page in the maintained docs versions and
// the cloud, quill, guides and samples sections, one file per client language, for coding agents.
// The edge (scripts/handle_redirects.js) serves these copies to requests that prefer
// text/markdown and to explicit `.md` URLs. Built pages get <link rel="alternate"> tags and a
// line in the agent hint pointing at their copies.

import type { LoadContext, Plugin } from "@docusaurus/types";
import logger from "@docusaurus/logger";
import fs from "fs";
import path from "path";

import {
    buildAgentHintMarkdown,
    buildMarkdownHintLine,
    buildStaticUrl,
    readSkillDescription,
} from "../agent-hint-plugin/lib/hint.js";
import { SKILL_SOURCE_DIR, SKILL_URL_DIR } from "../agent-hint-plugin/lib/skill-tree.js";
import { createConverter, type ConvertSite, type DocTarget } from "./lib/convert.js";
import { buildDocument, languageLabel, type Variant } from "./lib/document.js";
import { addMarkdownLinks, alternateTags, summarizePage } from "./lib/html.js";
import { createImageIndex } from "./lib/images.js";
import { markdownPath, stripTrailingSlash } from "./lib/urls.js";
// version-policy.js is CJS; named-import interop works under both tsx --test and the Docusaurus loader.
import { ACTIVE_VERSIONS, CURRENT_VERSION } from "../../../scripts/lib/version-policy.js";

const PLUGIN_NAME = "markdown-export";

/** Versions whose pages get Markdown copies. Legacy versions are noindex and get none. */
export const MARKDOWN_VERSIONS: readonly string[] = [CURRENT_VERSION, ...ACTIVE_VERSIONS];

/** Unversioned docs instances that get Markdown copies. templates is for doc writers, not agents. */
export const MARKDOWN_SECTIONS: readonly string[] = ["cloud", "quill", "guides", "samples"];

const UNVERSIONED_ROOTS = new Set([...MARKDOWN_SECTIONS, "templates"]);

/** Where an image the bundler inlined as a data URI can still be fetched from. */
const SOURCE_BASE_URL = "https://raw.githubusercontent.com/ravendb/docs/main/";

/** Whether the edge serves Markdown for this route. Mirrored by `markdownScope` in handle_redirects.js. */
export function inMarkdownScope(route: string): boolean {
    const first = route.split("/")[1] ?? "";
    return MARKDOWN_VERSIONS.includes(first) || MARKDOWN_SECTIONS.includes(first);
}

// The parts of @docusaurus/plugin-content-docs' loaded content this plugin reads.
interface LoadedDoc {
    source: string;
    permalink: string;
    title: string;
    description: string;
}
interface LoadedVersion {
    versionName: string;
    contentPath: string;
    docs: LoadedDoc[];
}
interface DocsContent {
    loadedVersions: LoadedVersion[];
}

interface DocRecord extends DocTarget {
    file: string;
    contentRoot: string;
    version: string | null;
    title: string;
    description: string;
}

/** Docusaurus derives a missing description from the first text on the page, raw JSX included. */
function usableDescription(description: string | undefined): string | undefined {
    return description && !description.trimStart().startsWith("<") ? description : undefined;
}

function htmlFileOf(outDir: string, route: string): string {
    return path.join(outDir, ...route.split("/").filter(Boolean), "index.html");
}

function writeFile(file: string, content: string): void {
    fs.mkdirSync(path.dirname(file), { recursive: true });
    fs.writeFileSync(file, content, "utf8");
}

export default function markdownExportPlugin(context: LoadContext): Plugin {
    const { siteDir, siteConfig, baseUrl } = context;
    const origin = siteConfig.url.replace(/\/+$/, "");
    const isStrict = process.env.DOCUSAURUS_STRICT === "true";
    let docs: DocRecord[] = [];

    return {
        name: PLUGIN_NAME,

        async allContentLoaded({ allContent }) {
            docs = [];
            const instances = (allContent["docusaurus-plugin-content-docs"] ?? {}) as Record<string, DocsContent>;
            for (const [id, content] of Object.entries(instances)) {
                const versioned = id === "default";
                if (!versioned && !MARKDOWN_SECTIONS.includes(id)) {
                    continue;
                }
                for (const v of content.loadedVersions) {
                    const version = versioned ? (v.versionName === "current" ? CURRENT_VERSION : v.versionName) : null;
                    if (versioned && !MARKDOWN_VERSIONS.includes(String(version))) {
                        continue;
                    }
                    for (const doc of v.docs) {
                        docs.push({
                            file: path.join(siteDir, doc.source.replace(/^@site[\\/]/, "")),
                            contentRoot: v.contentPath,
                            route: stripTrailingSlash(doc.permalink),
                            version,
                            title: doc.title,
                            description: doc.description,
                            languages: [],
                            defaultLanguage: null,
                        });
                    }
                }
            }
        },

        // Synchronous from start to end: versioned-seo-plugin rewrites some of the same HTML files
        // in its own postBuild, and Docusaurus runs the two concurrently. Neither yields between
        // reading and writing a file, so neither can overwrite the other's edit.
        async postBuild({ outDir, routesPaths }) {
            const warnings = new Map<string, { count: number; examples: string[] }>();
            // A component the converter does not know loses whatever it holds in props. A strict
            // build fails on it, so whoever adds a component decides what agents get from it.
            const unknownComponents = new Set<string>();
            const warn = (kind: string, message: string, file: string) => {
                if (kind === "jsx") {
                    unknownComponents.add(`${message} (${path.relative(siteDir, file)})`);
                }
                const w = warnings.get(kind) ?? { count: 0, examples: [] };
                w.count++;
                if (w.examples.length < 5) {
                    w.examples.push(`${message} (${path.relative(siteDir, file)})`);
                }
                warnings.set(kind, w);
            };
            const failures: string[] = [];

            const bySource = new Map(docs.map((d) => [path.normalize(d.file), d]));
            const byRoute = new Map(docs.map((d) => [d.route, d]));
            const site: ConvertSite = {
                origin,
                siteDir,
                currentVersion: CURRENT_VERSION,
                unversionedRoots: UNVERSIONED_ROOTS,
                docBySource: (abs) => bySource.get(path.normalize(abs)),
                docByRoute: (route) => byRoute.get(route),
                imageUrl: createImageIndex(outDir, origin, siteDir, SOURCE_BASE_URL),
            };
            const converter = createConverter(site, warn);

            const skillDescription = readSkillDescription(
                fs.readFileSync(path.join(siteDir, SKILL_SOURCE_DIR, "SKILL.md"), "utf8")
            );
            const agentSkill = buildAgentHintMarkdown(
                buildStaticUrl(siteConfig.url, baseUrl, `${SKILL_URL_DIR}/SKILL.md`),
                skillDescription
            );

            // Pass 1: every page's languages, so a link can point at the target's copy in the
            // reader's language before that target is converted.
            const failed = new Set<DocRecord>();
            for (const doc of docs) {
                try {
                    const scanned = converter.scan(doc.file);
                    doc.languages = scanned.languages;
                    doc.defaultLanguage = scanned.defaultLanguage;
                } catch (e) {
                    failed.add(doc);
                    failures.push((e as Error).message);
                }
            }

            const written = new Set<string>();
            let files = 0;
            let hinted = 0;
            const patch = (route: string, variants: Variant[]) => {
                const htmlFile = htmlFileOf(outDir, route);
                if (!fs.existsSync(htmlFile)) {
                    return;
                }
                const links = variants.map((v) => ({
                    url: v.url,
                    label: v.language ? languageLabel(v.language) : null,
                }));
                const result = addMarkdownLinks(
                    fs.readFileSync(htmlFile, "utf8"),
                    alternateTags(links),
                    buildMarkdownHintLine(links)
                );
                fs.writeFileSync(htmlFile, result.html, "utf8");
                if (result.hint) {
                    hinted++;
                }
            };

            // Pass 2: the copies themselves.
            for (const doc of docs) {
                if (failed.has(doc)) {
                    continue;
                }
                let page;
                try {
                    page = converter.convert({
                        file: doc.file,
                        contentRoot: doc.contentRoot,
                        route: doc.route,
                        version: doc.version,
                    });
                } catch (e) {
                    failures.push((e as Error).message);
                    continue;
                }
                const languages: (string | null)[] = page.languages.length ? page.languages : [null];
                const variants: Variant[] = languages.map((language) => ({
                    language,
                    url: `${origin}${markdownPath(doc.route, language === doc.defaultLanguage ? null : language)}`,
                }));
                const bodies = new Map<string | null, string>();
                for (const language of languages) {
                    try {
                        bodies.set(language, page.render(language));
                    } catch (e) {
                        failures.push(
                            `${path.relative(siteDir, doc.file)} (${language ?? "page"}): ${(e as Error).message}`
                        );
                    }
                }
                // A landing page built entirely from TSX components has no Markdown body; the
                // listing pass below builds its copy from the links in its HTML instead.
                if ([...bodies.values()].every((body) => !body.trim())) {
                    continue;
                }
                const description = usableDescription(doc.description);
                for (const [language, body] of bodies) {
                    const file = path.join(
                        outDir,
                        markdownPath(doc.route, language === doc.defaultLanguage ? null : language)
                    );
                    writeFile(
                        file,
                        buildDocument({
                            title: doc.title,
                            description,
                            url: `${origin}${doc.route}`,
                            version: doc.version,
                            language,
                            variants,
                            agentSkill,
                            body,
                            seeAlso: page.seeAlso(language),
                        })
                    );
                    files++;
                }
                written.add(doc.route);
                patch(doc.route, variants);
            }

            // Routes with no MDX source (listing pages, tag pages, a section's landing page) get a
            // copy built from their HTML, so the edge never answers a Markdown request with a 404
            // for a page that exists.
            let listings = 0;
            const listingRoutes = new Set(
                routesPaths
                    .map(stripTrailingSlash)
                    .filter((route) => !written.has(route) && inMarkdownScope(route))
                    .filter((route) => fs.existsSync(htmlFileOf(outDir, route)))
            );
            // A listing links to pages, which have copies, and to other listings, which get one here.
            const listingLink = (href: string, pageUrl: string): string => {
                let url: URL;
                try {
                    url = new URL(href, pageUrl);
                } catch (_) {
                    return href;
                }
                const route = stripTrailingSlash(url.pathname);
                if (url.origin === new URL(origin).origin && listingRoutes.has(route)) {
                    return `${origin}${markdownPath(route)}${url.hash}`;
                }
                return converter.toMarkdownUrl(url.href, null);
            };
            for (const route of listingRoutes) {
                const htmlFile = htmlFileOf(outDir, route);
                const summary = summarizePage(fs.readFileSync(htmlFile, "utf8"));
                const pageUrl = `${origin}${route}`;
                const body = summary.links
                    .map((l) => `- [${l.text.replace(/[[\]]/g, "\\$&")}](${listingLink(l.href, pageUrl)})`)
                    .join("\n");
                const url = `${origin}${markdownPath(route)}`;
                writeFile(
                    path.join(outDir, markdownPath(route)),
                    buildDocument({
                        title: summary.title || route,
                        description: usableDescription(summary.description),
                        url: pageUrl,
                        version: MARKDOWN_VERSIONS.includes(route.split("/")[1]) ? route.split("/")[1] : null,
                        language: null,
                        variants: [{ language: null, url }],
                        agentSkill,
                        body,
                        seeAlso: [],
                    })
                );
                files++;
                listings++;
                written.add(route);
                patch(route, [{ language: null, url }]);
            }

            logger.info(
                `[${PLUGIN_NAME}] ${files} Markdown files for ${written.size} pages (${listings} from HTML), ` +
                    `alternates and hint line on ${hinted} pages`
            );
            for (const [kind, w] of [...warnings].sort((a, b) => b[1].count - a[1].count)) {
                logger.warn(`[${PLUGIN_NAME}] ${w.count} x ${kind}, e.g. ${w.examples.join("; ")}`);
            }
            if (failures.length) {
                const message = `[${PLUGIN_NAME}] ${failures.length} page(s) have no Markdown copy:\n  ${failures.slice(0, 20).join("\n  ")}`;
                if (isStrict) {
                    throw new Error(message);
                }
                logger.warn(message);
            }
            if (isStrict && unknownComponents.size) {
                throw new Error(
                    `[${PLUGIN_NAME}] ${unknownComponents.size} unhandled component use(s). Add each component to ` +
                        `WRAPPERS, TSX_ONLY or CARDS in src/plugins/markdown-export/lib/convert.ts, or give it ` +
                        `its own case:\n  ${[...unknownComponents].join("\n  ")}`
                );
            }
        },
    };
}
