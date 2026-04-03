import { themes as prismThemes } from "prism-react-renderer";
import type { Config } from "@docusaurus/types";
import type * as Preset from "@docusaurus/preset-classic";

// This runs in Node.js - Don't use client-side code here (browser APIs, JSX...)

function getOnlyIncludeVersions(): string[] | undefined {
    const versionsEnv = process.env.DOCUSAURUS_VERSIONS;

    if (!versionsEnv) {
        return undefined;
    }

    return versionsEnv.split(",").map((v) => v.trim());
}

const isStrict = process.env.DOCUSAURUS_STRICT === "true";

const config: Config = {
    title: "RavenDB Documentation",
    tagline: "High-performance NoSQL database that just works.",
    favicon: "img/favicon.ico",

    // Future flags, see https://docusaurus.io/docs/api/docusaurus-config#future
    future: {
        v4: true, // Improve compatibility with the upcoming Docusaurus v4
        experimental_faster: true,
    },

    customFields: {
        latestVersion: "7.2",
    },

    url: "https://docs.ravendb.net/",
    baseUrl: "/",

    onBrokenLinks: isStrict ? "throw" : "warn",
    onBrokenMarkdownLinks: isStrict ? "throw" : "warn",
    onBrokenAnchors: "ignore",

    i18n: {
        defaultLocale: "en",
        locales: ["en"],
    },

    presets: [
        [
            "classic",
            {
                docs: {
                    sidebarPath: "sidebars.ts",
                    routeBasePath: "/",
                    includeCurrentVersion: true,
                    lastVersion: "current",
                    versions: {
                        current: {
                            label: "7.2",
                            path: "7.2",
                        },
                    },
                    onlyIncludeVersions: getOnlyIncludeVersions(),
                    editUrl: "https://github.com/ravendb/docs/edit/main/",
                },
                blog: false,
                theme: {
                    customCss: "./src/css/custom.css",
                },
                sitemap: {
                    lastmod: "date",
                    changefreq: null,
                    priority: null,
                    ignorePatterns: [
                        "/1.0/**",
                        "/2.0/**",
                        "/2.5/**",
                        "/3.0/**",
                        "/3.5/**",
                        "/4.0/**",
                        "/4.1/**",
                        "/4.2/**",
                        "/5.0/**",
                        "/5.1/**",
                        "/5.2/**",
                        "/5.3/**",
                        "/5.4/**",
                    ],
                },
                googleTagManager: {
                    containerId: "GTM-TDH4JWF2",
                },
            } satisfies Preset.Options,
        ],
    ],
    plugins: [
        require.resolve("./src/plugins/tailwind-config"),
        [
            "content-docs",
            {
                id: "cloud",
                path: "cloud",
                routeBasePath: "cloud",
                sidebarPath: require.resolve("./sidebarsCloud.js"),
                editUrl: "https://github.com/ravendb/docs/edit/main",
            },
        ],
        [
            "content-docs",
            {
                id: "guides",
                path: "guides",
                routeBasePath: "guides",
                sidebarPath: require.resolve("./sidebarsGuides.js"),
                showLastUpdateTime: true,
            },
        ],
        [
            "content-docs",
            {
                id: "templates",
                path: "templates",
                routeBasePath: "templates",
                sidebarPath: require.resolve("./sidebarsTemplates.js"),
                showLastUpdateTime: true,
            },
        ],
        [
            "content-docs",
            {
                id: "samples",
                path: "samples",
                routeBasePath: "samples",
                sidebarPath: require.resolve("./sidebarsSamples.js"),
            },
        ],
        [
            "@docusaurus/plugin-ideal-image",
            {
                max: 1200,
                min: 640,
                steps: 3,
                quality: 85,
                disableInDev: false,
            },
        ],
        require.resolve("./src/plugins/recent-guides-plugin"),
        require.resolve("./src/plugins/recent-samples-plugin"),
    ],
    headTags: [
        {
            tagName: "link",
            attributes: {
                rel: "preload",
                href: "/css/fonts/Inter[wght].woff2",
                as: "font",
                type: "font/woff2",
                crossorigin: "anonymous",
            },
        },
        {
            tagName: "link",
            attributes: {
                rel: "preload",
                href: "/css/fonts/JetBrainsMono[wght].woff2",
                as: "font",
                type: "font/woff2",
                crossorigin: "anonymous",
            },
        },
        {
            tagName: "script",
            attributes: { type: "application/ld+json" },
            innerHTML: JSON.stringify({
                "@context": "https://schema.org",
                "@type": "WebSite",
                name: "RavenDB Documentation",
                url: "https://docs.ravendb.net/",
                description:
                    "Official RavenDB documentation. Learn installation, querying, indexing, scaling, security, and every advanced feature of the fully ACID NoSQL database that combines performance with ease of use.",
                publisher: {
                    "@type": "Organization",
                    name: "RavenDB",
                    url: "https://ravendb.net/",
                    logo: {
                        "@type": "ImageObject",
                        url: "https://docs.ravendb.net/img/social-card.jpg",
                    },
                },
                potentialAction: {
                    "@type": "SearchAction",
                    target: {
                        "@type": "EntryPoint",
                        urlTemplate: "https://docs.ravendb.net/search?q={search_term_string}",
                    },
                    "query-input": "required name=search_term_string",
                },
            }),
        },
        {
            tagName: "script",
            attributes: { type: "application/ld+json" },
            innerHTML: JSON.stringify({
                "@context": "https://schema.org",
                "@type": "Organization",
                name: "RavenDB",
                url: "https://ravendb.net/",
                logo: {
                    "@type": "ImageObject",
                    url: "https://docs.ravendb.net/img/social-card.jpg",
                },
                sameAs: [
                    "https://github.com/ravendb/ravendb",
                    "https://www.youtube.com/@ravendb_net",
                    "https://en.wikipedia.org/wiki/RavenDB",
                    "https://www.linkedin.com/company/ravendb",
                    "https://stackoverflow.com/questions/tagged/ravendb",
                ],
            }),
        },
        {
            tagName: "script",
            attributes: { type: "application/ld+json" },
            innerHTML: JSON.stringify({
                "@context": "https://schema.org",
                "@type": "SoftwareApplication",
                name: "RavenDB",
                applicationCategory: "DeveloperTools",
                operatingSystem: "Windows, Linux, macOS, Docker",
                description:
                    "A fully transactional NoSQL document database with ACID transactions, distributed clusters, and multi-model data support.",
                url: "https://ravendb.net/",
                softwareVersion: "7.2",
                author: {
                    "@type": "Organization",
                    name: "RavenDB",
                    url: "https://ravendb.net/",
                },
                offers: {
                    "@type": "Offer",
                    price: "0",
                    priceCurrency: "USD",
                    url: "https://ravendb.net/download",
                },
            }),
        },
    ],
    themeConfig: {
        docs: {
            sidebar: {
                autoCollapseCategories: true,
            },
        },
        tableOfContents: {
            minHeadingLevel: 2,
            maxHeadingLevel: 4,
        },
        image: "img/social-card.jpg",
        metadata: [
            {
                name: "description",
                content:
                    "Official RavenDB documentation. Learn installation, querying, indexing, scaling, security, and every advanced feature of the fully ACID NoSQL database that combines performance with ease of use.",
            },
            {
                property: "og:image:width",
                content: "1200",
            },
            {
                property: "og:image:height",
                content: "630",
            },
            {
                property: "og:image:alt",
                content: "RavenDB Documentation",
            },
        ],
        colorMode: {
            defaultMode: "dark",
            disableSwitch: false,
            respectPrefersColorScheme: true,
        },
        footer: {
            links: [
                {
                    items: [
                        {
                            context: "Need some help?",
                            label: "Contact support",
                            href: "https://ravendb.net/support",
                            icon: "support",
                        },
                        {
                            context: "Discuss your ideas with our",
                            label: "community",
                            href: "https://ravendb.net/community",
                            icon: "community",
                        },
                        {
                            context: "Check the latest",
                            label: "product updates",
                            href: "https://ravendb.net/whats-new",
                            icon: "star-filled",
                        },
                        {
                            context: "Something’s not working?",
                            label: "Check system status",
                            href: "https://status.ravendb.net",
                            icon: "server",
                        },
                    ],
                },
                {
                    items: [
                        {
                            label: "About us",
                            href: "https://ravendb.net/about",
                        },
                        { label: "Legal", href: "https://ravendb.net/legal" },
                    ],
                },
                {
                    items: [
                        {
                            href: "https://github.com/ravendb/ravendb",
                            icon: "github",
                            label: "GitHub",
                        },
                        {
                            href: "https://www.youtube.com/@ravendb_net",
                            icon: "youtube",
                            label: "YouTube",
                        },
                        {
                            href: "https://ravendb.net/l/3VRWT2",
                            icon: "discord",
                            label: "Discord",
                        },
                    ],
                },
            ],
            copyright: `Copyright © ${new Date().getFullYear()} RavenDB`,
        },
        prism: {
            theme: prismThemes.nightOwlLight,
            darkTheme: prismThemes.dracula,
            additionalLanguages: ["csharp", "java", "php", "bash"],
        },
        algolia: {
            // The application ID provided by Algolia
            appId: "GYTCYX561T",

            // Public API key: it is safe to commit it
            apiKey: "a97a73adcd8ac27ce741f4f663e9f554",

            indexName: "ProductionDocsCrawler",

            contextualSearch: true,

            // Optional: Specify domains where the navigation should occur through window.location instead on history.push. Useful when our Algolia config crawls multiple documentation sites and we want to navigate with window.location.href to them.
            externalUrlRegex: "external\\.com|domain\\.com",

            // Optional: Replace parts of the item URLs from Algolia. Useful when using the same search index for multiple deployments using a different baseUrl. You can use regexp or string in the `from` param. For example: localhost:3000 vs myCompany.com/docs
            replaceSearchResultPathname: {
                from: "/docs/", // or as RegExp: /\/docs\//
                to: "/",
            },

            // Optional: path for search page that enabled by default (`false` to disable it)
            searchPagePath: "search",

            insights: false,
        },
    } satisfies Preset.ThemeConfig,
};

export default config;
