/**
 * Templates noindex plugin.
 *
 * Doc authoring templates live in templates/ and are exposed at /templates/*
 * so writers can see the rendered examples in situ — but they are internal
 * scaffolding, not product content. We don't want them in search results.
 *
 * Defense-in-depth: robots.txt also carries `Disallow: /templates/` so most
 * well-behaved crawlers never fetch these pages. The meta tag catches the
 * few that index before checking robots, and removes any already-indexed
 * templates on re-crawl.
 *
 * Walks build/templates/**\/*.html and injects `<meta name="robots"
 * content="noindex,nofollow">` into <head> if not already present.
 */

import docusaurusLogger from "@docusaurus/logger";
import type { LoadContext, Plugin } from "@docusaurus/types";
import fs from "node:fs";
import path from "node:path";

const META_TAG = '<meta name="robots" content="noindex,nofollow">';

function walk(dir: string, cb: (p: string) => void): void {
    if (!fs.existsSync(dir)) {
        return;
    }
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
        const full = path.join(dir, entry.name);
        if (entry.isDirectory()) {
            walk(full, cb);
        } else if (entry.isFile()) {
            cb(full);
        }
    }
}

function hasRobotsMeta(html: string): boolean {
    return /<meta\s+[^>]*\bname\s*=\s*["']robots["'][^>]*>/i.test(html);
}

function inject(html: string): string {
    // Place right after <head ...>, preserving whatever else Docusaurus put there.
    return html.replace(/<head(\s[^>]*)?>/i, (m) => `${m}${META_TAG}`);
}

const templatesNoindexPlugin = function templatesNoindexPlugin(_context: LoadContext): Plugin<void> {
    return {
        name: "templates-noindex-plugin",
        async postBuild({ outDir }) {
            const templatesDir = path.join(outDir, "templates");
            if (!fs.existsSync(templatesDir)) {
                return;
            }

            let scanned = 0;
            let injected = 0;

            walk(templatesDir, (file) => {
                if (!file.endsWith(".html")) {
                    return;
                }
                scanned++;
                const original = fs.readFileSync(file, "utf8");
                if (hasRobotsMeta(original)) {
                    return;
                }
                const updated = inject(original);
                if (updated !== original) {
                    fs.writeFileSync(file, updated, "utf8");
                    injected++;
                }
            });

            docusaurusLogger.info(
                `[templates-noindex] scanned ${scanned} template HTML file(s), injected noindex into ${injected}`
            );
        },
    };
};

export default templatesNoindexPlugin;
