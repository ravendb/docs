export const PathType = {
    Cloud: "CLOUD",
    Quill: "QUILL",
    Guides: "GUIDES",
    Documentation: "DOCUMENTATION",
    Templates: "TEMPLATES",
    Samples: "SAMPLES",
} as const;

export type PathTypeValue = (typeof PathType)[keyof typeof PathType];

// Versionless content areas, in match order. The URL segment doubles as the landing page path.
// Anything that matches none of these is versioned documentation.
const SECTIONS: readonly { segment: string; type: PathTypeValue }[] = [
    { segment: "cloud", type: PathType.Cloud },
    { segment: "quill", type: PathType.Quill },
    { segment: "guides", type: PathType.Guides },
    { segment: "samples", type: PathType.Samples },
    { segment: "templates", type: PathType.Templates },
];

export function getPathType(path: string): PathTypeValue {
    const [firstSegment] = path.replace(/^\/+/, "").split("/");
    return SECTIONS.find((section) => section.segment === firstSegment)?.type ?? PathType.Documentation;
}

export function getLandingPagePath(pathType: PathTypeValue, versionLabel: string): string {
    const section = SECTIONS.find((candidate) => candidate.type === pathType);
    return section ? `/${section.segment}` : `/${versionLabel}`;
}
