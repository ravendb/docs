import fs from "fs";
import path from "path";

const HASHED = /^(.*)-([0-9a-f]{6,})(\.[A-Za-z0-9]+)$/;

/** Matches by name and content: an inlined image has no file and must not take another's. */
export function createImageIndex(
    outDir: string,
    origin: string,
    siteDir: string,
    sourceBaseUrl: string
): (sourcePath: string) => string | null {
    const dir = path.join(outDir, "assets", "images");
    const byName = new Map<string, string[]>();
    const names = fs.existsSync(dir) ? fs.readdirSync(dir) : [];
    for (const name of names) {
        const m = HASHED.exec(name);
        if (m) {
            const key = (m[1] + m[3]).toLowerCase();
            byName.set(key, [...(byName.get(key) ?? []), name]);
        }
    }

    const published = (sourcePath: string): string | undefined => {
        const source = fs.readFileSync(sourcePath);
        return (byName.get(path.basename(sourcePath).toLowerCase()) ?? []).find((name) => {
            const file = path.join(dir, name);
            return fs.statSync(file).size === source.length && fs.readFileSync(file).equals(source);
        });
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
