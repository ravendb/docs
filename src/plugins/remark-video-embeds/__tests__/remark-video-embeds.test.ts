import assert from "node:assert/strict";
import { describe, it } from "node:test";
import { createVideoEmbedChecks, type MarkdownNode } from "../index";

const ID = "qMJfgQicjwk";

type Attribute = NonNullable<MarkdownNode["attributes"]>[number];

function attribute(name: string, value: Attribute["value"]): Attribute {
    return { name, value };
}

function jsx(name: string, attributes: Attribute[], line = 1): MarkdownNode {
    return { type: "mdxJsxFlowElement", name, attributes, children: [], position: { start: { line } } };
}

function video(url: Attribute["value"], line = 1): MarkdownNode {
    return jsx("Video", [attribute("url", url), attribute("title", "Demo")], line);
}

function compile(
    files: Record<string, MarkdownNode[]>,
    options: Parameters<typeof createVideoEmbedChecks>[0] = { checkWithYouTube: false }
) {
    const checks = createVideoEmbedChecks(options);
    const transform = checks.remarkVideoEmbeds();
    for (const [path, children] of Object.entries(files)) {
        transform({ type: "root", children }, { path });
    }
    return checks;
}

function youTubeAnswering(status: number): { fetchImpl: typeof fetch; requests: string[] } {
    const requests: string[] = [];
    return {
        requests,
        fetchImpl: async (url) => {
            requests.push(String(url));
            return new Response(null, { status });
        },
    };
}

function mentions(...expected: string[]) {
    return (error: Error) => expected.every((text) => error.message.includes(text));
}

async function captureWarnings(action: () => Promise<void>): Promise<string[]> {
    const warnings: string[] = [];
    const originalWarn = console.warn;
    console.warn = (message: string) => warnings.push(message);
    try {
        await action();
    } finally {
        console.warn = originalWarn;
    }
    return warnings;
}

describe("remarkVideoEmbeds: <Video>", () => {
    it("accepts a <Video> with a YouTube url", () => {
        compile({ "docs/page.mdx": [video(`https://www.youtube.com/watch?v=${ID}`)] });
    });

    it("rejects a url that is not a YouTube video, naming the file and line", () => {
        assert.throws(
            () => compile({ "docs/page.mdx": [video("https://vimeo.com/123456", 7)] }),
            mentions(`docs/page.mdx:7: <Video> url "https://vimeo.com/123456" is not a YouTube watch or youtu.be link`)
        );
    });

    it("rejects a url written as an expression, since it cannot be checked", () => {
        assert.throws(
            () => compile({ "docs/page.mdx": [video({ value: "videoUrl" })] }),
            mentions("<Video> needs a plain string url")
        );
    });
});

describe("remarkVideoEmbeds: hand-written iframes", () => {
    it("rejects a YouTube iframe written as JSX", () => {
        const iframe = jsx("iframe", [attribute("src", `https://www.youtube.com/embed/${ID}?si=Fxk7alyTQ9bDcf5O`)], 3);
        assert.throws(
            () => compile({ "docs/page.mdx": [iframe] }),
            mentions("docs/page.mdx:3: YouTube iframe, blocked by the CSP in production")
        );
    });

    it("accepts iframes that do not embed YouTube", () => {
        compile({
            "docs/page.mdx": [jsx("iframe", [attribute("src", "https://public.example.com/apps/demo/embed/token")])],
        });
    });
});

describe("videoAvailabilityPlugin", () => {
    it("asks oEmbed once per video, about its canonical watch URL", async () => {
        const youTube = youTubeAnswering(200);
        const checks = compile(
            { "docs/a.mdx": [video(ID)], "docs/b.mdx": [video(`https://youtu.be/${ID}?si=abc`)] },
            { checkWithYouTube: true, fetchImpl: youTube.fetchImpl }
        );
        await checks.videoAvailabilityPlugin().postBuild();

        assert.equal(youTube.requests.length, 1);
        const endpoint = new URL(youTube.requests[0]);
        assert.equal(endpoint.origin + endpoint.pathname, "https://www.youtube.com/oembed");
        assert.equal(endpoint.searchParams.get("url"), `https://www.youtube.com/watch?v=${ID}`);
    });

    it("fails the build on a video YouTube reports as gone, private or not embeddable, naming where it is used", async () => {
        for (const status of [400, 401, 403, 404]) {
            const checks = compile(
                { "docs/a.mdx": [video(ID, 4)], "docs/b.mdx": [video(ID, 9)] },
                { checkWithYouTube: true, fetchImpl: youTubeAnswering(status).fetchImpl }
            );
            await assert.rejects(
                checks.videoAvailabilityPlugin().postBuild(),
                mentions(`${ID} (YouTube answered ${status})`, "used in: docs/a.mdx:4, docs/b.mdx:9")
            );
        }
    });

    it("only warns when YouTube cannot be asked", async () => {
        for (const fetchImpl of [
            youTubeAnswering(429).fetchImpl,
            youTubeAnswering(503).fetchImpl,
            async () => {
                throw new Error("getaddrinfo ENOTFOUND www.youtube.com");
            },
        ]) {
            const checks = compile({ "docs/a.mdx": [video(ID)] }, { checkWithYouTube: true, fetchImpl });
            const warnings = await captureWarnings(() => checks.videoAvailabilityPlugin().postBuild());
            assert.equal(warnings.length, 1);
            assert.ok(warnings[0].includes("could not check with YouTube"), warnings[0]);
        }
    });

    it("stays offline unless asked to check", async () => {
        const youTube = youTubeAnswering(404);
        const checks = compile(
            { "docs/a.mdx": [video(ID)] },
            { checkWithYouTube: false, fetchImpl: youTube.fetchImpl }
        );
        await checks.videoAvailabilityPlugin().postBuild();
        assert.equal(youTube.requests.length, 0);
    });
});
