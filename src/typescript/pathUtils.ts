export enum PathType {
    Cloud = "CLOUD",
    Guides = "GUIDES",
    Documentation = "DOCUMENTATION",
    Templates = "TEMPLATES"
}

export function getPathType(path: string): PathType {
    if (path.includes("/cloud")) {
        return PathType.Cloud;
    }
    if (path.includes("/guides")) {
        return PathType.Guides;
    }
    if (path.includes("/templates")) {
        return PathType.Templates;
    }
    return PathType.Documentation;
}

export function getLandingPagePath(pathType: PathType, versionLabel: string): string {
    if (pathType === PathType.Cloud) {
        return "/cloud";
    }
    if (pathType === PathType.Guides) {
        return "/guides";
    }
    if (pathType === PathType.Templates) {
        return "/templates";
    }
    return `/${versionLabel}`;
}
