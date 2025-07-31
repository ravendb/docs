import { themes as prismThemes } from "prism-react-renderer";
import type { Config } from "@docusaurus/types";
import type * as Preset from "@docusaurus/preset-classic";

// This runs in Node.js - Don't use client-side code here (browser APIs, JSX...)

const config: Config = {
  title: "RavenDB Documentation",
  tagline:
    "Everything you need to know about our product, from getting started to advanced features.",
  favicon: "img/favicon.ico",

  // Future flags, see https://docusaurus.io/docs/api/docusaurus-config#future
  future: {
    v4: true, // Improve compatibility with the upcoming Docusaurus v4
  },

  // Set the production url of your site here
  url: "https://test.docs.ravendb.net/",
  // Set the /<baseUrl>/ pathname under which your site is served
  // For GitHub pages deployment, it is often '/<projectName>/'
  baseUrl: "/",

  // GitHub pages deployment config.
  // If you aren't using GitHub pages, you don't need these.
  organizationName: "facebook", // Usually your GitHub org/user name.
  projectName: "docusaurus", // Usually your repo name.

  onBrokenLinks: "ignore",
  onBrokenMarkdownLinks: "ignore",
  onBrokenAnchors: "ignore",

  // Even if you don't use internationalization, you can use this field to set
  // useful metadata like html lang. For example, if your site is Chinese, you
  // may want to replace "en" with "zh-Hans".
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
              label: "7.1",
              path: "/7.1"
            }
          }
          // Please change this to your repo.
          // Remove this to remove the "edit this page" links.
          // editUrl:
          //  'https://github.com/facebook/docusaurus/tree/main/packages/create-docusaurus/templates/shared/',
        },
        blog: false,
        theme: {
          customCss: "./src/css/custom.css",
        },
        sitemap: {
          lastmod: null,
          changefreq: "weekly",
          priority: 0.5,
          ignorePatterns: ["/tags/**"],
          filename: "sitemap.xml",
          createSitemapItems: async (params) => {
            const { defaultCreateSitemapItems, ...rest } = params;
            const items = await defaultCreateSitemapItems(rest);
            return items.filter((item) => !item.url.includes("/page/"));
          },
        },
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
              icon: "updates",
            },
            {
              context: "Something’s not working?",
              label: "Check system status",
              href: "https://status.ravendb.net",
              icon: "status",
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
      theme: prismThemes.github,
      darkTheme: prismThemes.dracula,
      additionalLanguages: ["csharp", "java", "php"],
    },
    algolia: {
      // The application ID provided by Algolia
      appId: "GYTCYX561T",

      // Public API key: it is safe to commit it
      apiKey: "a97a73adcd8ac27ce741f4f663e9f554",

      indexName: "TestDocsCrawler",

      // Optional: see doc section below
      contextualSearch: true,

      // Optional: Specify domains where the navigation should occur through window.location instead on history.push. Useful when our Algolia config crawls multiple documentation sites and we want to navigate with window.location.href to them.
      externalUrlRegex: "external\\.com|domain\\.com",

      // Optional: Replace parts of the item URLs from Algolia. Useful when using the same search index for multiple deployments using a different baseUrl. You can use regexp or string in the `from` param. For example: localhost:3000 vs myCompany.com/docs
      replaceSearchResultPathname: {
        from: "/docs/", // or as RegExp: /\/docs\//
        to: "/",
      },

      // Optional: Algolia search parameters
      searchParameters: {},

      // Optional: path for search page that enabled by default (`false` to disable it)
      searchPagePath: "search",

      // Optional: whether the insights feature is enabled or not on Docsearch (`false` by default)
      insights: false,

      //... other Algolia params
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
