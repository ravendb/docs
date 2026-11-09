// Markdown copies of pages for coding agents; scripts/handle_redirects.js serves them.

import type { LoadContext, Plugin } from "@docusaurus/types";
import logger from "@docusaurus/logger";
import fs from "fs";
import path from "path";

import { buildAgentHintMarkdown, buildStaticUrl, readSkillDescription } from "../agent-hint-plugin/lib/hint.js";
import { SKILL_SOURCE_DIR, SKILL_URL_DIR } from "../agent-hint-plugin/lib/skill-tree.js";
import { createConverter } from "./lib/convert.js";
import {
    collectDocs,
    createReport,
    createSite,
    scanLanguages,
    writeListingCopies,
    writePageCopies,
    type DocRecord,
    type Report,
    type Run,
} from "./lib/export.js";

export { MARKDOWN_SECTIONS, MARKDOWN_VERSIONS, inMarkdownScope } from "./lib/scope.js";

const PLUGIN_NAME = "markdown-export";

export default function markdownExportPlugin(context: LoadContext): Plugin {
    const { siteDir, siteConfig, baseUrl } = context;
    const origin = siteConfig.url.replace(/\/+$/, "");
    const isStrict = process.env.DOCUSAURUS_STRICT === "true";
    let docs: DocRecord[] = [];

    return {
        name: PLUGIN_NAME,

        async allContentLoaded({ allContent }) {
            docs = collectDocs(allContent, siteDir);
        },

        async postBuild({ outDir, routesPaths }) {
            const report = createReport(siteDir);
            const skillMarkdown = fs.readFileSync(path.join(siteDir, SKILL_SOURCE_DIR, "SKILL.md"), "utf8");
            const run: Run = {
                outDir,
                origin,
                siteDir,
                agentSkill: buildAgentHintMarkdown(
                    buildStaticUrl(siteConfig.url, baseUrl, `${SKILL_URL_DIR}/SKILL.md`),
                    readSkillDescription(skillMarkdown)
                ),
                converter: createConverter(createSite(docs, outDir, origin, siteDir), report.warn),
                report,
                written: new Set(),
            };
            writePageCopies(run, scanLanguages(run, docs));
            writeListingCopies(run, routesPaths);
            logReport(report, run.written.size, isStrict);
        },
    };
}

function logReport(report: Report, pages: number, isStrict: boolean): void {
    logger.info(
        `[${PLUGIN_NAME}] ${report.files} Markdown files for ${pages} pages (${report.listings} from HTML), ` +
            `alternates and hint line on ${report.hinted} pages`
    );
    for (const [kind, w] of [...report.warnings].sort((a, b) => b[1].count - a[1].count)) {
        logger.warn(`[${PLUGIN_NAME}] ${w.count} x ${kind}, e.g. ${w.examples.join("; ")}`);
    }
    if (report.failures.length) {
        const message =
            `[${PLUGIN_NAME}] ${report.failures.length} page(s) have no Markdown copy:\n  ` +
            report.failures.slice(0, 20).join("\n  ");
        if (isStrict) {
            throw new Error(message);
        }
        logger.warn(message);
    }
    if (isStrict && report.unknownComponents.size) {
        throw new Error(
            `[${PLUGIN_NAME}] ${report.unknownComponents.size} unhandled component use(s). Add each component ` +
                `to WRAPPERS, TSX_ONLY or CARDS in src/plugins/markdown-export/lib/convert.ts, or give it its ` +
                `own case:\n  ${[...report.unknownComponents].join("\n  ")}`
        );
    }
}
