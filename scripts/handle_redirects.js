import cf from "cloudfront";

const kvsHandle = cf.kvs();

const defaultVersion = "7.2";

const staticAssetRegex =
    /\.(html|css|js|jpg|jpeg|png|gif|webp|svg|ico|ttf|otf|woff|woff2|eot|mp4|mp3|webm|avi|mov|pdf|txt|xml)$/i;

const versionRegex = /^\/(\d+\.\d+)(\/.*)?/;

function compareVersions(v1, v2) {
    const parts1 = v1.split(".");
    const parts2 = v2.split(".");

    const major1 = parseInt(parts1[0]);
    const major2 = parseInt(parts2[0]);

    const minor1 = parseInt(parts1[1]);
    const minor2 = parseInt(parts2[1]);

    if (major1 === major2) {
        if (minor1 > minor2) {
            return 1;
        }
        if (minor1 < minor2) {
            return -1;
        }

        return 0;
    }

    if (major1 > major2) {
        return 1;
    }
    if (major1 < major2) {
        return -1;
    }
}

function redirect(targetUrl) {
    return {
        statusCode: 301,
        statusDescription: "Moved Permanently",
        headers: {
            location: { value: targetUrl },
        },
    };
}

async function handler(event) {
    const request = event.request;
    const uri = request.uri;

    if (staticAssetRegex.test(uri)) {
        return request;
    }

    const hasTrailingSlash = uri !== "/" && uri.endsWith("/");
    const normalizedUri = hasTrailingSlash ? uri.slice(0, -1) : uri;

    if (normalizedUri.startsWith("/templates")) {
        if (hasTrailingSlash) {
            return redirect(normalizedUri);
        }
        request.uri = uri + "/index.html";
        return request;
    }

    if (normalizedUri.startsWith("/guides") || normalizedUri.startsWith("/cloud")) {
        try {
            const redirectData = await kvsHandle.get(normalizedUri);
            const redirectJsonValue = JSON.parse(redirectData);
            if (redirectJsonValue.targetUrl) {
                return redirect(redirectJsonValue.targetUrl);
            }
        } catch (_) {
            // No redirect rule found
        }
        if (hasTrailingSlash) {
            return redirect(normalizedUri);
        }
        request.uri = uri + "/index.html";
        return request;
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

    try {
        const redirectData = await kvsHandle.get(versionlessUri);
        const redirectJsonValue = JSON.parse(redirectData);
        const redirectPath = redirectJsonValue.targetUrl;
        const redirectMinimumVersion = redirectJsonValue.minimumVersion;

        const isVersionSupported = compareVersions(version, redirectMinimumVersion) >= 0;

        if (redirectPath && isVersionSupported) {
            targetUri = `/${version}${redirectPath}`;
            redirectRequired = true;
        }
    } catch (_) {
        // If we fail to obtain a redirect rule, we ignore it and continue with the default behavior
    }

    if (redirectRequired) {
        return redirect(targetUri);
    }

    request.uri = normalizedUri + "/index.html";
    return request;
}
