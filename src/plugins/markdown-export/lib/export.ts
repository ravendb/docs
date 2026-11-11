// Fully synchronous: versioned-seo-plugin edits the same HTML files in a concurrent postBuild.

import fs from "fs";
import path from "path";

import { buildMarkdownHintLine } from "../../agent-hint-plugin/lib/hint.js";
import { createConverter, type ConvertSite, type ConvertedPage, type DocTarget, type Warn } from "./convert.js";
import { buildDocument, languageLabel, type Variant } from "./document.js";
import { addMarkdownLinks, alternateTags, summarizePage } from "./html.js";
import { createImageIndex } from "./images.js";
import {
    CURRENT_VERSION,
    MARKDOWN_SECTIONS,
    MARKDOWN_VERSIONS,
    UNVERSIONED_ROOTS,
    inMarkdownScope,
    versionOfRoute,
} from "./scope.js";
import { markdownPath, stripTrailingSlash } from "./urls.js";

const SOURCE_BASE_URL = "https://raw.githubusercontent.com/ravendb/docs/main/";

export interface DocRecord extends DocTarget {
    file: string;
    contentRoot: string;
    version: string | null;
    title: string;
    description: string;
}

export interface Report {
    warn: Warn;
    warnings: Map<string, { count: number; examples: string[] }>;
    unknownComponents: Set<string>;
    failures: string[];
    files: number;
    listings: number;
    hinted: number;
}

export interface Run {
    outDir: string;
    origin: string;
    siteDir: string;
    agentSkill: string;
    converter: ReturnType<typeof createConverter>;
    report: Report;
    written: Set<string>;
}

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

export function collectDocs(allContent: Record<string, unknown>, siteDir: string): DocRecord[] {
    const instances = (allContent["docusaurus-plugin-content-docs"] ?? {}) as Record<string, DocsContent>;
    return Object.entries(instances)
        .filter(([id]) => id === "default" || MARKDOWN_SECTIONS.includes(id))
        .flatMap(([id, content]) =>
            content.loadedVersions
                .map((v) => ({ v, version: id === "default" ? docsVersion(v.versionName) : null }))
                .filter(({ version }) => id !== "default" || MARKDOWN_VERSIONS.includes(String(version)))
                .flatMap(({ v, version }) => v.docs.map((doc) => docRecord(doc, v, version, siteDir)))
        );
}

function docsVersion(versionName: string): string {
    return versionName === "current" ? CURRENT_VERSION : versionName;
}

function docRecord(doc: LoadedDoc, v: LoadedVersion, version: string | null, siteDir: string): DocRecord {
    return {
        file: path.join(siteDir, doc.source.replace(/^@site[\\/]/, "")),
        contentRoot: v.contentPath,
        route: stripTrailingSlash(doc.permalink),
        version,
        title: doc.title,
        description: doc.description,
        languages: [],
        defaultLanguage: null,
    };
}

export function createReport(siteDir: string): Report {
    const report: Report = {
        warnings: new Map(),
        unknownComponents: new Set(),
        failures: [],
        files: 0,
        listings: 0,
        hinted: 0,
        warn: (kind, message, file) => {
            const where = `${message} (${path.relative(siteDir, file)})`;
            if (kind === "jsx") {
                report.unknownComponents.add(where);
            }
            const w = report.warnings.get(kind) ?? { count: 0, examples: [] };
            w.count++;
            if (w.examples.length < 5) {
                w.examples.push(where);
            }
            report.warnings.set(kind, w);
        },
    };
    return report;
}

export function createSite(docs: DocRecord[], outDir: string, origin: string, siteDir: string): ConvertSite {
    const bySource = new Map(docs.map((d) => [path.normalize(d.file), d]));
    const byRoute = new Map(docs.map((d) => [d.route, d]));
    return {
        origin,
        siteDir,
        currentVersion: CURRENT_VERSION,
        unversionedRoots: UNVERSIONED_ROOTS,
        docBySource: (abs) => bySource.get(path.normalize(abs)),
        docByRoute: (route) => byRoute.get(route),
        imageUrl: createImageIndex(outDir, origin, siteDir, SOURCE_BASE_URL),
    };
}

export function scanLanguages(run: Run, docs: DocRecord[]): DocRecord[] {
    return docs.filter((doc) => {
        try {
            const { languages, defaultLanguage } = run.converter.scan(doc.file);
            Object.assign(doc, { languages, defaultLanguage });
            return true;
        } catch (e) {
            run.report.failures.push((e as Error).message);
            return false;
        }
    });
}

