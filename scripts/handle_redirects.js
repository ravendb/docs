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

async function handler(event) {
    const request = event.request;
    const uri = request.uri;

    if (uri.startsWith("/cloud") || uri.startsWith("/guides") || uri.startsWith("/templates")) {
        return request;
    }

    if (staticAssetRegex.test(uri)) {
        return request;
    }

    const versionMatch = uri.match(versionRegex);

    let version, versionlessUri, targetUri;
    let redirectRequired = false;

    if (versionMatch) {
        version = versionMatch[1];
        versionlessUri = versionMatch[2] || "/";
    } else {
        version = defaultVersion;
        versionlessUri = uri;
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
        return {
            statusCode: 301,
            statusDescription: "Moved Permanently",
            headers: {
                location: { value: targetUri },
            },
        };
    }

    return request;
}
