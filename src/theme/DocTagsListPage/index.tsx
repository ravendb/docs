import React, { type ReactNode, useState } from "react";
import clsx from "clsx";
import {
    PageMetadata,
    HtmlClassNameProvider,
    ThemeClassNames,
} from "@docusaurus/theme-common";
import TagsListByLetter from "@theme/TagsListByLetter";
import SearchMetadata from "@theme/SearchMetadata";
import type { Props } from "@theme/DocTagsListPage";
import Heading from "@theme/Heading";
import { Icon } from "@site/src/components/Common/Icon";
import DocSidebar from "@theme/DocSidebar";
import SidebarStyles from "@docusaurus/theme-classic/lib/theme/DocRoot/Layout/Sidebar/styles.module.css";
import DocRootStyles from "@docusaurus/theme-classic/lib/theme/DocRoot/Layout/styles.module.css";
import CustomBreadcrumbs from "@site/src/components/Common/CustomBreadcrumbs";

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
                <main className="container padding-top--md">
                    <div className="row">
                        <main className="col">
                            <CustomBreadcrumbs
                                items={[
                                    { label: "Guides", href: "/guides" },
                                    { label: "Tags" },
                                ]}
                            />
                            <header className="flex flex-col sm:flex-row sm:items-center justify-between gap-8">
                                <div className="flex items-center gap-4">
                                    <Heading as="h1" className="!mb-0">
                                        {title}
                                    </Heading>
                                </div>
                                <div className="relative w-full sm:max-w-[300px]">
                                    <input
                                        type="search"
                                        placeholder="Filter tags..."
                                        value={searchQuery}
                                        onChange={(e) =>
                                            setSearchQuery(e.target.value)
                                        }
                                        className={clsx(
                                            "w-full py-2 px-4 rounded-full text-sm font-normal",
                                            "border border-black/10 dark:border-white/10",
                                            "hover:border-black/20 dark:hover:border-white/20",
                                            "outline-none focus:bg-black/5 dark:focus:bg-white/5 focus:border-black/20 dark:focus:border-white/20 transition-all",
                                        )}
                                    />
                                </div>
                            </header>
                            {filteredTags.length > 0 ? (
                                <TagsListByLetter tags={filteredTags} />
                            ) : (
                                <div className="flex flex-col items-center justify-center py-16 text-center">
                                    <Icon
                                        icon="search"
                                        size="lg"
                                        className="opacity-20 mb-4"
                                    />
                                    <p className="text-lg font-medium text-gray-600 dark:text-gray-400 !mb-1">
                                        No tags found
                                    </p>
                                    <p className="text-sm text-gray-500 dark:text-gray-500">
                                        Try adjusting your search filter
                                    </p>
                                </div>
                            )}
                        </main>
                    </div>
                </main>
            </div>
        </HtmlClassNameProvider>
    );
}

export default function DocTagsListPage(props: Props): ReactNode {
    const title = "Browse by topic";
    return (
        <>
            <DocTagsListPageMetadata {...props} title={title} />
            <DocTagsListPageContent {...props} title={title} />
        </>
    );
}
