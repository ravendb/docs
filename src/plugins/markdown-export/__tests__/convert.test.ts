import { test } from "node:test";
import assert from "node:assert/strict";
import fs from "node:fs";
import os from "node:os";
import path from "node:path";

import { createConverter, type DocTarget } from "../lib/convert.js";
import { buildDocument } from "../lib/document.js";
import { defaultLanguageOf, markdownPath } from "../lib/urls.js";

const ORIGIN = "https://docs.ravendb.net";

/** A two-page site: a shell with C# and Python partials, and a page it links to. */
function fixtureSite() {
    const siteDir = fs.mkdtempSync(path.join(os.tmpdir(), "markdown-export-"));
    const docs = path.join(siteDir, "docs");
    const files: Record<string, string> = {
        "docs/session/loading.mdx": [
            "---",
            'title: "Loading Entities"',
            'supported_languages: ["csharp", "python"]',
            "see_also:",
            '  - title: "Other"',
            '    link: "session/other"',
            '    source: "docs"',
            "---",
            "",
            'import LanguageSwitcher from "@site/src/components/LanguageSwitcher";',
            'import LanguageContent from "@site/src/components/LanguageContent";',
            "import LoadingCsharp from './content/_loading-csharp.mdx';",
            "import LoadingPython from './content/_loading-python.mdx';",
            "",
            "<LanguageSwitcher supportedLanguages={frontMatter.supported_languages} />",
            "",
            '<LanguageContent language="csharp">',
            "    <LoadingCsharp />",
            "</LanguageContent>",
            "",
            '<LanguageContent language="python">',
            "    <LoadingPython />",
            "</LanguageContent>",
        ].join("\n"),
        "docs/session/content/_loading-csharp.mdx": [
            'import Admonition from "@theme/Admonition";',
            'import Tabs from "@theme/Tabs";',
            'import TabItem from "@theme/TabItem";',
            'import CodeBlock from "@theme/CodeBlock";',
            "",
            '<Admonition type="note" title="Before you start">',
            "",
            "See [other](../other.mdx#setup) and [this section](#load).",
            "",
            "</Admonition>",
            "",
            '<Panel heading="Load">',
            "",
            "<Tabs groupId='languages'>",
            '<TabItem value="Sync" label="Sync">',
            '<CodeBlock language="csharp">',
            '{`var e = session.Load<Employee>("employees/1");`}',
            "</CodeBlock>",
            "</TabItem>",
            '<TabItem value="Async" label="Async">',
            '<CodeBlock language="csharp">',
            '{`var e = await asyncSession.LoadAsync<Employee>("employees/1");`}',
            "</CodeBlock>",
            "</TabItem>",
            "</Tabs>",
            "",
            "</Panel>",
        ].join("\n"),
        "docs/session/content/_loading-python.mdx": [
            "See [other](../other.mdx#setup).",
            "",
            "```python",
            'employee = session.load("employees/1", Employee)',
            "```",
        ].join("\n"),
        "docs/session/other.mdx": [
            "---",
            "title: Other",
            "---",
            "",
            'import LanguageContent from "@site/src/components/LanguageContent";',
            "",
            '<LanguageContent language="csharp">',
            "",
            "## Setup {#setup}",
            "",
            "C# setup.",
            "",
            "</LanguageContent>",
            "",
            '<LanguageContent language="python">',
            "",
            "## Setup {#setup}",
            "",
            "Python setup.",
            "",
            "</LanguageContent>",
        ].join("\n"),
    };
    for (const [rel, text] of Object.entries(files)) {
        fs.mkdirSync(path.dirname(path.join(siteDir, rel)), { recursive: true });
        fs.writeFileSync(path.join(siteDir, rel), text);
    }

    const records: (DocTarget & { file: string })[] = [
        {
            file: path.join(docs, "session", "loading.mdx"),
            route: "/7.2/session/loading",
            languages: [],
            defaultLanguage: null,
        },
        {
            file: path.join(docs, "session", "other.mdx"),
            route: "/7.2/session/other",
            languages: [],
            defaultLanguage: null,
        },
    ];
    const warnings: string[] = [];
    const converter = createConverter(
        {
            origin: ORIGIN,
            siteDir,
            currentVersion: "7.2",
            unversionedRoots: new Set(["guides"]),
            docBySource: (abs) => records.find((r) => r.file === path.normalize(abs)),
            docByRoute: (route) => records.find((r) => r.route === route),
            imageUrl: () => null,
        },
        (kind, message) => warnings.push(`${kind}: ${message}`)
    );
    for (const r of records) {
        Object.assign(r, converter.scan(r.file));
    }
    const page = converter.convert({
        file: records[0].file,
        contentRoot: docs,
        route: records[0].route,
        version: "7.2",
    });
    return { page, warnings };
}

