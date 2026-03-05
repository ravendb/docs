import cf from "cloudfront";

const kvsHandle = cf.kvs();

const defaultVersion = "7.2";

const staticAssetRegex =
    /\.(html|css|js|jpg|jpeg|png|gif|webp|svg|ico|ttf|otf|woff|woff2|eot|mp4|mp3|webm|avi|mov|pdf|txt|xml)$/i;

const versionRegex = /^\/(\d+\.\d+)(\/.*)?/;

async function handler(event) {
    const request = event.request;
    const uri = request.uri;

    if (uri.startsWith("/cloud") || uri.startsWith("/guides")) {
        return request;
    }

    if (staticAssetRegex.test(uri)) {
        return request;
    }

    const versionMatch = uri.match(versionRegex);

    let version, versionlessUri;
    if (versionMatch) {
        version = versionMatch[1];
        versionlessUri = versionMatch[2];
    } else {
        version = defaultVersion;
        versionlessUri = uri;
    }

    try {
        const redirectData = await kvsHandle.get(versionlessUri);
        const redirectJsonValue = JSON.parse(redirectData).value;
        const redirectPath = redirectJsonValue.key;
        const redirectMinimumVersion = redirectJsonValue.minimumVersion;

        const isVersionSupported = version.localeCompare(redirectMinimumVersion, "en", {
            numeric: true
        }) >= 0;

        if (redirectPath === null || !isVersionSupported) {
            return request;
        }

        const newUri = `/${version}${redirectPath}`;

        return {
            statusCode: 301,
            statusDescription: "Moved Permanently",
            headers: {
                location: { value: newUri },
            },
        };
    } catch (_) {
        return request;
    }
}
