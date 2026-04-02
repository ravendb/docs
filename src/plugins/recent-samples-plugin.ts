import type { Plugin } from "@docusaurus/types";
import path from "path";
import fs from "fs";
import matter from "gray-matter";
const yaml = require("js-yaml");

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

                return {
                    id: path.basename(filePath, path.extname(filePath)),
                    title: data.title,
                    permalink: data.slug || permalink,
                    tags: formattedTags,
                    description: data.description,
                    image: data.image,
                    img_alt: data.img_alt,
                };
            });

            const allTags: Array<{
                label: string;
                key: string;
                count: number;
                category: string;
            }> = [];

            Object.entries(tagsByCategory).forEach(([category, tags]) => {
                Object.entries(tags).forEach(([key, value]) => {
                    allTags.push({
                        label: value.label,
                        key,
                        count: tagCounts[key] || 0,
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
