import fs from "fs";
import path from "path";

/** Only this subtree is published; the package root also holds README, LICENSE and package.json. */
export const SKILL_SOURCE_DIR = path.join("node_modules", "@ravendb", "skills", "skills", "ravendb");

export const GENERATED_STATIC_DIR = path.join(".generated", "static");

export const SKILL_URL_DIR = "skills/ravendb";

export function assertSkillSource(sourceDir: string): string {
    const skillFile = path.join(sourceDir, "SKILL.md");
    if (!fs.existsSync(skillFile)) {
        throw new Error(`@ravendb/skills is not installed: run npm ci (no ${skillFile})`);
    }
    return skillFile;
}

export function copySkillTree(sourceDir: string, targetDir: string): void {
    fs.rmSync(targetDir, { recursive: true, force: true });
    fs.mkdirSync(path.dirname(targetDir), { recursive: true });
    fs.cpSync(sourceDir, targetDir, { recursive: true });
}
