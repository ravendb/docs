import React, { useCallback, useEffect, useMemo, useRef, useState, type ReactNode } from "react";
import { createPortal } from "react-dom";
import Head from "@docusaurus/Head";
import Link from "@docusaurus/Link";
import { useHistory } from "@docusaurus/router";
import { isRegexpStringMatch, useSearchLinkCreator } from "@docusaurus/theme-common";
import { useAlgoliaContextualFacetFilters, useSearchResultUrlProcessor } from "@docusaurus/theme-search-algolia/client";
import Translate from "@docusaurus/Translate";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import translations from "@theme/SearchTranslations";
import {
    InternalDocSearchHit,
    DocSearchModal as DocSearchModalType,
    DocSearchModalProps,
    StoredDocSearchHit,
    DocSearchTransformClient,
    DocSearchHit,
    useDocSearchKeyboardEvents,
} from "@docsearch/react";

import type { AutocompleteState } from "@algolia/autocomplete-core";
import type { FacetFilters } from "algoliasearch/lite";
import CustomSearchButton from "@site/src/components/Common/CustomSearchButton";
import SearchResultBadge from "@site/src/components/Search/SearchResultBadge";
import SearchFilterPills from "@site/src/components/Search/SearchFilterPills";
import {
    ALL_SCOPE_EXCLUDED_PLUGIN_IDS,
    getSearchResultSource,
    SEARCH_ATTRIBUTES_TO_RETRIEVE,
    SEARCH_FILTERS,
} from "@site/src/components/Search/searchSource";
import type { ContentSource } from "@site/src/components/Common/contentSource";
import { resolveExternalGuideUrl, useExternalGuideUrls } from "@site/src/components/Search/useExternalGuides";

type DocSearchProps = Omit<DocSearchModalProps, "onClose" | "initialScrollY"> & {
    contextualSearch?: string;
    externalUrlRegex?: string;
    searchPagePath: boolean | string;
};

let DocSearchModal: typeof DocSearchModalType | null = null;

function importDocSearchModalIfNeeded() {
    if (DocSearchModal) {
        return Promise.resolve();
    }
    return Promise.all([
        import("@docsearch/react/modal"),
        import("@docsearch/react/style"),
        import("./styles.css"),
    ]).then(([{ DocSearchModal: Modal }]) => {
        DocSearchModal = Modal;
    });
}

function useNavigator(
    { externalUrlRegex }: Pick<DocSearchProps, "externalUrlRegex">,
    externalGuideUrls: Map<string, string>
): DocSearchModalProps["navigator"] {
    const history = useHistory();
    return useMemo(
        () => ({
            navigate(params) {
                const url = resolveExternalGuideUrl(externalGuideUrls, params.itemUrl) ?? params.itemUrl;
                if (/^https?:\/\//.test(url) || isRegexpStringMatch(externalUrlRegex, url)) {
                    // Open external destinations (incl. external guides) in a new tab, matching the Link.
                    if (!window.open(url, "_blank", "noopener,noreferrer")) {
                        window.location.href = url;
                    }
                } else {
                    history.push(url);
                }
            },
        }),
        [externalUrlRegex, externalGuideUrls, history]
    );
}

function useTransformSearchClient(realNbHits: { current: number }): DocSearchModalProps["transformSearchClient"] {
    const {
        siteMetadata: { docusaurusVersion },
    } = useDocusaurusContext();
    return useCallback(
        (searchClient: DocSearchTransformClient) => {
            searchClient.addAlgoliaAgent("docusaurus", docusaurusVersion);
            // DocSearch accumulates nbHits across keystrokes; capture the real per-query count instead.
            const originalSearch = searchClient.search.bind(searchClient);
            searchClient.search = ((...args: Parameters<typeof originalSearch>) =>
                originalSearch(...args).then((response) => {
                    const firstResult = (response as { results?: Array<{ nbHits?: number }> })?.results?.[0];
                    if (firstResult && typeof firstResult.nbHits === "number") {
                        realNbHits.current = firstResult.nbHits;
                    }
                    return response;
                })) as typeof searchClient.search;
            return searchClient;
        },
        [docusaurusVersion, realNbHits]
    );
}

function useTransformItems(props: Pick<DocSearchProps, "transformItems">) {
    const processSearchResultUrl = useSearchResultUrlProcessor();
    const [transformItems] = useState<DocSearchModalProps["transformItems"]>(() => {
        return (items: DocSearchHit[]) =>
            props.transformItems
                ? props.transformItems(items)
                : items.map((item) => ({
                      ...item,
                      url: processSearchResultUrl(item.url),
                  }));
    });
    return transformItems;
}

function useResultsFooterComponent({
    closeModal,
    realNbHits,
    activeFilter,
}: {
    closeModal: () => void;
    realNbHits: { current: number };
    activeFilter: ContentSource | null;
}): DocSearchProps["resultsFooterComponent"] {
    return useMemo(
        () =>
            ({ state }) => (
                <ResultsFooter
                    state={state}
                    onClose={closeModal}
                    nbHits={realNbHits.current}
                    activeFilter={activeFilter}
                />
            ),
        [closeModal, realNbHits, activeFilter]
    );
}

