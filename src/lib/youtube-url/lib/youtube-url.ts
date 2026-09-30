const YOUTUBE_HOSTS = new Set(["youtube.com", "www.youtube.com"]);

const VIDEO_ID_LENGTH = 11;
const VIDEO_ID_CHARACTERS = new Set("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_");

function isVideoId(value: string | null): value is string {
    return value?.length === VIDEO_ID_LENGTH && [...value].every((character) => VIDEO_ID_CHARACTERS.has(character));
}

function parseUrl(input: string): URL | null {
    try {
        return new URL(input);
    } catch {
        return null;
    }
}

export function youTubeVideoId(input: string): string | null {
    if (isVideoId(input)) {
        return input;
    }

    const url = parseUrl(input);
    if (url?.hostname === "youtu.be") {
        const id = url.pathname.slice(1);
        return isVideoId(id) ? id : null;
    }
    if (url && YOUTUBE_HOSTS.has(url.hostname) && url.pathname === "/watch") {
        const id = url.searchParams.get("v");
        return isVideoId(id) ? id : null;
    }
    return null;
}

export function isYouTubeUrl(input: string): boolean {
    const url = parseUrl(input);
    return url !== null && YOUTUBE_HOSTS.has(url.hostname);
}

export function youTubeEmbedUrl(id: string): string {
    return `https://www.youtube-nocookie.com/embed/${id}?autoplay=1&playsinline=1&rel=0`;
}

export function youTubeWatchUrl(id: string): string {
    return `https://www.youtube.com/watch?v=${id}`;
}

// Exists only for videos uploaded in 720p or higher; lower ones get a grey placeholder.
export function youTubePosterUrl(id: string): string {
    return `https://i.ytimg.com/vi/${id}/maxresdefault.jpg`;
}
