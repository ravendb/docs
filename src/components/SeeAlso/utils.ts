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
