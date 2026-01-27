import type { Plugin } from "@docusaurus/types";
import path from "path";
import fs from "fs";
import matter from "gray-matter";
import type { IconName } from "../typescript/iconName";
const yaml = require("js-yaml");

export interface Guide {
    id: string;
    title: string;
    permalink: string;
    tags: { label: string; permalink: string }[];
    lastUpdatedAt: number;
    description?: string;
    image?: string | { light: string; dark: string };
    icon?: IconName;
}

export interface PluginData {
    guides: Guide[];
    tags: Array<{
        label: string;
        permalink: string;
        key: string;
        count: number;
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

const recentGuidesPlugin: Plugin = function recentGuidesPlugin(
    context,
    _options,
) {
    return {
        name: "recent-guides-plugin",
        async loadContent() {
            const guidesDir = path.join(context.siteDir, "guides");

            if (!fs.existsSync(guidesDir)) {
                return [];
            }

            const tagsYmlPath = path.join(guidesDir, "tags.yml");
            let predefinedTags: Record<
                string,
                { label: string; permalink: string }
            > = {};
            if (fs.existsSync(tagsYmlPath)) {
                try {
                    const fileContent = fs.readFileSync(tagsYmlPath, "utf8");
                    predefinedTags = (yaml.load(fileContent) as any) || {};
                } catch (e) {
                    // eslint-disable-next-line no-console
                    console.error("Failed to load tags.yml", e);
                }
            }

            const tagCounts: Record<string, number> = {};

            const files = getFiles(guidesDir).filter((f) =>
                /\.(md|mdx)$/.test(f),
            );

            const guides = files.map((filePath) => {
                const fileContent = fs.readFileSync(filePath, "utf-8");
                const { data } = matter(fileContent);
                const stats = fs.statSync(filePath);

                const relativePath = path.relative(guidesDir, filePath);

                const relativePathNormalized = relativePath
                    .split(path.sep)
                    .join("/");
                const baseName = relativePathNormalized.replace(
                    /\.(md|mdx)$/,
                    "",
                );

                const slug = baseName.endsWith("/index")
                    ? baseName.replace(/\/index$/, "")
                    : baseName;
                const permalink = `/guides/${slug === "index" ? "" : slug}`;

                let tags = data.tags || [];
                if (!Array.isArray(tags)) {tags = [];}

                tags.forEach((tag: string) => {
                    tagCounts[tag] = (tagCounts[tag] || 0) + 1;
                });

                const formattedTags = tags.map((tag: string) => {
                    const definedTag = predefinedTags[tag];
                    if (definedTag) {
                        return {
                            label: definedTag.label,
                            permalink: path.posix.join(
                                "/guides/tags",
                                definedTag.permalink || tag.toLowerCase(),
                            ),
                        };
                    }
                    return {
                        label: tag,
                        permalink: `/guides/tags/${tag.toLowerCase().replace(/\s+/g, "-")}`,
                    };
                });

                return {
                    id: path.basename(filePath, path.extname(filePath)),
                    title:
                        data.title ||
                        path.basename(filePath, path.extname(filePath)),
                    permalink: data.slug || permalink,
                    tags: formattedTags,
                    lastUpdatedAt: Math.floor(stats.mtimeMs / 1000),
                    description: data.description,
                    image: data.image,
                    icon: data.icon,
                };
            });

            const allTags = Object.entries(predefinedTags).map(
                ([key, value]) => ({
                    ...value,
                    key,
                    count: tagCounts[key] || 0,
                }),
            );

            return {
                guides: guides.sort(
                    (a, b) => b.lastUpdatedAt - a.lastUpdatedAt,
                ),
                tags: allTags,
            };
        },
        async contentLoaded({ content, actions }) {
            const { setGlobalData } = actions;
            setGlobalData(content);
        },
    };
};

export default recentGuidesPlugin;
