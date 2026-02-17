import { IconName } from "@site/src/typescript/iconName";
import { SeeAlsoItemType } from "./types";

export const getIconName = (source: SeeAlsoItemType["source"]): IconName => {
    switch (source) {
        case "docs":
            return "document2";
        case "cloud":
            return "cloud";
        case "guides":
            return "guides";
        case "external":
            return "newtab";
        default:
            return "document";
    }
};

export const parsePath = (path: string) => {
    const parts = path ? path.split(">") : [];
    const mainCategory = parts[0]?.trim() || path;
    const restOfPath =
        parts.length > 1
            ? parts
                  .slice(1)
                  .map((p) => p.trim())
                  .join(" > ")
            : "";
    return { mainCategory, restOfPath, fullPath: path };
};
