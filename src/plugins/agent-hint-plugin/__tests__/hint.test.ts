import { test } from "node:test";
import assert from "node:assert/strict";
import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

import { buildAgentHint, buildStaticUrl, escapeHtml, readSkillDescription } from "../lib/hint.js";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const SKILL_FILE = path.join(__dirname, "..", "..", "..", "..", "static", "skills", "ravendb", "SKILL.md");
const SKILL_URL = "https://docs.ravendb.net/skills/ravendb/SKILL.md";

test("readSkillDescription reads the hosted skill's description", () => {
    const description = readSkillDescription(fs.readFileSync(SKILL_FILE, "utf8"));
    assert.match(description, /RavenDB/);
});

test("readSkillDescription accepts CRLF frontmatter", () => {
    const skill = "---\r\nname: x\r\ndescription: Read this first.\r\n---\r\n\r\n# X\r\n";
    assert.equal(readSkillDescription(skill), "Read this first.");
});

test("readSkillDescription accepts a leading byte-order mark", () => {
    assert.equal(readSkillDescription("\uFEFF---\ndescription: Read this first.\n---\n"), "Read this first.");
});

test("readSkillDescription throws without frontmatter", () => {
    assert.throws(() => readSkillDescription("# No frontmatter\n"), /no frontmatter/);
});

test("readSkillDescription throws when description is missing or empty", () => {
    assert.throws(() => readSkillDescription("---\nname: x\n---\n"), /no description/);
    assert.throws(() => readSkillDescription("---\nname: x\ndescription: ''\n---\n"), /no description/);
});

test("buildAgentHint carries the skill URL and description", () => {
    const html = buildAgentHint(SKILL_URL, "Read this before writing any RavenDB query.");
    assert.ok(html.includes(`href="${SKILL_URL}"`));
    assert.ok(html.includes(`>${SKILL_URL}</a>`));
    assert.ok(html.includes("<p>Read this before writing any RavenDB query.</p>"));
});

test("buildAgentHint is hidden from people and kept out of the tab order", () => {
    const opening = buildAgentHint(SKILL_URL, "d").match(/^<blockquote [^>]*>/)?.[0] ?? "";
    assert.ok(opening.includes('aria-hidden="true"'));
    assert.match(opening, /style="[^"]*clip:rect\(0,0,0,0\)/);
    assert.match(buildAgentHint(SKILL_URL, "d"), /<a [^>]*tabindex="-1"/);
});

test("buildStaticUrl resolves against the site url and baseUrl", () => {
    const skillPath = "skills/ravendb/SKILL.md";
    assert.equal(buildStaticUrl("https://docs.ravendb.net/", "/", skillPath), SKILL_URL);
    assert.equal(buildStaticUrl("https://docs.ravendb.net", "/", skillPath), SKILL_URL);
    assert.equal(
        buildStaticUrl("https://example.com", "/docs/", skillPath),
        "https://example.com/docs/skills/ravendb/SKILL.md"
    );
});

test("buildAgentHint escapes markup in the description", () => {
    const html = buildAgentHint(SKILL_URL, `<script>alert("x")</script> & co`);
    assert.ok(!html.includes("<script>"));
    assert.ok(html.includes("&lt;script&gt;alert(&quot;x&quot;)&lt;/script&gt; &amp; co"));
});

test("escapeHtml escapes all five significant characters", () => {
    assert.equal(escapeHtml(`&<>"'`), "&amp;&lt;&gt;&quot;&#39;");
});