function Hit({ hit, children }: { hit: InternalDocSearchHit | StoredDocSearchHit; children: React.ReactNode }) {
    const source = getSearchResultSource((hit as { docusaurus_tag?: string | string[] }).docusaurus_tag);
    const externalUrl = resolveExternalGuideUrl(useExternalGuideUrls(), hit.url);
    // Crawler-provided section trail; drop the leading source root ("Docs ›" / "Guides") — the
    // source is already shown by the badge and section header.
    const breadcrumb = ((hit as { breadcrumb?: string }).breadcrumb ?? "").split(" › ").slice(1).join(" › ");

    // DocSearch renders no path for page-level (lvl1) hits; inject the section trail into that slot.
    const injectBreadcrumb = (node: HTMLAnchorElement | null) => {
        if (!node || !breadcrumb) {
            return;
        }
        const wrapper = node.querySelector(".DocSearch-Hit-content-wrapper");
        if (!wrapper || wrapper.querySelector(".DocSearch-Hit-path")) {
            return;
        }
        const pathEl = document.createElement("div");
        pathEl.className = "DocSearch-Hit-path";
        pathEl.textContent = breadcrumb;
        wrapper.appendChild(pathEl);
    };

    return (
        <Link to={externalUrl ?? hit.url} ref={injectBreadcrumb}>
            {children}
            {(source || externalUrl) && (
                <span className="DocSearch-Hit-sourceBadge">
                    <SearchResultBadge source={source} />
                    {externalUrl && <SearchResultBadge source="external" />}
                </span>
            )}
        </Link>
    );
}

type ResultsFooterProps = {
    state: AutocompleteState<InternalDocSearchHit>;
    onClose: () => void;
    nbHits: number;
    activeFilter: ContentSource | null;
};

function ResultsFooter({ state, onClose, nbHits, activeFilter }: ResultsFooterProps) {
    const createSearchLink = useSearchLinkCreator();
    let to = createSearchLink(state.query);
    if (activeFilter) {
        // Carry the active source filter through to the full search page.
        to += `${to.includes("?") ? "&" : "?"}source=${activeFilter}`;
    }

    return (
        <Link to={to} onClick={onClose}>
            <Translate id="theme.SearchBar.seeAll" values={{ count: nbHits }}>
                {"See all {count} results"}
            </Translate>
        </Link>
    );
}

function useSearchParameters(
    { contextualSearch, ...props }: DocSearchProps,
    activePluginId: string | null
): DocSearchProps["searchParameters"] {
    const contextual = useAlgoliaContextualFacetFilters() as (string | string[])[];
    const configFilters = (props.searchParameters?.facetFilters ?? []) as (string | string[])[];

    // Contextual filters look like ["language:en", ["docusaurus_tag:..", ...]]. Narrow the tag
    // group: one pill -> just that instance's tag, keeping the version already in the contextual
    // group (so the Docs pill stays on the page's version); "All" -> drop separated scopes (samples).
    const isExcludedFromAll = (tag: string) =>
        ALL_SCOPE_EXCLUDED_PLUGIN_IDS.some((pluginId) => tag.startsWith(`docusaurus_tag:docs-${pluginId}-`));
    const narrowed = contextual.map((entry) => {
        if (!Array.isArray(entry)) {
            return entry;
        }
        return activePluginId
            ? entry.filter((tag) => tag.startsWith(`docusaurus_tag:docs-${activePluginId}-`))
            : entry.filter((tag) => !isExcludedFromAll(tag));
    });

    let facetFilters: (string | string[])[];
    if (contextualSearch) {
        facetFilters = [...narrowed, ...configFilters];
    } else if (activePluginId) {
        facetFilters = [[`docusaurus_tag:docs-${activePluginId}-current`], ...configFilters];
    } else {
        facetFilters = configFilters;
    }

    return {
        ...props.searchParameters,
        attributesToRetrieve: props.searchParameters?.attributesToRetrieve ?? SEARCH_ATTRIBUTES_TO_RETRIEVE,
        facetFilters: facetFilters as FacetFilters,
    };
}

// Text fields where typing must not hijack the type-to-search shortcut.
function isEditableElement(target: EventTarget | null): boolean {
    if (!(target instanceof HTMLElement)) {
        return false;
    }
    return (
        target.tagName === "INPUT" ||
        target.tagName === "TEXTAREA" ||
        target.tagName === "SELECT" ||
        target.isContentEditable
    );
}

