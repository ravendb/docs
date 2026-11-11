import { test } from "node:test";
import assert from "node:assert/strict";
import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

import { MARKDOWN_SECTIONS, MARKDOWN_VERSIONS } from "../index.js";
import { DOCS_LANGUAGES, markdownPath } from "../lib/urls.js";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const SCRIPTS = path.join(__dirname, "..", "..", "..", "..", "scripts");

type Request = { uri: string; headers: Record<string, { value: string }>; querystring?: Record<string, unknown> };
type Redirect = { statusCode: number; headers: { location: { value: string } } };

const REDIRECTS: Record<string, { targetUrl: string; minimumVersion?: string }> = {
    "/old-page": { targetUrl: "/new-page" },
    "/guides/old-guide": { targetUrl: "/guides/new-guide" },
};

function loadFunction<T>(file: string, names: string[]): T {
    const source = fs.readFileSync(path.join(SCRIPTS, file), "utf8").replace(/^import cf from "cloudfront";$/m, "");
    const cf = {
        kvs: () => ({
            get: async (key: string) => {
                if (!(key in REDIRECTS)) {
                    throw new Error("key not found");
                }
                return JSON.stringify(REDIRECTS[key]);
            },
        }),
    };
    return new Function("cf", `${source}\nreturn { ${names.join(", ")} };`)(cf) as T;
}

const edge = loadFunction<{
    handler: (event: { request: Request }) => Promise<Request | Redirect>;
    prefersMarkdown: (accept: string | undefined) => boolean;
    MARKDOWN_VERSIONS: string[];
    MARKDOWN_SECTIONS: string[];
    markdownPageRegex: RegExp;
}>("handle_redirects.js", [
    "handler",
    "prefersMarkdown",
    "MARKDOWN_VERSIONS",
    "MARKDOWN_SECTIONS",
    "markdownPageRegex",
]);

const CLAUDE_CODE = "text/markdown, text/html, */*";
const CURSOR = "text/markdown, text/plain;q=0.9, */*;q=0.8";
const CHROME = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8";

async function get(uri: string, accept?: string): Promise<string> {
    const request: Request = { uri, headers: accept ? { accept: { value: accept } } : {} };
    const result = await edge.handler({ request });
    return "statusCode" in result ? `301 ${result.headers.location.value}` : result.uri;
}

test("prefersMarkdown follows what agents and browsers send", () => {
    const yes = [
        CLAUDE_CODE,
        CURSOR,
        "text/markdown,text/html;q=0.9,application/xhtml+xml;q=0.8,*/*;q=0.5",
        "text/markdown;q=1.0, text/x-markdown;q=0.9, text/plain;q=0.8, text/html;q=0.7, */*;q=0.1",
        "text/markdown, text/html;q=0.9, application/xhtml+xml;q=0.9, application/xml;q=0.8, */*;q=0.7",
        "TEXT/MARKDOWN",
    ];
    const no = [CHROME, "*/*", "text/html", "text/markdown;q=0", "text/html, text/markdown;q=0.5", "", undefined];
    for (const accept of yes) {
        assert.equal(edge.prefersMarkdown(accept), true, String(accept));
    }
    for (const accept of no) {
        assert.equal(edge.prefersMarkdown(accept), false, String(accept));
    }
});

test("a page request that prefers Markdown gets the copy at the same URL", async () => {
    assert.equal(
        await get("/7.2/client-api/session/loading-entities", CLAUDE_CODE),
        "/7.2/client-api/session/loading-entities.md"
    );
    assert.equal(
        await get("/7.2/client-api/session/loading-entities", CHROME),
        "/7.2/client-api/session/loading-entities/index.html"
    );
    assert.equal(await get("/7.2", CURSOR), "/7.2.md");
    assert.equal(await get("/guides/some-guide", CLAUDE_CODE), "/guides/some-guide.md");
});

test("negotiated redirects keep the page URL; the client repeats its Accept header", async () => {
    assert.equal(
        await get("/client-api/session/loading-entities", CLAUDE_CODE),
        "301 /7.2/client-api/session/loading-entities"
    );
    assert.equal(await get("/7.2/old-page", CLAUDE_CODE), "301 /7.2/new-page");
});

test("explicit .md URLs go through versioning and redirects and keep their suffix", async () => {
    assert.equal(await get("/7.2/client-api/x.python.md"), "/7.2/client-api/x.python.md");
    assert.equal(await get("/client-api/x.md"), "301 /7.2/client-api/x.md");
    assert.equal(await get("/client-api/x.nodejs.md"), "301 /7.2/client-api/x.nodejs.md");
    assert.equal(await get("/7.2/old-page.java.md"), "301 /7.2/new-page.java.md");
    assert.equal(await get("/7.2/client-api/x/.md"), "301 /7.2/client-api/x.md");
    assert.equal(await get("/guides/old-guide.md"), "301 /guides/new-guide.md");
});

test("versions and sections without copies keep serving HTML", async () => {
    assert.equal(await get("/5.4/client-api/x", CLAUDE_CODE), "/5.4/client-api/x/index.html");
    assert.equal(await get("/templates/panel", CLAUDE_CODE), "/templates/panel/index.html");
    assert.equal(await get("/5.4/client-api/x.md"), "/5.4/client-api/x.md");
});

test("a request without headers gets HTML", async () => {
    const result = await edge.handler({ request: { uri: "/7.2/client-api/x" } as Request });
    assert.equal("uri" in result && result.uri, "/7.2/client-api/x/index.html");
});

test("static files, the skill included, pass through untouched", async () => {
    assert.equal(await get("/skills/ravendb/SKILL.md", CLAUDE_CODE), "/skills/ravendb/SKILL.md");
    assert.equal(
        await get("/skills/ravendb/references/rql-cheatsheet.md"),
        "/skills/ravendb/references/rql-cheatsheet.md"
    );
    assert.equal(await get("/assets/js/main.js", CLAUDE_CODE), "/assets/js/main.js");
    assert.equal(await get("/llms.txt", CLAUDE_CODE), "/llms.txt");
});

test("the edge serves Markdown exactly where markdown-export writes it", () => {
    assert.deepEqual([...edge.MARKDOWN_VERSIONS], [...MARKDOWN_VERSIONS]);
    assert.deepEqual([...edge.MARKDOWN_SECTIONS], [...MARKDOWN_SECTIONS]);
    for (const language of [null, ...DOCS_LANGUAGES]) {
        const file = markdownPath("/7.2/client-api/x", language);
        assert.equal(edge.markdownPageRegex.exec(file)?.[1], "/7.2/client-api/x", file);
    }
});

async function robotsTag(uri: string, contentType: string): Promise<string | undefined> {
    const response = { headers: { "content-type": { value: contentType } } as Record<string, { value: string }> };
    const event = { context: { eventType: "viewer-response" }, request: { uri, headers: {} }, response };
    await edge.handler(event as unknown as { request: Request });
    return response.headers["x-robots-tag"]?.value;
}

test("on viewer-response, Markdown (copies and the skill) is noindex; pages are not", async () => {
    assert.equal(await robotsTag("/7.2/client-api/x.python.md", "text/markdown; charset=utf-8"), "noindex");
    assert.equal(await robotsTag("/7.2/client-api/x", "text/markdown; charset=utf-8"), "noindex");
    assert.equal(await robotsTag("/skills/ravendb/SKILL.md", "text/markdown; charset=utf-8"), "noindex");
    assert.equal(await robotsTag("/7.2/client-api/x", "text/html"), undefined);
});
