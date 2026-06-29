/**
 * Swizzled from @docusaurus/theme-search-algolia, kept close to upstream for diffing.
 * Adds per-hit source badges and source filter pills (see SEARCH_FILTERS).
 */
import React, { useEffect, useReducer, useRef, useState, type ReactNode } from "react";
import { useHistory, useLocation } from "@docusaurus/router";
import clsx from "clsx";
import algoliaSearchHelper from "algoliasearch-helper";
import { liteClient } from "algoliasearch/lite";
import ExecutionEnvironment from "@docusaurus/ExecutionEnvironment";
import Head from "@docusaurus/Head";
import Link from "@docusaurus/Link";
import { useAllDocsData } from "@docusaurus/plugin-content-docs/client";
import {
    HtmlClassNameProvider,
    PageMetadata,
    useEvent,
    usePluralForm,
    useSearchQueryString,
} from "@docusaurus/theme-common";
import Translate, { translate } from "@docusaurus/Translate";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import { useAlgoliaThemeConfig, useSearchResultUrlProcessor } from "@docusaurus/theme-search-algolia/client";
import Layout from "@theme/Layout";
import Heading from "@theme/Heading";
import SearchResultBadge from "@site/src/components/Search/SearchResultBadge";
import SearchFilterPills from "@site/src/components/Search/SearchFilterPills";
import {
    ALL_SCOPE_EXCLUDED_TAGS,
    getSearchResultSource,
    SEARCH_ATTRIBUTES_TO_RETRIEVE,
    SEARCH_FILTERS,
} from "@site/src/components/Search/searchSource";
import type { ContentSource } from "@site/src/components/Common/contentSource";
import { resolveExternalGuideUrl, useExternalGuideUrls } from "@site/src/components/Search/useExternalGuides";
import styles from "./styles.module.css";

interface ResultItem {
    title: string;
    url: string;
    summary: string;
    breadcrumbs: string[];
    source: ContentSource | null;
    external: boolean;
}

interface SearchResultState {
    items: ResultItem[];
    query: string | null;
    totalResults: number | null;
    totalPages: number | null;
    lastPage: number | null;
    hasMore: boolean | null;
    loading: boolean | null;
}

type SearchResultAction =
    | { type: "reset" }
    | { type: "loading" }
    | { type: "update"; value: SearchResultState }
    | { type: "advance" };

// Very simple pluralization: probably good enough for now
function useDocumentsFoundPlural() {
    const { selectMessage } = usePluralForm();
    return (count: number) =>
        selectMessage(
            count,
            translate(
                {
                    id: "theme.SearchPage.documentsFound.plurals",
                    description:
                        'Pluralized label for "{count} documents found". Use as much plural forms (separated by "|") as your language support (see https://www.unicode.org/cldr/cldr-aux/charts/34/supplemental/language_plural_rules.html)',
                    message: "One document found|{count} documents found",
                },
                { count }
            )
        );
}

function useDocsSearchVersionsHelpers() {
    const allDocsData = useAllDocsData();
    // State of the version select menus / algolia facet filters
    // docsPluginId -> versionName map
    const [searchVersions, setSearchVersions] = useState<Record<string, string>>(() =>
        Object.entries(allDocsData).reduce(
            (acc, [pluginId, pluginData]) => ({ ...acc, [pluginId]: pluginData.versions[0].name }),
            {}
        )
    );
    // Set the value of a single select menu
    const setSearchVersion = (pluginId: string, searchVersion: string) =>
        setSearchVersions((s) => ({ ...s, [pluginId]: searchVersion }));
    const versioningEnabled = Object.values(allDocsData).some((docsData) => docsData.versions.length > 1);
    return { allDocsData, versioningEnabled, searchVersions, setSearchVersion };
}

type DocsSearchVersionsHelpers = ReturnType<typeof useDocsSearchVersionsHelpers>;

