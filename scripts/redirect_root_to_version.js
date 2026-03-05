import cf from 'cloudfront';

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
    }
    else {
        version = defaultVersion;
        versionlessUri = uri;
    }

    try {
        const redirectPath = await kvsHandle.get(versionlessUri);

        if (redirectPath === null) {
            return request;
        }

        const newUri = `/${version}${redirectPath}`;

        return {
            statusCode: 301,
            statusDescription: "Moved Permanently",
            headers: {
                location: { value: newUri }
            }
        };
    }
    catch (_) {
        return request;
    }
}
