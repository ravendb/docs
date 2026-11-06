import { test } from "node:test";
import assert from "node:assert/strict";
import fs from "node:fs";
import os from "node:os";
import path from "node:path";

import { assertSkillSource, copySkillTree } from "../lib/skill-tree.js";

function tempDir(): string {
    return fs.mkdtempSync(path.join(os.tmpdir(), "skill-tree-"));
}

test("copySkillTree copies the tree byte for byte, line endings included", () => {
    const root = tempDir();
    const source = path.join(root, "source");
    fs.mkdirSync(path.join(source, "references", "dotnet"), { recursive: true });
    fs.writeFileSync(path.join(source, "SKILL.md"), "---\r\ndescription: x\n---\n");
    fs.writeFileSync(path.join(source, "references", "dotnet", "client.md"), "é\n");

    const target = path.join(root, "out", "skills", "ravendb");
    copySkillTree(source, target);

    assert.deepEqual(fs.readFileSync(path.join(target, "SKILL.md")), fs.readFileSync(path.join(source, "SKILL.md")));
    assert.equal(fs.readFileSync(path.join(target, "references", "dotnet", "client.md"), "utf8"), "é\n");
});

test("copySkillTree drops files that no longer exist upstream", () => {
    const root = tempDir();
    const source = path.join(root, "source");
    fs.mkdirSync(source);
    fs.writeFileSync(path.join(source, "SKILL.md"), "x");
    const target = path.join(root, "out");
    fs.mkdirSync(target);
    fs.writeFileSync(path.join(target, "removed.md"), "stale");

    copySkillTree(source, target);

    assert.deepEqual(fs.readdirSync(target), ["SKILL.md"]);
});

test("assertSkillSource names the fix when the dependency is not installed", () => {
    assert.throws(() => assertSkillSource(tempDir()), /@ravendb\/skills is not installed: run npm ci/);
});
