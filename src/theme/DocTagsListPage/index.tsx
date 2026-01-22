import React, { type ReactNode, useState } from "react";
import clsx from "clsx";
import {
    PageMetadata,
    HtmlClassNameProvider,
    ThemeClassNames,
    translateTagsPageTitle,
} from "@docusaurus/theme-common";
import TagsListByLetter from "@theme/TagsListByLetter";
import SearchMetadata from "@theme/SearchMetadata";
import type { Props } from "@theme/DocTagsListPage";
import Heading from "@theme/Heading";
import { Icon } from "@site/src/components/Common/Icon";
import Link from "@docusaurus/Link";
import DocSidebar from "@theme/DocSidebar";
import SidebarStyles from "@docusaurus/theme-classic/lib/theme/DocRoot/Layout/Sidebar/styles.module.css";
import DocRootStyles from "@docusaurus/theme-classic/lib/theme/DocRoot/Layout/styles.module.css";

function DocTagsListPageMetadata({
    title,
}: Props & { title: string }): ReactNode {
    return (
        <>
            <PageMetadata title={title} />
            <SearchMetadata tag="doc_tags_list" />
        </>
    );
}

function DocTagsListPageContent({
    tags,
    title,
}: Props & { title: string }): ReactNode {
    const [searchQuery, setSearchQuery] = useState("");

    const filteredTags = tags.filter((tag) =>
        tag.label.toLowerCase().includes(searchQuery.toLowerCase()),
    );

    return (
        <HtmlClassNameProvider
            className={clsx(
                ThemeClassNames.wrapper.docsPages,
                ThemeClassNames.page.docsDocPage,
            )}
        >
            <div className={DocRootStyles.docRoot}>
                <aside
                    className={clsx(
                        ThemeClassNames.docs.docSidebarContainer,
                        SidebarStyles.docSidebarContainer,
                        "h-[calc(100vh-var(--ifm-navbar-height))]",
                    )}
                >
                    <DocSidebar
                        sidebar={[]}
                        path="/guides"
                        onCollapse={() => {}}
                        isHidden={false}
                    />
                </aside>
                <main className="container margin-vert--lg">
                    <div className="row">
                        <main className="col">
                            <header className="flex flex-col sm:flex-row sm:items-center justify-between gap-8">
                                <div className="flex items-center gap-4">
                                    <Link
                                        to={"/guides"}
                                        className="!text-inherit hover:opacity-75 !transition-all"
                                        title="Back to guides"
                                    >
                                        <Icon
                                            icon="arrow-thin-left"
                                            className="w-6 h-6"
                                        />
                                    </Link>
                                    <Heading as="h1" className="!mb-0">
                                        {title}
                                    </Heading>
                                </div>
                                <div className="relative w-full sm:max-w-[300px]">
                                    <input
                                        type="text"
                                        placeholder="Filter tags..."
                                        value={searchQuery}
                                        onChange={(e) =>
                                            setSearchQuery(e.target.value)
                                        }
                                        className={clsx(
                                            "w-full py-2 px-4 pr-10 rounded-full text-sm font-normal",
                                            "border border-black/10 dark:border-white/10",
                                            "hover:border-black/20 dark:hover:border-white/20",
                                            "outline-none focus:bg-black/5 dark:focus:bg-white/5 focus:border-black/20 dark:focus:border-white/20 transition-all",
                                        )}
                                    />
                                    {searchQuery && (
                                        <button
                                            onClick={() => setSearchQuery("")}
                                            className={clsx(
                                                "absolute right-3 top-1/2 -translate-y-1/2",
                                                "p-1 rounded-full",
                                                "hover:bg-black/5 dark:hover:bg-white/5",
                                                "!transition-all cursor-pointer",
                                            )}
                                            aria-label="Clear filter"
                                            title="Clear filter"
                                            type="button"
                                        >
                                            <Icon
                                                icon="close"
                                                size="xs"
                                                className="opacity-50 hover:opacity-100"
                                            />
                                        </button>
                                    )}
                                </div>
                            </header>
                            <TagsListByLetter tags={filteredTags} />
                        </main>
                    </div>
                </main>
            </div>
        </HtmlClassNameProvider>
    );
}

export default function DocTagsListPage(props: Props): ReactNode {
    const title = translateTagsPageTitle();
    return (
        <>
            <DocTagsListPageMetadata {...props} title={title} />
            <DocTagsListPageContent {...props} title={title} />
        </>
    );
}