function DocSearch({ externalUrlRegex, ...props }: DocSearchProps) {
    const externalGuideUrls = useExternalGuideUrls();
    const navigator = useNavigator({ externalUrlRegex }, externalGuideUrls);
    const [activeFilter, setActiveFilter] = useState<ContentSource | null>(null);
    const activePluginId = SEARCH_FILTERS.find((f) => f.kind === activeFilter)?.pluginId ?? null;
    const searchParameters = useSearchParameters({ ...props }, activePluginId);
    const transformItems = useTransformItems(props);
    const realNbHits = useRef(0);
    const transformSearchClient = useTransformSearchClient(realNbHits);

    const searchContainer = useRef<HTMLDivElement | null>(null);
    const searchButtonRef = useRef<HTMLButtonElement>(null);
    const [isOpen, setIsOpen] = useState(false);
    const [initialQuery, setInitialQuery] = useState<string | undefined>(undefined);
    const [pillHost, setPillHost] = useState<HTMLElement | null>(null);

    const prepareSearchContainer = useCallback(() => {
        if (!searchContainer.current) {
            const divElement = document.createElement("div");
            searchContainer.current = divElement;
            document.body.insertBefore(divElement, document.body.firstChild);
        }
    }, []);

    const openModal = useCallback(() => {
        prepareSearchContainer();
        importDocSearchModalIfNeeded().then(() => setIsOpen(true));
    }, [prepareSearchContainer]);

    const closeModal = useCallback(() => {
        setIsOpen(false);
        searchButtonRef.current?.focus();
        setInitialQuery(undefined);
        setActiveFilter(null);
    }, []);

    // The DocSearch modal has no slot for custom controls, so insert a host node after its search bar
    // and portal the pills into it. Visibility (hidden until the user types) is pure CSS, so typing
    // never touches React state and the first character is never dropped.
    useEffect(() => {
        if (!isOpen) {
            setPillHost(null);
            return undefined;
        }
        const searchBar = document.querySelector(".DocSearch-Modal .DocSearch-SearchBar");
        if (!searchBar) {
            return undefined;
        }
        const host = document.createElement("div");
        host.className = "DocSearch-SourceFilters";
        searchBar.insertAdjacentElement("afterend", host);
        setPillHost(host);
        return () => host.remove();
    }, [isOpen, activeFilter]);

    // DocSearch reads searchParameters only at mount, so a filter change remounts the modal
    // (keyed on the filter); the typed query is fed back as the next initial query.
    const handleFilterChange = useCallback((kind: ContentSource | null) => {
        const input = document.querySelector<HTMLInputElement>(".DocSearch-Input");
        setInitialQuery(input?.value || undefined);
        setPillHost(null);
        setActiveFilter(kind);
    }, []);

    // Type-to-search: a printable key pressed anywhere outside a text field opens the modal
    // seeded with that character (Space and "/" keep their usual behavior). Seeding the query
    // is also what guarantees the first keystroke survives the modal's lazy mount.
    useEffect(() => {
        if (isOpen) {
            return undefined;
        }
        const onKeyDown = (event: KeyboardEvent) => {
            if (
                event.key.length !== 1 ||
                event.key === " " ||
                event.key === "/" ||
                event.metaKey ||
                event.ctrlKey ||
                event.altKey ||
                event.isComposing ||
                isEditableElement(event.composedPath()[0] ?? event.target)
            ) {
                return;
            }
            event.preventDefault();
            setInitialQuery((prev) => (prev ?? "") + event.key);
            openModal();
        };
        window.addEventListener("keydown", onKeyDown);
        return () => window.removeEventListener("keydown", onKeyDown);
    }, [isOpen, openModal]);

    const resultsFooterComponent = useResultsFooterComponent({ closeModal, realNbHits, activeFilter });

    useDocSearchKeyboardEvents({
        isOpen,
        onOpen: openModal,
        onClose: closeModal,
        isAskAiActive: false,
        onAskAiToggle: (_toggle: boolean) => {},
    });

    return (
        <>
            <Head>
                {/* Preconnect to the Algolia cluster to speed up the first query. */}
                <link rel="preconnect" href={`https://${props.appId}-dsn.algolia.net`} crossOrigin="anonymous" />
            </Head>

            <CustomSearchButton
                ref={searchButtonRef}
                onTouchStart={importDocSearchModalIfNeeded}
                onFocus={importDocSearchModalIfNeeded}
                onMouseOver={importDocSearchModalIfNeeded}
                onClick={openModal}
                translations={props.translations?.button}
            />

            {isOpen &&
                DocSearchModal &&
                searchContainer.current &&
                createPortal(
                    <DocSearchModal
                        key={activeFilter ?? "all"}
                        onClose={closeModal}
                        initialScrollY={window.scrollY}
                        initialQuery={initialQuery}
                        navigator={navigator}
                        transformItems={transformItems}
                        hitComponent={Hit}
                        transformSearchClient={transformSearchClient}
                        {...(props.searchPagePath && {
                            resultsFooterComponent,
                        })}
                        placeholder={translations.placeholder}
                        {...props}
                        translations={props.translations?.modal ?? translations.modal}
                        searchParameters={searchParameters}
                    />,
                    searchContainer.current
                )}

            {isOpen &&
                pillHost &&
                createPortal(<SearchFilterPills active={activeFilter} onChange={handleFilterChange} />, pillHost)}
        </>
    );
}

export default function SearchBar(): ReactNode {
    const { siteConfig } = useDocusaurusContext();
    return <DocSearch {...(siteConfig.themeConfig.algolia as DocSearchProps)} />;
}
