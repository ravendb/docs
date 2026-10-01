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

export interface ActiveNavItem extends SectionNavItem {
    isActive: boolean;
}

export interface ProductNavItem extends SectionNavItem {
    pathType: PathTypeValue;
    description: string;
}

export interface SidebarNav {
    links: ActiveNavItem[];
    productDocs: ProductNavItem[];
    products: ProductNavItem[];
    currentProduct: ProductNavItem | null;
    footer: ActiveNavItem[];
}

interface Product {
    type: PathTypeValue;
    label: string;
    icon: IconName;
    description: string;
    currentOnly?: boolean;
}

const PRODUCTS: readonly Product[] = [
    {
        type: PathType.Documentation,
        label: "RavenDB",
        icon: "database",
        description: "Database server, client APIs and Studio",
    },
    { type: PathType.Cloud, label: "RavenDB Cloud", icon: "cloud", description: "Managed RavenDB clusters" },
    {
        type: PathType.Quill,
        label: "Quill",
        icon: "quill",
        description: "AI agents for the database you already have",
    },
    {
        type: PathType.Templates,
        label: "Templates",
        icon: "documentation-guide",
        description: "Building blocks for writing these docs",
        currentOnly: true,
    },
];

const RESOURCES: readonly SectionNavItem[] = [
    { label: "Guides", icon: "guides", to: "/guides", pathType: PathType.Guides },
    { label: "Samples", icon: "create-sample-data", to: "/samples", pathType: PathType.Samples },
];

const COMMUNITY: SectionNavItem = {
    label: "Community",
    icon: "community",
    to: "https://ravendb.net/community",
    pathType: null,
    external: true,
};

function withoutTrailingSlash(path: string): string {
    return path.replace(/\/+$/, "") || "/";
}

function isCurrentPage(path: string, to: string): boolean {
    return withoutTrailingSlash(path) === withoutTrailingSlash(to);
}

export function getSidebarNav(path: string, versionLabel: string): SidebarNav {
    const pathType = getPathType(path);

    const products = PRODUCTS.filter((product) => !product.currentOnly || product.type === pathType).map((product) => ({
        label: product.label,
        icon: product.icon,
        description: product.description,
        to: getLandingPagePath(product.type, versionLabel),
        pathType: product.type,
    }));
    const currentProduct = products.find((product) => product.pathType === pathType) ?? null;
    const productDocs = currentProduct
        ? []
        : products.map((product) => ({ ...product, label: `${product.label} Docs` }));
    const activeOnItsPage = (item: SectionNavItem): ActiveNavItem => ({
        ...item,
        isActive: isCurrentPage(path, item.to),
    });

    const landingPage = getLandingPagePath(pathType, versionLabel);
    const resources = RESOURCES.map((item) => ({ ...item, isActive: item.pathType === pathType }));
    const start: ActiveNavItem = {
        label: "Start",
        icon: "home",
        to: landingPage,
        pathType,
        isActive: isCurrentPage(path, landingPage) && !resources.some((item) => item.isActive),
    };
    const whatsNew =
        pathType === PathType.Documentation
            ? [
                  activeOnItsPage({
                      label: "What's new",
                      icon: "star-filled",
                      to: `/${versionLabel}/whats-new`,
                      pathType,
                  }),
              ]
            : [];

    return {
        links: [start, ...resources],
        productDocs,
        products,
        currentProduct,
        footer: [...whatsNew, { ...COMMUNITY, isActive: false }],
    };
}
