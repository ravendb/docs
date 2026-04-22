/**
 * Post-build script: generates build/robots.txt from a template plus the
 * legacy-version list in scripts/lib/version-policy.js.
 *
 * Template variant is picked via the RAVENDB_DOCS_TEST_BUILD env var (same
 * convention previously used by scripts/deploy.ps1's Prepare-RobotsTxt):
 *   - truthy → robots_test.template.txt  (blanket Disallow, no placeholders)
 *   - falsy  → robots_prod.template.txt  (per-crawler policy)
 *
 * Placeholders substituted in prod template:
 *   {{DISALLOW_LEGACY_VERSIONS}} → Disallow: /X.Y/  (one line per legacy version)
 */

const fs = require("fs");
const path = require("path");

const { LEGACY_VERSIONS } = require("./lib/version-policy.js");

const TRUTHY_ENV_VALUES = new Set(["1", "true", "yes", "y", "on"]);

function isTestBuild() {
    const raw = process.env.RAVENDB_DOCS_TEST_BUILD;
    if (!raw) return false;
    return TRUTHY_ENV_VALUES.has(raw.trim().toLowerCase());
}

function buildDisallowBlock(versions) {
    // Stable order: numeric ascending (1.0 → 5.4) to keep diffs minimal.
    const sorted = [...versions].sort((a, b) => {
        const [aMaj, aMin] = a.split(".").map(Number);
        const [bMaj, bMin] = b.split(".").map(Number);
        return aMaj - bMaj || aMin - bMin;
    });
    return sorted.map((v) => `Disallow: /${v}/`).join("\n");
}

const buildDir = path.join(__dirname, "..", "build");

if (!fs.existsSync(buildDir)) {
    console.error(`generate-robots: build directory not found at ${buildDir}`);
    process.exit(1);
}

const testBuild = isTestBuild();
const templateName = testBuild ? "robots_test.template.txt" : "robots_prod.template.txt";
const templatePath = path.join(__dirname, "robots-templates", templateName);

if (!fs.existsSync(templatePath)) {
    console.error(`generate-robots: template not found at ${templatePath}`);
    process.exit(1);
}

const template = fs.readFileSync(templatePath, "utf8");
// The test template is a blanket Disallow with no placeholders; skip substitution
// entirely so a future stray "{{…}}" token in it would be surfaced verbatim.
const rendered = testBuild
    ? template
    : template.replace(/\{\{DISALLOW_LEGACY_VERSIONS\}\}/g, buildDisallowBlock(LEGACY_VERSIONS));

const outPath = path.join(buildDir, "robots.txt");
fs.writeFileSync(outPath, rendered, "utf8");

console.log(
    `generate-robots: wrote ${outPath} (${testBuild ? "test" : "prod"} variant, ${LEGACY_VERSIONS.length} legacy versions)`
);
