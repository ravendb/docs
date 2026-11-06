import fs from "fs";
import path from "path";

/**
 * The skill is authored in ravendb/skills, installed as the @ravendb/skills npm dependency
 * (from GitHub, pinned to a commit). Only its skills/ravendb subtree is published: listing the
 * package root in staticDirectories would also publish its README, LICENSE and package.json.
 */
export const SKILL_SOURCE_DIR = path.join("node_modules", "@ravendb", "skills", "skills", "ravendb");

/** Gitignored, listed in staticDirectories, so start, build and their :current variants serve it. */
export const GENERATED_STATIC_DIR = path.join(".generated", "static");

/** URL path of the skill, outside the version prefix. */
export const SKILL_URL_DIR = "skills/ravendb";

/** Throws the fix, not a bare ENOENT, when the dependency is not installed. */
export function assertSkillSource(sourceDir: string): string {
    const skillFile = path.join(sourceDir, "SKILL.md");
    if (!fs.existsSync(skillFile)) {
        throw new Error(`@ravendb/skills is not installed: run npm ci (no ${skillFile})`);
    }
    return skillFile;
}

/** Replaces targetDir with a byte-for-byte copy of sourceDir, so a file removed upstream is not left behind. */
export function copySkillTree(sourceDir: string, targetDir: string): void {
    fs.rmSync(targetDir, { recursive: true, force: true });
    fs.mkdirSync(path.dirname(targetDir), { recursive: true });
    fs.cpSync(sourceDir, targetDir, { recursive: true });
}
