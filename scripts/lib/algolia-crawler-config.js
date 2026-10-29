// Algolia Crawler config, pushed by scripts/sync-algolia.ps1; the crawler's apiKey stays in Algolia.

const indexSettings = require("./algolia-index-settings.json");

const INDEX_NAME = "ProductionDocsCrawler";

const recordExtractor = ({ url, $, helpers }) => {
    const LANDING_PAGES = ["", "/7.2", "/7.1", "/6.2", "/cloud", "/guides", "/samples", "/quill"];
    const SECTION_LABELS = {
        guides: "Guides",
        cloud: "Cloud",
        samples: "Samples",
        quill: "Quill",
    };

    const path = url.pathname.endsWith("/") ? url.pathname.slice(0, -1) : url.pathname;
    if (LANDING_PAGES.includes(path)) {
        return [];
    }

    const lvl0 =
        SECTION_LABELS[path.split("/")[1]] ||
        $(".menu__link.menu__link--sublist.menu__link--active, .navbar__item.navbar__link--active").last().text() ||
        "Documentation";

    // The page's own breadcrumb, minus the home icon and the current page
    const items = $(".theme-doc-breadcrumbs .breadcrumbs__item").not(".breadcrumbs__item--active");
    const crumbs = [];
    for (let i = 0; i < items.length; i++) {
        const text = items.eq(i).text().trim();
        if (text) {
            crumbs.push(text);
        }
    }
    const breadcrumb = crumbs.join(" › ");

    const records = helpers.docsearch({
        recordProps: {
            lvl0: { selectors: "", defaultValue: lvl0 },
            lvl1: ["header h1", "article h1"],
            lvl2: "article h2",
            lvl3: "article h3",
            lvl4: "article h4",
            lvl5: "article h5, article td:first-child",
            lvl6: "article h6",
            content: "article p, article li, article td:last-child",
        },
        aggregateContent: true,
        recordVersion: "v3",
    });

    return records.map((record) => ({ ...record, breadcrumb }));
};

const CRAWLER_CONFIG = {
    appId: "GYTCYX561T",
    indexPrefix: "",
    rateLimit: 8,
    maxDepth: 10,
    maxUrls: null,
    schedule: "on the 8 day of the month",
    startUrls: ["https://docs.ravendb.net/7.2"],
    renderJavaScript: false,
    sitemaps: ["https://docs.ravendb.net/sitemap.xml"],
    // Older versions canonicalize to the current one; without this they would not be indexed.
    ignoreCanonicalTo: true,
    discoveryPatterns: ["https://docs.ravendb.net/**"],
    ignoreRobotsTxtRules: true,
    // Language switcher ?lang= links serve the same HTML; index each page once.
    ignoreQueryParams: ["lang"],
    actions: [
        {
            indexName: INDEX_NAME,
            pathsToMatch: ["https://docs.ravendb.net/**"],
            recordExtractor,
        },
    ],
    safetyChecks: {
        beforeIndexPublishing: {
            maxLostRecordsPercentage: 50,
        },
    },
    // Applied only when Algolia creates the index; nulls are defaults, not values.
    initialIndexSettings: {
        [INDEX_NAME]: Object.fromEntries(Object.entries(indexSettings).filter(([, value]) => value !== null)),
    },
};

module.exports = { CRAWLER_CONFIG };

if (require.main === module) {
    const toApi = (key, value) =>
        typeof value === "function" ? { __type: "function", source: value.toString() } : value;
    console.log(JSON.stringify(CRAWLER_CONFIG, toApi, 4));
}
