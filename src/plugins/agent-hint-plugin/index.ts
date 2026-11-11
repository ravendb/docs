// Publishes the agent skill at /skills/ravendb/ and points agents at it from a static pre-body tag.

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

    // Before Docusaurus reads the static directories.
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
