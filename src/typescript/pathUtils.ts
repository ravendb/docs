import { IconName } from "./iconName";

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

export interface SectionNavItem {
    label: string;
    icon: IconName;
    to: string;
    pathType: PathTypeValue | null;
    // Shown only while you are already inside it, for areas that are not public destinations.
    currentOnly?: boolean;
    external?: boolean;
}

const NAV_SECTIONS: readonly { type: PathTypeValue; label: string; icon: IconName; currentOnly?: boolean }[] = [
    { type: PathType.Documentation, label: "RavenDB Docs", icon: "database" },
    { type: PathType.Cloud, label: "RavenDB Cloud Docs", icon: "cloud" },
    { type: PathType.Quill, label: "Quill Docs", icon: "quill" },
    { type: PathType.Guides, label: "Guides", icon: "guides" },
    { type: PathType.Samples, label: "Samples", icon: "create-sample-data" },
    { type: PathType.Templates, label: "Templates", icon: "documentation-guide", currentOnly: true },
];

export function getSectionNavItems(pathType: PathTypeValue, versionLabel: string): SectionNavItem[] {
    const sections = NAV_SECTIONS.filter((section) => !section.currentOnly || section.type === pathType).map(
        (section) => ({
            label: section.label,
            icon: section.icon,
            to: getLandingPagePath(section.type, versionLabel),
            pathType: section.type,
        })
    );

    return [
        ...sections,
        {
            label: "Community",
            icon: "community",
            to: "https://ravendb.net/community",
            pathType: null,
            external: true,
        },
    ];
}
