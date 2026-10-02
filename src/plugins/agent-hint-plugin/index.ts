// Agent-hint plugin: points coding agents that land on any page at the hosted RavenDB skill.
// Emitted as a static pre-body tag, so it is in the server-rendered HTML that agents fetch,
// outside the React root and untouched by hydration.

import type { LoadContext, Plugin } from "@docusaurus/types";
import fs from "fs";
import path from "path";

import { buildAgentHint, readSkillDescription } from "./lib/hint.js";

// Served verbatim from static/, so the file path and the URL path are the same.
const SKILL_PATH = "skills/ravendb/SKILL.md";

export default function agentHintPlugin(context: LoadContext): Plugin {
    const skillFile = path.join(context.siteDir, "static", SKILL_PATH);
    const skillUrl = new URL(SKILL_PATH, context.siteConfig.url).href;

    return {
        name: "agent-hint-plugin",
        getPathsToWatch() {
            return [skillFile];
        },
        injectHtmlTags() {
            const description = readSkillDescription(fs.readFileSync(skillFile, "utf8"));
            return { preBodyTags: [buildAgentHint(skillUrl, description)] };
        },
    };
}
