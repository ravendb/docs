import type { Plugin } from "@docusaurus/types";
import path from "path";
import fs from "fs";
import matter from "gray-matter";
const yaml = require("js-yaml");

// Build-time-only helper for reading intrinsic image dimensions. It's a transitive
// dependency of the image pipeline, so the require is guarded: if it's ever absent,
// sample cards simply ship without width/height rather than failing the build.
let imageSize: ((input: Buffer) => { width?: number; height?: number }) | null = null;
try {
    const mod = require("image-size");
    imageSize = typeof mod === "function" ? mod : mod.imageSize || mod.default || null;
} catch {
    imageSize = null;
}

interface TagDefinition {
    label: string;
    description?: string;
}

interface TagsByCategory {
    [category: string]: {
        [tagKey: string]: TagDefinition;
    };
}

export interface Sample {
    id: string;
    title: string;
    permalink: string;
    tags: { label: string; key: string; category: string }[];
    description?: string;
    image?: string;
    imgAlt?: string;
    imgWidth?: number;
    imgHeight?: number;
}

export interface PluginData {
    samples: Sample[];
    tags: Array<{
        label: string;
        key: string;
        count: number;
        category: string;
    }>;
}

function getFiles(dir: string, files: string[] = []) {
    const fileList = fs.readdirSync(dir);
    for (const file of fileList) {
        const name = path.join(dir, file);
        if (fs.statSync(name).isDirectory()) {
            getFiles(name, files);
        } else {
            files.push(name);
        }
    }
    return files;
}

export default function recentSamplesPlugin(context, _options): Plugin {
    return {
        name: "recent-samples-plugin",
        async loadContent() {
            const samplesDir = path.join(context.siteDir, "samples");

            if (!fs.existsSync(samplesDir)) {
                return [];
            }

            const tagsByCategory: TagsByCategory = {};
            const tagsDir = path.join(samplesDir, "tags");

            if (fs.existsSync(tagsDir)) {
                const categoryFiles = fs.readdirSync(tagsDir).filter((f) => f.endsWith(".yml"));
                for (const file of categoryFiles) {
                    const category = path.basename(file, ".yml");
                    const filePath = path.join(tagsDir, file);
                    try {
                        const fileContent = fs.readFileSync(filePath, "utf8");
                        tagsByCategory[category] = (yaml.load(fileContent) as any) || {};
                    } catch (e) {
                        console.error(`Failed to load tags/${file}`, e);
                    }
                }
            }

            const tagCounts: Record<string, number> = {};

            const files = getFiles(samplesDir)
                .filter((f) => /\.(md|mdx)$/.test(f))
                .filter((f) => {
                    const relativePath = path.relative(samplesDir, f);
                    const normalized = relativePath.split(path.sep).join("/");
                    return normalized !== "home.mdx";
                });

            const samples = files.map((filePath) => {
                const fileContent = fs.readFileSync(filePath, "utf-8");
                const { data } = matter(fileContent);

                const relativePath = path.relative(samplesDir, filePath);
                const relativePathNormalized = relativePath.split(path.sep).join("/");
                const baseName = relativePathNormalized.replace(/\.(md|mdx)$/, "");

                const slug = baseName.endsWith("/index") ? baseName.replace(/\/index$/, "") : baseName;
                const permalink = `/samples/${slug === "index" ? "" : slug}`;

                const allTagsArray: Array<{ key: string; category: string }> = [];

                const challengesSolutionsTags = data.challenges_solutions_tags;

                if (Array.isArray(challengesSolutionsTags)) {
                    challengesSolutionsTags.forEach((tag: string) => {
                        allTagsArray.push({ key: tag, category: "challenges-solutions" });
                    });
                }

                const featureTags = data.feature_tags;
                if (Array.isArray(featureTags)) {
                    featureTags.forEach((tag: string) => {
                        allTagsArray.push({ key: tag, category: "feature" });
                    });
                }

                const techStackTags = data.tech_stack_tags;
                if (Array.isArray(techStackTags)) {
                    techStackTags.forEach((tag: string) => {
                        allTagsArray.push({ key: tag, category: "tech-stack" });
                    });
                }

                allTagsArray.forEach(({ key }) => {
                    tagCounts[key] = (tagCounts[key] || 0) + 1;
                });

                const formattedTags = allTagsArray.map(({ key, category }) => {
                    const categoryTags = tagsByCategory[category];
                    const definedTag = categoryTags[key];

                    return {
                        label: definedTag?.label || key,
                        key,
                        category,
                    };
                });

                let imgWidth: number | undefined;
                let imgHeight: number | undefined;
                if (imageSize && typeof data.image === "string") {
                    try {
                        const absImagePath = path.join(context.siteDir, "static", data.image);
                        const dimensions = imageSize(fs.readFileSync(absImagePath));
                        imgWidth = dimensions.width;
                        imgHeight = dimensions.height;
                    } catch {
                        // Cover dimensions are optional; ignore missing/unreadable images.
                    }
                }

                return {
                    id: path.basename(filePath, path.extname(filePath)),
                    title: data.title,
                    permalink: data.slug || permalink,
                    tags: formattedTags,
                    description: data.description,
                    image: data.image,
                    img_alt: data.img_alt,
                    imgWidth,
                    imgHeight,
                };
            });

            let orderList: string[] = [];
            const orderFilePath = path.join(samplesDir, "order.yml");
            if (fs.existsSync(orderFilePath)) {
                try {
                    const loaded = yaml.load(fs.readFileSync(orderFilePath, "utf8"));
                    if (Array.isArray(loaded)) {
                        orderList = loaded.map((id) => String(id));
                    }
                } catch (e) {
                    console.error("Failed to load samples/order.yml", e);
                }
            }

            const orderIndex = new Map<string, number>();
            orderList.forEach((id, index) => orderIndex.set(id, index));
            const rankOf = (id: string) => (orderIndex.has(id) ? orderIndex.get(id)! : Number.MAX_SAFE_INTEGER);

            samples.sort((a, b) => {
                const rankDiff = rankOf(a.id) - rankOf(b.id);
                return rankDiff !== 0 ? rankDiff : (a.title || "").localeCompare(b.title || "");
            });

            const allTags: Array<{
                label: string;
                key: string;
                count: number;
                category: string;
            }> = [];

            Object.entries(tagsByCategory).forEach(([category, tags]) => {
                Object.entries(tags).forEach(([key, value]) => {
                    const count = tagCounts[key] || 0;
                    if (count === 0) {
                        return;
                    }

                    allTags.push({
                        label: value.label,
                        key,
                        count,
                        category,
                    });
                });
            });

            return {
                samples: samples,
                tags: allTags,
            };
        },
        async contentLoaded({ content, actions }) {
            const { setGlobalData } = actions;
            setGlobalData(content);
        },
    };
}