export function writePageCopies(run: Run, docs: DocRecord[]): void {
    for (const doc of docs) {
        writePageCopy(run, doc);
    }
}

function writePageCopy(run: Run, doc: DocRecord): void {
    let page: ConvertedPage;
    try {
        page = run.converter.convert({
            file: doc.file,
            contentRoot: doc.contentRoot,
            route: doc.route,
            version: doc.version,
        });
    } catch (e) {
        run.report.failures.push((e as Error).message);
        return;
    }
    const copyPath = (language: string | null) =>
        markdownPath(doc.route, language === doc.defaultLanguage ? null : language);
    const languages: (string | null)[] = page.languages.length ? page.languages : [null];
    const variants: Variant[] = languages.map((language) => ({ language, url: `${run.origin}${copyPath(language)}` }));
    const bodies = renderBodies(run, doc, page, languages);
    // A TSX-only landing page gets a listing copy instead.
    if ([...bodies.values()].every((body) => !body.trim())) {
        return;
    }
    for (const [language, body] of bodies) {
        const document = buildDocument({
            title: doc.title,
            description: usableDescription(doc.description),
            url: `${run.origin}${doc.route}`,
            version: doc.version,
            language,
            variants,
            agentSkill: run.agentSkill,
            body,
            seeAlso: page.seeAlso(language),
        });
        writeFile(path.join(run.outDir, copyPath(language)), document);
        run.report.files++;
    }
    run.written.add(doc.route);
    linkPageToCopies(run, doc.route, variants);
}

function renderBodies(
    run: Run,
    doc: DocRecord,
    page: ConvertedPage,
    languages: (string | null)[]
): Map<string | null, string> {
    const bodies = new Map<string | null, string>();
    for (const language of languages) {
        try {
            bodies.set(language, page.render(language));
        } catch (e) {
            const where = `${path.relative(run.siteDir, doc.file)} (${language ?? "page"})`;
            run.report.failures.push(`${where}: ${(e as Error).message}`);
        }
    }
    return bodies;
}

/** Pages without MDX (listings, tags) get a copy of their links, so the edge never 404s on them. */
export function writeListingCopies(run: Run, routesPaths: string[]): void {
    const listings = new Set(
        routesPaths
            .map(stripTrailingSlash)
            .filter((route) => !run.written.has(route) && inMarkdownScope(route))
            .filter((route) => fs.existsSync(htmlFileOf(run.outDir, route)))
    );
    for (const route of listings) {
        writeListingCopy(run, route, listings);
    }
}

function writeListingCopy(run: Run, route: string, listings: Set<string>): void {
    const summary = summarizePage(fs.readFileSync(htmlFileOf(run.outDir, route), "utf8"));
    const pageUrl = `${run.origin}${route}`;
    const url = `${run.origin}${markdownPath(route)}`;
    const body = summary.links
        .map((l) => `- [${l.text.replace(/[[\]]/g, "\\$&")}](${listingLink(run, l.href, pageUrl, listings)})`)
        .join("\n");
    const document = buildDocument({
        title: summary.title || route,
        description: usableDescription(summary.description),
        url: pageUrl,
        version: versionOfRoute(route),
        language: null,
        variants: [{ language: null, url }],
        agentSkill: run.agentSkill,
        body,
        seeAlso: [],
    });
    writeFile(path.join(run.outDir, markdownPath(route)), document);
    run.report.files++;
    run.report.listings++;
    run.written.add(route);
    linkPageToCopies(run, route, [{ language: null, url }]);
}

function listingLink(run: Run, href: string, pageUrl: string, listings: Set<string>): string {
    let url: URL;
    try {
        url = new URL(href, pageUrl);
    } catch (_) {
        return href;
    }
    const route = stripTrailingSlash(url.pathname);
    if (url.origin === new URL(run.origin).origin && listings.has(route)) {
        return `${run.origin}${markdownPath(route)}${url.hash}`;
    }
    return run.converter.toMarkdownUrl(url.href, null);
}

function linkPageToCopies(run: Run, route: string, variants: Variant[]): void {
    const htmlFile = htmlFileOf(run.outDir, route);
    if (!fs.existsSync(htmlFile)) {
        return;
    }
    const links = variants.map((v) => ({ url: v.url, label: v.language ? languageLabel(v.language) : null }));
    const html = fs.readFileSync(htmlFile, "utf8");
    const result = addMarkdownLinks(html, alternateTags(links), buildMarkdownHintLine(links));
    fs.writeFileSync(htmlFile, result.html, "utf8");
    if (result.hint) {
        run.report.hinted++;
    }
}

/** Docusaurus may derive a description from raw JSX. */
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
