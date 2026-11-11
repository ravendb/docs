import { test } from "node:test";
import assert from "node:assert/strict";

import { buildAgentHint, buildMarkdownHintLine } from "../../agent-hint-plugin/lib/hint.js";
import { addMarkdownLinks, alternateTags, summarizePage } from "../lib/html.js";

const C_SHARP = { url: "https://docs.ravendb.net/7.2/x.md", label: "C#" };
const PYTHON = { url: "https://docs.ravendb.net/7.2/x.python.md", label: "Python" };

test("alternates go into <head> and the hint line into the agent hint", () => {
    const html =
        "<html><head><title>X</title></head><body>" +
        buildAgentHint("https://docs.ravendb.net/skills/ravendb/SKILL.md", "d") +
        "<main>page</main></body></html>";
    const result = addMarkdownLinks(html, alternateTags([C_SHARP, PYTHON]), buildMarkdownHintLine([C_SHARP, PYTHON]));
    assert.equal(result.hint, true);
    assert.match(
        result.html,
        /<link rel="alternate" type="text\/markdown" href="https:\/\/docs\.ravendb\.net\/7\.2\/x\.md" title="C#"><link rel="alternate" type="text\/markdown" href="https:\/\/docs\.ravendb\.net\/7\.2\/x\.python\.md" title="Python"><\/head>/
    );
    assert.match(
        result.html,
        /one file per client language: <a href="https:\/\/docs\.ravendb\.net\/7\.2\/x\.md" tabindex="-1">C#<\/a>, <a [^>]*>Python<\/a>\.<\/p><\/blockquote>/
    );
});

test("a minified page still gets both", () => {
    const html =
        "<head><meta charset=utf-8></head><body><blockquote data-agent-hint aria-hidden=true><h2>x</h2></blockquote>";
    const result = addMarkdownLinks(
        html,
        alternateTags([{ url: "u", label: null }]),
        buildMarkdownHintLine([{ url: "u", label: null }])
    );
    assert.equal(result.hint, true);
    assert.ok(result.html.includes('<link rel="alternate" type="text/markdown" href="u"></head>'));
    assert.ok(result.html.includes("available as Markdown at <a"));
});

test("summarizePage reads a listing page's title, description and links", () => {
    const html =
        '<head><title>Guides | RavenDB Documentation</title><meta name=description content="All the guides &amp; more"></head>' +
        '<body><nav><a href="/7.2">Docs</a></nav><main><a href="/guides/a"><h3>Guide <b>A</b></h3></a>' +
        '<a href="#top">Top</a><a href=/guides/b>Guide B</a><a href="/guides/a">Guide A again</a></main></body>';
    assert.deepEqual(summarizePage(html), {
        title: "Guides",
        description: "All the guides & more",
        links: [
            { text: "Guide A", href: "/guides/a" },
            { text: "Guide B", href: "/guides/b" },
        ],
    });
});
