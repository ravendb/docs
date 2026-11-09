import cf from "cloudfront";

const CURRENT_VERSION = "7.2";

function compareVersions(v1, v2) {
    const parts1 = v1.split(".");
    const parts2 = v2.split(".");
    const major1 = parseInt(parts1[0], 10);
    const major2 = parseInt(parts2[0], 10);
    const minor1 = parseInt(parts1[1], 10);
    const minor2 = parseInt(parts2[1], 10);
    if (major1 === major2) {
        if (minor1 > minor2) return 1;
        if (minor1 < minor2) return -1;
        return 0;
    }
    if (major1 > major2) return 1;
    if (major1 < major2) return -1;
    return 0;
}

const kvsHandle = cf.kvs();

const defaultVersion = CURRENT_VERSION;

// `md` is listed so the hosted agent skill under /skills/**.md reaches S3 instead of being
// version-prefixed into a path that does not exist.
const staticAssetRegex =
    /\.(html|css|js|json|jpg|jpeg|png|gif|webp|svg|ico|ttf|otf|woff|woff2|eot|mp4|mp3|webm|avi|mov|pdf|txt|xml|md)$/i;

const versionRegex = /^\/(\d+\.\d+)(\/.*)?/;

// Kept in sync with markdown-export by its edge test.
const MARKDOWN_VERSIONS = ["7.2", "7.1", "6.2"];
const MARKDOWN_SECTIONS = ["cloud", "quill", "guides", "samples"];

const markdownPageRegex = /^(.+?)((?:\.(?:csharp|java|python|php|nodejs))?\.md)$/;

function redirect(targetUrl) {
    return {
        statusCode: 301,
        statusDescription: "Moved Permanently",
        headers: {
            location: { value: targetUrl },
        },
    };
}

function acceptWeight(accept, mediaType) {
    const ranges = accept.toLowerCase().split(",");
    for (let i = 0; i < ranges.length; i++) {
        const params = ranges[i].split(";");
        if (params[0].trim() !== mediaType) continue;
        for (let j = 1; j < params.length; j++) {
            const param = params[j].trim();
            if (param.startsWith("q=")) return parseFloat(param.slice(2)) || 0;
        }
        return 1;
    }
    return -1;
}

// A tie goes to Markdown: Claude Code sends text/markdown and text/html without weights.
function prefersMarkdown(accept) {
    const markdown = acceptWeight(accept || "", "text/markdown");
    return markdown > 0 && markdown >= acceptWeight(accept || "", "text/html");
}

function markMarkdownResponse(event) {
    const response = event.response;
    const contentType = response.headers["content-type"];
    if (contentType && contentType.value.startsWith("text/markdown")) {
        response.headers["x-robots-tag"] = { value: "noindex" };
    }
    return response;
}

// Associated with both viewer-request and viewer-response.
async function handler(event) {
    if (event.context && event.context.eventType === "viewer-response") {
        return markMarkdownResponse(event);
    }
    const request = event.request;
    let uri = request.uri;

    const accept = request.headers && request.headers.accept;
    let markdownSuffix = "";
    let explicitMarkdown = false;
    const markdownMatch = uri.startsWith("/skills/") ? null : uri.match(markdownPageRegex);
    if (markdownMatch) {
        uri = markdownMatch[1];
        markdownSuffix = markdownMatch[2];
        explicitMarkdown = true;
    } else if (staticAssetRegex.test(uri)) {
        return request;
    } else if (accept && prefersMarkdown(accept.value)) {
        markdownSuffix = ".md";
    }
    const redirectSuffix = explicitMarkdown ? markdownSuffix : "";

    function serve(path, hasMarkdown) {
        request.uri = explicitMarkdown || (markdownSuffix && hasMarkdown) ? path + markdownSuffix : path + "/index.html";
        return request;
    }

    const hasTrailingSlash = uri !== "/" && uri.endsWith("/");
    const normalizedUri = hasTrailingSlash ? uri.slice(0, -1) : uri;

    if (normalizedUri.startsWith("/templates")) {
        if (hasTrailingSlash) {
            return redirect(normalizedUri + redirectSuffix);
        }
        return serve(normalizedUri, false);
    }

    if (
        normalizedUri.startsWith("/guides") ||
        normalizedUri.startsWith("/cloud") ||
        normalizedUri.startsWith("/quill") ||
        normalizedUri.startsWith("/samples")
    ) {
        try {
            const redirectData = await kvsHandle.get(normalizedUri);
            const redirectJsonValue = JSON.parse(redirectData);
            if (redirectJsonValue.targetUrl) {
                return redirect(redirectJsonValue.targetUrl + redirectSuffix);
            }
        } catch (_) {
            // No redirect rule found
        }
        if (hasTrailingSlash) {
            return redirect(normalizedUri + redirectSuffix);
        }
        return serve(normalizedUri, MARKDOWN_SECTIONS.indexOf(normalizedUri.split("/")[1]) >= 0);
    }

    const versionMatch = normalizedUri.match(versionRegex);

    let version, versionlessUri, targetUri;
    let redirectRequired = hasTrailingSlash;

    if (versionMatch) {
        version = versionMatch[1];
        versionlessUri = versionMatch[2] || "";
    } else {
        version = defaultVersion;
        versionlessUri = normalizedUri === "/" ? "" : normalizedUri;
        redirectRequired = true;
    }

    targetUri = `/${version}${versionlessUri}`;

    let current = versionlessUri;
    while (true) {
        let rule;
        try {
            const redirectData = await kvsHandle.get(current);
            rule = JSON.parse(redirectData);
        } catch (_) {
            break; // no rule → terminal
        }
        if (rule.minimumVersion && compareVersions(version, rule.minimumVersion) < 0) break;
        current = rule.targetUrl;
    }
    if (current !== versionlessUri) {
        targetUri = `/${version}${current}`;
        redirectRequired = true;
    }

    if (redirectRequired) {
        return redirect(targetUri + redirectSuffix);
    }

    return serve(normalizedUri, MARKDOWN_VERSIONS.indexOf(version) >= 0);
}
