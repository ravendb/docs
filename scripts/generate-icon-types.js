const fs = require("fs");
const path = require("path");

const ICONS_DIR = path.join(__dirname, "../static/icons");
const TYPES_FILE = path.join(__dirname, "../src/typescript/iconName.ts");

function generateIconTypes() {
  try {
    const files = fs.readdirSync(ICONS_DIR);
    const svgFiles = files.filter((file) => file.endsWith(".svg"));

    const iconNamesArray = svgFiles.map((file) => {
      const name = path.basename(file, ".svg");
      return `  '${name}'`;
    });

    const typesContent = `
export const ALL_ICON_NAMES = [
${iconNamesArray.join(",\n")}
] as const;

export type IconName = typeof ALL_ICON_NAMES[number];
`;

    fs.writeFileSync(TYPES_FILE, typesContent);
    console.log(`Generated icon types for ${svgFiles.length} icons`);
  } catch (error) {
    console.error("Error generating icon types:", error);
    process.exit(1);
  }
}

generateIconTypes();
