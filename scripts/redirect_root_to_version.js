function handler(event) {
    var request = event.request;
    var uri = request.uri;

    if (uri.startsWith('/cloud')) {
        return request;
    }

    var defaultVersion = "7.2";

    var staticAssetRegex = /\.(html|css|js|jpg|jpeg|png|gif|webp|svg|ico|ttf|otf|woff|woff2|eot|mp4|mp3|webm|avi|mov|pdf|txt|xml)$/i;

    if (staticAssetRegex.test(uri)) {
        return request;
    }

    var versionRegex = /^\/\d+\.\d+/;

    if (!versionRegex.test(uri)) {
        var newUri = `/${defaultVersion}${uri}`;

        return {
            statusCode: 301,
            statusDescription: 'Moved Permanently',
            headers: {
                location: { value: newUri }
            }
        };
    }

    return request;
}
