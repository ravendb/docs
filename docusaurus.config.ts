import { themes as prismThemes } from "prism-react-renderer";
import type { Config } from "@docusaurus/types";
import type * as Preset from "@docusaurus/preset-classic";

// This runs in Node.js - Don't use client-side code here (browser APIs, JSX...)

function getOnlyIncludeVersions(): string[] | undefined {
    const versionsEnv = process.env.DOCUSAURUS_VERSIONS;

    if (!versionsEnv) {
        return undefined;
    }

    return versionsEnv.split(',').map(v => v.trim());
}

const config: Config = {
  title: "RavenDB Documentation",
  tagline:
    "High-performance NoSQL database that just works.",
  favicon: "img/favicon.ico",

  // Future flags, see https://docusaurus.io/docs/api/docusaurus-config#future
  future: {
    v4: true, // Improve compatibility with the upcoming Docusaurus v4
    experimental_faster: true
  },

  url: "https://docs.ravendb.net/",
  baseUrl: "/",

  onBrokenLinks: "ignore",
  onBrokenMarkdownLinks: "ignore",
  onBrokenAnchors: "ignore",

  trailingSlash: false,

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
          lastVersion: 'current',
          versions: {
            current: {
              label: "7.2",
              path: "7.2"
            }
          },
          onlyIncludeVersions: getOnlyIncludeVersions(),
          //editUrl:
          //    'https://github.com/ravendb/docs/tree/main/'
        },
        blog: false,
        theme: {
          customCss: "./src/css/custom.css",
        },
        sitemap: {
          lastmod: null,
          changefreq: "weekly",
          priority: null
        },
        googleTagManager: {
            containerId: "GTM-TDH4JWF2"
        }
      } satisfies Preset.Options,
    ],
  ],
  plugins: [
    require.resolve("./src/plugins/tailwind-config"),
    [
      'content-docs',
      {
        id: 'cloud',
        path: 'cloud',
        routeBasePath: 'cloud',
        sidebarPath: './sidebarsCloud.js',
      },
    ]
  ],
  themeConfig: {
    docs: {
      sidebar: {
        autoCollapseCategories: true,
      },
    },
    image: "img/social-card.jpg",
    headTags: [
      {
        tagName: "link",
        attributes: {
          rel: "preload",
          href: "css/fonts/Inter[wght].woff2",
          as: "font",
          type: "font/woff2",
          crossorigin: "anonymous",
        },
      },
      {
        tagName: "link",
        attributes: {
          rel: "preload",
          href: "css/fonts/Inter-Italic[wght].woff2",
          as: "font",
          type: "font/woff2",
          crossorigin: "anonymous",
        },
      },
      {
        tagName: "link",
        attributes: {
          rel: "preload",
          href: "css/fonts/JetBrainsMono[wght].woff2",
          as: "font",
          type: "font/woff2",
          crossorigin: "anonymous",
        },
      },
      {
        tagName: "link",
        attributes: {
          rel: "preload",
          href: "css/fonts/JetBrainsMono-Italic[wght].woff2",
          as: "font",
          type: "font/woff2",
          crossorigin: "anonymous",
        },
      },
    ],
    metadata: [
      {
        name: 'keywords',
        content: 'nosql, document database'
      },
      {
        name: 'description',
        content: 'Official RavenDB documentation. Learn installation, querying, indexing, scaling, security, and every advanced feature of the fully ACID NoSQL database that combines performance with ease of use.'
      }
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
            { label: "Contributing", href: "https://ravendb.net/" },
            { label: "About us", href: "https://ravendb.net/about" },
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
      additionalLanguages: ["csharp", "java", "php"],
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
