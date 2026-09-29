// A remark plugin, not a built-HTML check: LanguageContent leaves other languages' partials out of the HTML.
// The YouTube requests wait for postBuild because during compilation they time out.

import path from "path";

import { isYouTubeUrl, youTubeVideoId, youTubeWatchUrl } from "../../lib/youtube-url/lib/youtube-url.js";

export type MarkdownNode = {
    type: string;
    name?: string | null;
    attributes?: { name?: string; value?: unknown }[];
    children?: MarkdownNode[];
    position?: { start: { line: number } };
};

type VideoAvailability = { status: "ok" | "broken" | "unverified"; reason: string };

const YOUTUBE_TIMEOUT_MS = 10_000;
const LOG_PREFIX = "[video-embeds]";

export function createVideoEmbedChecks({
    checkWithYouTube = process.env.DOCUSAURUS_STRICT === "true",
    fetchImpl = fetch,
}: { checkWithYouTube?: boolean; fetchImpl?: typeof fetch } = {}) {
    const usagesById = new Map<string, string[]>();

    function checkFile(root: MarkdownNode, file?: { path?: string }) {
        const { videos, problems } = findEmbeds(root, relativePath(file?.path));
        if (problems.length > 0) {
            throw new Error(
                `${LOG_PREFIX} ${problems.join("\n")}\nEmbed YouTube with <Video url="..." title="..." /> (see templates/video.mdx).`
            );
        }
        for (const { id, location } of videos) {
            usagesById.set(id, [...(usagesById.get(id) ?? []), location]);
        }
    }

    async function checkVideosWithYouTube() {
        if (checkWithYouTube) {
            await assertVideosPlay(usagesById, fetchImpl);
        }
    }

    return {
        remarkVideoEmbeds: () => checkFile,
        videoAvailabilityPlugin: () => ({ name: "video-availability", postBuild: checkVideosWithYouTube }),
    };
}

function findEmbeds(root: MarkdownNode, source: string) {
    const videos: { id: string; location: string }[] = [];
    const problems: string[] = [];

    for (const node of descendants(root)) {
        const location = `${source}:${node.position?.start.line ?? "?"}`;

        if (isJsxElement(node, "Video")) {
            const url = stringAttribute(node, "url");
            const id = url === undefined ? null : youTubeVideoId(url);
            if (id) {
                videos.push({ id, location });
            } else if (url === undefined) {
                problems.push(`${location}: <Video> needs a plain string url`);
            } else {
                problems.push(
                    `${location}: <Video> url "${url}" is not a YouTube watch or youtu.be link, or a video ID`
                );
            }
        }

        const src = isJsxElement(node, "iframe") ? stringAttribute(node, "src") : undefined;
        if (src && isYouTubeUrl(src)) {
            problems.push(`${location}: YouTube iframe, blocked by the CSP in production: ${src}`);
        }
    }

    return { videos, problems };
}

async function assertVideosPlay(usagesById: Map<string, string[]>, fetchImpl: typeof fetch) {
    const results = await Promise.all(
        [...usagesById].map(async ([id, usages]) => ({ id, usages, ...(await askYouTube(id, fetchImpl)) }))
    );
    const describe = ({ id, usages, reason }: (typeof results)[number]) =>
        `  - ${id} (${reason})\n      used in: ${usages.join(", ")}`;

    const unverified = results.filter(({ status }) => status === "unverified");
    if (unverified.length > 0) {
        console.warn(`${LOG_PREFIX} could not check with YouTube:\n${unverified.map(describe).join("\n")}`);
    }

    const broken = results.filter(({ status }) => status === "broken");
    if (broken.length > 0) {
        throw new Error(
            `${LOG_PREFIX} YouTube will not play these videos in an embed:\n${broken.map(describe).join("\n")}\nReplace or remove the video, or have its owner make it public and allow embedding.`
        );
    }
}

// A 4xx means the video is deleted, private or not embeddable; a timeout, 5xx or 429 says nothing about it.
async function askYouTube(id: string, fetchImpl: typeof fetch): Promise<VideoAvailability> {
    const endpoint = `https://www.youtube.com/oembed?format=json&url=${encodeURIComponent(youTubeWatchUrl(id))}`;
    try {
        const response = await fetchImpl(endpoint, { signal: AbortSignal.timeout(YOUTUBE_TIMEOUT_MS) });
        if (response.ok) {
            return { status: "ok", reason: "YouTube answered 200" };
        }
        const isAboutTheVideo = response.status >= 400 && response.status < 500 && response.status !== 429;
        return { status: isAboutTheVideo ? "broken" : "unverified", reason: `YouTube answered ${response.status}` };
    } catch (error) {
        return { status: "unverified", reason: `YouTube unreachable: ${(error as Error).message}` };
    }
}

function* descendants(node: MarkdownNode): Generator<MarkdownNode> {
    for (const child of node.children ?? []) {
        yield child;
        yield* descendants(child);
    }
}

function isJsxElement(node: MarkdownNode, name: string): boolean {
    return (node.type === "mdxJsxFlowElement" || node.type === "mdxJsxTextElement") && node.name === name;
}

function stringAttribute(node: MarkdownNode, name: string): string | undefined {
    const value = node.attributes?.find((attribute) => attribute.name === name)?.value;
    return typeof value === "string" ? value : undefined;
}

function relativePath(filePath: string | undefined): string {
    return filePath ? path.relative(process.cwd(), filePath).split(path.sep).join("/") : "unknown file";
}
