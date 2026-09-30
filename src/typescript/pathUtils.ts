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
interface Section {
    segment: string;
    type: PathTypeValue;
}

const SECTIONS: readonly Section[] = [
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
    external?: boolean;
}

interface NavSection {
    type: PathTypeValue;
    label: string;
    icon: IconName;
}

const NAV_SECTIONS: readonly NavSection[] = [
    { type: PathType.Documentation, label: "RavenDB Docs", icon: "database" },
    { type: PathType.Cloud, label: "RavenDB Cloud Docs", icon: "cloud" },
    { type: PathType.Quill, label: "Quill Docs", icon: "quill" },
    { type: PathType.Guides, label: "Guides", icon: "guides" },
    { type: PathType.Samples, label: "Samples", icon: "create-sample-data" },
];

const COMMUNITY: SectionNavItem = {
    label: "Community",
    icon: "community",
    to: "https://ravendb.net/community",
    pathType: null,
    external: true,
};

export function getSectionNavItems(pathType: PathTypeValue, versionLabel: string): SectionNavItem[] {
    // The section you are in keeps its place on the list, named for what the entry does rather
    // than repeating the product name already shown in the header. The sidebars mark it active.
    const start: SectionNavItem = {
        label: "Start",
        icon: "home",
        to: getLandingPagePath(pathType, versionLabel),
        pathType,
    };

    const others = NAV_SECTIONS.filter((section) => section.type !== pathType).map((section) => ({
        label: section.label,
        icon: section.icon,
        to: getLandingPagePath(section.type, versionLabel),
        pathType: section.type,
    }));

    return [start, ...others, COMMUNITY];
}
