import fs from "fs";
import path from "path";

const HASHED = /^(.*)-([0-9a-f]{6,})(\.[A-Za-z0-9]+)$/;

/**
 * Maps a source image to the URL the build published it under. The bundler renames images to
 * name-contenthash.ext under assets/images, a hash we cannot recompute, so the lookup goes by
 * name and, when several images share one, by content. Small images are inlined into the page as
 * data URIs and have no file of their own; those fall back to the source file at sourceBaseUrl.
 */
export function createImageIndex(
    outDir: string,
    origin: string,
    siteDir: string,
    sourceBaseUrl: string
): (sourcePath: string) => string | null {
    const dir = path.join(outDir, "assets", "images");
    const byName = new Map<string, string[]>();
    let names: string[] = [];
    try {
        names = fs.readdirSync(dir);
    } catch (_) {
        names = [];
    }
    for (const name of names) {
        const m = HASHED.exec(name);
        if (!m) {
            continue;
        }
        const key = (m[1] + m[3]).toLowerCase();
        byName.set(key, [...(byName.get(key) ?? []), name]);
    }

    const published = (sourcePath: string): string | null => {
        const candidates = byName.get(path.basename(sourcePath).toLowerCase()) ?? [];
        if (candidates.length === 0) {
            return null;
        }
        if (candidates.length === 1) {
            return candidates[0];
        }
        const source = fs.readFileSync(sourcePath);
        return (
            candidates.find((c) => {
                const p = path.join(dir, c);
                return fs.statSync(p).size === source.length && fs.readFileSync(p).equals(source);
            }) ?? null
        );
    };

    return (sourcePath) => {
        if (!fs.existsSync(sourcePath)) {
            return null;
        }
        const file = published(sourcePath);
        if (file) {
            return `${origin}/assets/images/${file}`;
        }
        return sourceBaseUrl + path.relative(siteDir, sourcePath).split(path.sep).map(encodeURIComponent).join("/");
    };
}
