// Agent-hint plugin: publishes the RavenDB agent skill at /skills/ravendb/ and points coding
// agents that land on any page at it. The hint is a static pre-body tag, so it is in the
// server-rendered HTML that agents fetch, outside the React root and untouched by hydration.

import type { LoadContext, Plugin } from "@docusaurus/types";
import fs from "fs";
import path from "path";

import { buildAgentHint, buildStaticUrl, readSkillDescription } from "./lib/hint.js";
import {
    assertSkillSource,
    copySkillTree,
    GENERATED_STATIC_DIR,
    SKILL_SOURCE_DIR,
    SKILL_URL_DIR,
} from "./lib/skill-tree.js";

export default function agentHintPlugin(context: LoadContext): Plugin {
    const sourceDir = path.join(context.siteDir, SKILL_SOURCE_DIR);
    const skillFile = assertSkillSource(sourceDir);
    const skillUrl = buildStaticUrl(context.siteConfig.url, context.baseUrl, `${SKILL_URL_DIR}/SKILL.md`);

    // Runs when the plugin loads, before the static directories are read, and again on every
    // reload getPathsToWatch triggers.
    copySkillTree(sourceDir, path.join(context.siteDir, GENERATED_STATIC_DIR, SKILL_URL_DIR));

    return {
        name: "agent-hint-plugin",
        getPathsToWatch() {
            return [path.join(sourceDir, "**", "*")];
        },
        injectHtmlTags() {
            const description = readSkillDescription(fs.readFileSync(skillFile, "utf8"));
            return { preBodyTags: [buildAgentHint(skillUrl, description)] };
        },
    };
}