// We want to display one select per versioned docs plugin instance
function SearchVersionSelectList({
    docsSearchVersionsHelpers,
}: {
    docsSearchVersionsHelpers: DocsSearchVersionsHelpers;
}) {
    const versionedPluginEntries = Object.entries(docsSearchVersionsHelpers.allDocsData)
        // Do not show a version select for unversioned docs plugin instances
        .filter(([, docsData]) => docsData.versions.length > 1);
    return (
        <div className={clsx("col", "col--3", "padding-left--none", styles.searchVersionColumn)}>
            {versionedPluginEntries.map(([pluginId, docsData]) => {
                const labelPrefix = versionedPluginEntries.length > 1 ? `${pluginId}: ` : "";
                return (
                    <select
                        key={pluginId}
                        onChange={(e) => docsSearchVersionsHelpers.setSearchVersion(pluginId, e.target.value)}
                        defaultValue={docsSearchVersionsHelpers.searchVersions[pluginId]}
                        className={styles.searchVersionInput}
                    >
                        {docsData.versions.map((version, i) => (
                            <option key={i} label={`${labelPrefix}${version.label}`} value={version.name} />
                        ))}
                    </select>
                );
            })}
        </div>
    );
}

function getSearchPageTitle(searchQuery: string): string {
    return searchQuery
        ? translate(
              {
                  id: "theme.SearchPage.existingResultsTitle",
                  message: 'Search results for "{query}"',
                  description: "The search page title for non-empty query",
              },
              { query: searchQuery }
          )
        : translate({
              id: "theme.SearchPage.emptyResultsTitle",
              message: "Search the documentation",
              description: "The search page title for empty query",
          });
}