test("a language shell renders one document per language, without the others", () => {
    const { page, warnings } = fixtureSite();
    assert.deepEqual(page.languages, ["csharp", "python"]);
    assert.equal(page.defaultLanguage, "csharp");

    const csharp = page.render("csharp");
    const python = page.render("python");
    assert.match(csharp, /session\.Load<Employee>/);
    assert.doesNotMatch(csharp, /session\.load\(/);
    assert.match(python, /session\.load\("employees\/1", Employee\)/);
    assert.doesNotMatch(python, /Load<Employee>/);
    assert.deepEqual(warnings, []);
});

test("tabs, panels and admonitions become plain Markdown", () => {
    const csharp = fixtureSite().page.render("csharp");
    assert.match(csharp, /^> \*\*Note:\*\* Before you start$/m);
    assert.match(csharp, /^## Load \{#load\}$/m);
    assert.match(csharp, /^### Sync\n\n```csharp\nvar e = session\.Load/m);
    assert.match(csharp, /^### Async\n\n```csharp\nvar e = await asyncSession\.LoadAsync/m);
    assert.doesNotMatch(csharp, /<(Tabs|TabItem|CodeBlock|Panel|Admonition|LanguageContent)/);
});

test("links stay in the reader's language and point at Markdown copies", () => {
    const { page } = fixtureSite();
    assert.match(page.render("csharp"), /\[other\]\(https:\/\/docs\.ravendb\.net\/7\.2\/session\/other\.md#setup\)/);
    assert.match(
        page.render("python"),
        /\[other\]\(https:\/\/docs\.ravendb\.net\/7\.2\/session\/other\.python\.md#setup\)/
    );
    // A link into the same document keeps only its anchor.
    assert.match(page.render("csharp"), /\[this section\]\(#load\)/);
    assert.deepEqual(page.seeAlso("python"), [
        { title: "Other", url: `${ORIGIN}/7.2/session/other.python.md`, path: undefined },
    ]);
});

test("the document header names the language, the other variants and the skill", () => {
    const md = buildDocument({
        title: "Loading Entities",
        description: "Load documents.",
        url: `${ORIGIN}/7.2/session/loading`,
        version: "7.2",
        language: "python",
        variants: [
            { language: "csharp", url: `${ORIGIN}${markdownPath("/7.2/session/loading")}` },
            { language: "python", url: `${ORIGIN}${markdownPath("/7.2/session/loading", "python")}` },
        ],
        agentSkill: "RavenDB publishes an official agent skill.",
        body: "Body.",
        seeAlso: [{ title: "Other", url: `${ORIGIN}/7.2/session/other.python.md` }],
    });
    assert.equal(
        md,
        [
            "---",
            'title: "Loading Entities"',
            'description: "Load documents."',
            'url: "https://docs.ravendb.net/7.2/session/loading"',
            'version: "7.2"',
            'language: "python"',
            "---",
            "",
            "# Loading Entities",
            "",
            "> Client language: Python. Also available for [C#](https://docs.ravendb.net/7.2/session/loading.md).",
            ">",
            "> RavenDB publishes an official agent skill.",
            "",
            "Body.",
            "",
            "## See also",
            "",
            "- [Other](https://docs.ravendb.net/7.2/session/other.python.md)",
            "",
        ].join("\n")
    );
});

test("the default copy is the language the page shows before anyone picks one", () => {
    assert.equal(defaultLanguageOf(["csharp", "python"], ["python", "csharp"]), "csharp");
    assert.equal(defaultLanguageOf(["java", "python"], ["python", "java"]), "java");
    assert.equal(defaultLanguageOf([], ["python"]), "python");
    assert.equal(defaultLanguageOf(["csharp"], []), null);
});

test("markdownPath names the copies the way the edge expects", () => {
    assert.equal(markdownPath("/7.2/session/loading"), "/7.2/session/loading.md");
    assert.equal(markdownPath("/7.2/session/loading", "python"), "/7.2/session/loading.python.md");
    assert.equal(markdownPath("/7.2/"), "/7.2.md");
    assert.equal(markdownPath("/guides"), "/guides.md");
});