function SearchPageContent(): ReactNode {
    const {
        i18n: { currentLocale },
    } = useDocusaurusContext();
    const {
        algolia: { appId, apiKey, indexName, contextualSearch },
    } = useAlgoliaThemeConfig();
    const processSearchResultUrl = useSearchResultUrlProcessor();
    const documentsFoundPlural = useDocumentsFoundPlural();
    const docsSearchVersionsHelpers = useDocsSearchVersionsHelpers();
    const [searchQuery, setSearchQuery] = useSearchQueryString();
    const history = useHistory();
    const location = useLocation();
    const [activeFilter, setActiveFilter] = useState<ContentSource | null>(null);
    // The URL's ?source= is the source of truth for the active pill. Synced after mount (so the
    // first client render matches the query-less prerendered HTML) and on every URL change (so the
    // modal's "See all results" link and back/forward stay in sync).
    useEffect(() => {
        const source = new URLSearchParams(location.search).get("source");
        setActiveFilter(SEARCH_FILTERS.find((f) => f.kind === source)?.kind ?? null);
    }, [location.search]);
    const handleFilterChange = (kind: ContentSource | null) => {
        const params = new URLSearchParams(location.search);
        if (kind) {
            params.set("source", kind);
        } else {
            params.delete("source");
        }
        history.replace({ search: params.toString() });
    };
    const pageTitle = getSearchPageTitle(searchQuery);
    const initialSearchResultState: SearchResultState = {
        items: [],
        query: null,
        totalResults: null,
        totalPages: null,
        lastPage: null,
        hasMore: null,
        loading: null,
    };
    const [searchResultState, searchResultStateDispatcher] = useReducer(
        (prevState: SearchResultState, data: SearchResultAction): SearchResultState => {
            switch (data.type) {
                case "reset": {
                    return initialSearchResultState;
                }
                case "loading": {
                    return { ...prevState, loading: true };
                }
                case "update": {
                    if (searchQuery !== data.value.query) {
                        return prevState;
                    }
                    return {
                        ...data.value,
                        items: data.value.lastPage === 0 ? data.value.items : prevState.items.concat(data.value.items),
                    };
                }
                case "advance": {
                    const hasMore = (prevState.totalPages ?? 0) > (prevState.lastPage ?? 0) + 1;
                    return {
                        ...prevState,
                        lastPage: hasMore ? (prevState.lastPage ?? 0) + 1 : prevState.lastPage,
                        hasMore,
                    };
                }
                default:
                    return prevState;
            }
        },
        initialSearchResultState
    );

    const externalGuideUrls = useExternalGuideUrls();

    // respect settings from the theme config for facets
    const disjunctiveFacets = contextualSearch ? ["language", "docusaurus_tag"] : [];
    const algoliaClient = liteClient(appId, apiKey);
    const algoliaHelper = algoliaSearchHelper(algoliaClient as any, indexName, {
        hitsPerPage: 15,
        advancedSyntax: true,
        disjunctiveFacets,
        attributesToRetrieve: SEARCH_ATTRIBUTES_TO_RETRIEVE,
    } as any);

    algoliaHelper.on("result", ({ results }: any) => {
        const { query, hits, page, nbHits, nbPages } = results;
        if (query === "" || !Array.isArray(hits)) {
            searchResultStateDispatcher({ type: "reset" });
            return;
        }
        const sanitizeValue = (value: string) =>
            value.replace(/algolia-docsearch-suggestion--highlight/g, "search-result-match");
        const items: ResultItem[] = hits.map((hit: any) => {
            const {
                url,
                _highlightResult: { hierarchy },
                _snippetResult: snippet = {},
            } = hit;
            const source = getSearchResultSource(hit.docusaurus_tag);
            const externalUrl = resolveExternalGuideUrl(externalGuideUrls, url);
            const titles = Object.keys(hierarchy).map((key) => sanitizeValue(hierarchy[key].value));
            return {
                title: titles.pop() ?? "",
                url: externalUrl ?? processSearchResultUrl(url),
                summary: snippet.content ? `${sanitizeValue(snippet.content.value)}...` : "",
                breadcrumbs: titles,
                source,
                external: !!externalUrl,
            };
        });
        searchResultStateDispatcher({
            type: "update",
            value: {
                items,
                query,
                totalResults: nbHits,
                totalPages: nbPages,
                lastPage: page,
                hasMore: nbPages > page + 1,
                loading: false,
            },
        });
    });

    const [loaderRef, setLoaderRef] = useState<HTMLDivElement | null>(null);
    const prevY = useRef(0);
    const observer = useRef(
        ExecutionEnvironment.canUseIntersectionObserver &&
            new IntersectionObserver(
                (entries) => {
                    const {
                        isIntersecting,
                        boundingClientRect: { y: currentY },
                    } = entries[0];
                    if (isIntersecting && prevY.current > currentY) {
                        searchResultStateDispatcher({ type: "advance" });
                    }
                    prevY.current = currentY;
                },
                { threshold: 1 }
            )
    );

    const makeSearch = useEvent((page = 0) => {
        if (contextualSearch) {
            algoliaHelper.addDisjunctiveFacetRefinement("language", currentLocale);
            const activeFilterDef = SEARCH_FILTERS.find((f) => f.kind === activeFilter);
            if (activeFilterDef?.pluginId) {
                // One source selected: restrict to that instance's tag at its selected version.
                const version = docsSearchVersionsHelpers.searchVersions[activeFilterDef.pluginId] ?? "current";
                algoliaHelper.addDisjunctiveFacetRefinement(
                    "docusaurus_tag",
                    `docs-${activeFilterDef.pluginId}-${version}`
                );
            } else {
                // "All": every instance except separated scopes (samples).
                algoliaHelper.addDisjunctiveFacetRefinement("docusaurus_tag", "default");
                Object.entries(docsSearchVersionsHelpers.searchVersions).forEach(([pluginId, searchVersion]) => {
                    const tag = `docs-${pluginId}-${searchVersion}`;
                    if (!ALL_SCOPE_EXCLUDED_TAGS.includes(tag)) {
                        algoliaHelper.addDisjunctiveFacetRefinement("docusaurus_tag", tag);
                    }
                });
            }
        }
        algoliaHelper.setQuery(searchQuery).setPage(page).search();
    });

    useEffect(() => {
        if (!loaderRef) {
            return undefined;
        }
        const currentObserver = observer.current;
        if (currentObserver) {
            currentObserver.observe(loaderRef);
            return () => currentObserver.unobserve(loaderRef);
        }
        return () => true;
    }, [loaderRef]);

    useEffect(() => {
        searchResultStateDispatcher({ type: "reset" });
        if (searchQuery) {
            searchResultStateDispatcher({ type: "loading" });
            setTimeout(() => {
                makeSearch();
            }, 300);
        }
    }, [searchQuery, activeFilter, docsSearchVersionsHelpers.searchVersions, makeSearch]);

    useEffect(() => {
        if (!searchResultState.lastPage || searchResultState.lastPage === 0) {
            return;
        }
        makeSearch(searchResultState.lastPage);
    }, [makeSearch, searchResultState.lastPage]);

    return (
        <Layout>
            <PageMetadata title={pageTitle} />

            <Head>
                {/*
                 We should not index search pages
                  See https://github.com/facebook/docusaurus/pull/3233
                */}
                <meta property="robots" content="noindex, follow" />
            </Head>

            <div className="container margin-vert--lg">
                <Heading as="h1">{pageTitle}</Heading>

                <form className="row" onSubmit={(e) => e.preventDefault()}>
                    <div
                        className={clsx("col", styles.searchQueryColumn, {
                            "col--9": docsSearchVersionsHelpers.versioningEnabled,
                            "col--12": !docsSearchVersionsHelpers.versioningEnabled,
                        })}
                    >
                        <input
                            type="search"
                            name="q"
                            className={styles.searchQueryInput}
                            placeholder={translate({
                                id: "theme.SearchPage.inputPlaceholder",
                                message: "Type your search here",
                                description: "The placeholder for search page input",
                            })}
                            aria-label={translate({
                                id: "theme.SearchPage.inputLabel",
                                message: "Search",
                                description: "The ARIA label for search page input",
                            })}
                            onChange={(e) => setSearchQuery(e.target.value)}
                            value={searchQuery}
                            autoComplete="off"
                            autoFocus
                        />
                    </div>

                    {contextualSearch && docsSearchVersionsHelpers.versioningEnabled && (
                        <SearchVersionSelectList docsSearchVersionsHelpers={docsSearchVersionsHelpers} />
                    )}
                </form>

                <SearchFilterPills active={activeFilter} onChange={handleFilterChange} className="margin-bottom--md" />

                <div className="row">
                    <div className={clsx("col", "col--12", styles.searchResultsColumn)}>
                        {!!searchResultState.totalResults && documentsFoundPlural(searchResultState.totalResults)}
                    </div>
                </div>

                {searchResultState.items.length > 0 ? (
                    <main>
                        {searchResultState.items.map((item, i) => (
                            <article key={i} className={styles.searchResultItem}>
                                <Heading
                                    as="h2"
                                    className={clsx(
                                        styles.searchResultItemHeading,
                                        "flex flex-wrap items-center gap-x-2 gap-y-1"
                                    )}
                                >
                                    <Link to={item.url} dangerouslySetInnerHTML={{ __html: item.title }} />
                                    <SearchResultBadge source={item.source} />
                                    {item.external && <SearchResultBadge source="external" />}
                                </Heading>

                                {item.breadcrumbs.length > 0 && (
                                    <nav aria-label="breadcrumbs">
                                        <ul className={clsx("breadcrumbs", styles.searchResultItemPath)}>
                                            {item.breadcrumbs.map((html, index) => (
                                                <li
                                                    key={index}
                                                    className="breadcrumbs__item"
                                                    // Markup comes from Algolia highlighting and is trusted.
                                                    dangerouslySetInnerHTML={{ __html: html }}
                                                />
                                            ))}
                                        </ul>
                                    </nav>
                                )}

                                {item.summary && (
                                    <p
                                        className={styles.searchResultItemSummary}
                                        // Markup comes from Algolia highlighting and is trusted.
                                        dangerouslySetInnerHTML={{ __html: item.summary }}
                                    />
                                )}
                            </article>
                        ))}
                    </main>
                ) : (
                    [
                        searchQuery && !searchResultState.loading && (
                            <p key="no-results">
                                <Translate
                                    id="theme.SearchPage.noResultsText"
                                    description="The paragraph for empty search result"
                                >
                                    No results were found
                                </Translate>
                            </p>
                        ),
                        !!searchResultState.loading && <div key="spinner" className={styles.loadingSpinner} />,
                    ]
                )}

                {searchResultState.hasMore && (
                    <div className={styles.loader} ref={setLoaderRef}>
                        <Translate
                            id="theme.SearchPage.fetchingNewResults"
                            description="The paragraph for fetching new search results"
                        >
                            Fetching new results...
                        </Translate>
                    </div>
                )}
            </div>
        </Layout>
    );
}

export default function SearchPage(): ReactNode {
    return (
        <HtmlClassNameProvider className="search-page-wrapper">
            <SearchPageContent />
        </HtmlClassNameProvider>
    );
}
