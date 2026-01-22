import React, { type ReactNode, useState } from "react";
import clsx from "clsx";
import Link from "@docusaurus/Link";
import {
    PageMetadata,
    HtmlClassNameProvider,
    ThemeClassNames,
} from "@docusaurus/theme-common";
import Translate, { translate } from "@docusaurus/Translate";
import SearchMetadata from "@theme/SearchMetadata";
import type { Props } from "@theme/DocTagDocListPage";
import Unlisted from "@theme/ContentVisibility/Unlisted";
import Heading from "@theme/Heading";
import { Icon } from "@site/src/components/Common/Icon";
import CardWithImage from "@site/src/components/Common/CardWithImage";
import GuideListItem from "@site/src/components/Common/GuideListItem";
import LayoutSwitcher, {
    type LayoutMode,
} from "@site/src/components/Common/LayoutSwitcher";
import { usePluginData } from "@docusaurus/useGlobalData";
import type { PluginData, Guide } from "@site/src/plugins/recent-guides-plugin";
import DocSidebar from "@theme/DocSidebar";
import SidebarStyles from "@docusaurus/theme-classic/lib/theme/DocRoot/Layout/Sidebar/styles.module.css";
import DocRootStyles from "@docusaurus/theme-classic/lib/theme/DocRoot/Layout/styles.module.css";

function DocTagDocListPageMetadata({
    title,
    tag,
}: Props & { title: string }): ReactNode {
    return (
        <>
            <PageMetadata title={title} description={tag.description} />
            <SearchMetadata tag="doc_tag_doc_list" />
        </>
    );
}

function DocTagDocListPageContent({ tag }: Props): ReactNode {
    const guidesData = usePluginData("recent-guides-plugin") as PluginData;
    const [layoutMode, setLayoutMode] = useState<LayoutMode>("grid");

    const guides = guidesData?.guides || [];

    const sortedItems = React.useMemo(() => {
        return [...tag.items].sort((a, b) => {
            const guideA = guides.find(
                (g: Guide) => g.permalink === a.permalink,
            );
            const guideB = guides.find(
                (g: Guide) => g.permalink === b.permalink,
            );

            const dateA = guideA?.lastUpdatedAt || 0;
            const dateB = guideB?.lastUpdatedAt || 0;

            return dateB - dateA; // Latest first
        });
    }, [tag.items, guides]);

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
                            {tag.unlisted && <Unlisted />}
                            <div className="flex flex-col gap-8">
                                <header className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
                                    <div className="flex items-center gap-4">
                                        <Link
                                            to={tag.allTagsPath}
                                            className="!text-inherit hover:opacity-75 !transition-all"
                                            title="Back to all tags"
                                        >
                                            <Icon
                                                icon="arrow-thin-left"
                                                className="w-6 h-6"
                                            />
                                        </Link>
                                        <Heading as="h1" className="!mb-0">
                                            {tag.label}
                                        </Heading>
                                        <span
                                            className={clsx(
                                                "flex items-center justify-center rounded-full font-bold leading-none",
                                                "w-8 h-8 text-[12px]",
                                                "bg-black/10",
                                                "dark:bg-white/20 dark:text-white",
                                            )}
                                        >
                                            {tag.count}
                                        </span>
                                    </div>
                                    <LayoutSwitcher
                                        layoutMode={layoutMode}
                                        onLayoutChange={setLayoutMode}
                                    />
                                </header>
                                <div className="relative">
                                    {layoutMode === "grid" ? (
                                        <div
                                            className={clsx(
                                                "grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4",
                                                "animate-in fade-in",
                                            )}
                                        >
                                            {sortedItems.map((doc, index) => {
                                                const guide = guides.find(
                                                    (g: Guide) =>
                                                        g.permalink ===
                                                        doc.permalink,
                                                );
                                                const formattedDate =
                                                    guide?.lastUpdatedAt
                                                        ? new Date(
                                                              guide.lastUpdatedAt *
                                                                  1000,
                                                          ).toLocaleDateString(
                                                              "en-US",
                                                              {
                                                                  month: "short",
                                                                  day: "numeric",
                                                                  year: "numeric",
                                                              },
                                                          )
                                                        : undefined;
                                                return (
                                                    <CardWithImage
                                                        key={doc.id}
                                                        title={doc.title}
                                                        description={
                                                            doc.description
                                                        }
                                                        url={doc.permalink}
                                                        imgSrc={guide?.image}
                                                        imgIcon={guide?.icon}
                                                        tags={guide?.tags}
                                                        date={formattedDate}
                                                        animationDelay={
                                                            index * 50
                                                        }
                                                    />
                                                );
                                            })}
                                        </div>
                                    ) : (
                                        <div
                                            className={clsx(
                                                "flex flex-col",
                                                "animate-in fade-in",
                                            )}
                                        >
                                            {sortedItems.map((doc, index) => {
                                                const guide = guides.find(
                                                    (g: Guide) =>
                                                        g.permalink ===
                                                        doc.permalink,
                                                );
                                                const formattedDate =
                                                    guide?.lastUpdatedAt
                                                        ? new Date(
                                                              guide.lastUpdatedAt *
                                                                  1000,
                                                          ).toLocaleDateString(
                                                              "en-US",
                                                              {
                                                                  month: "short",
                                                                  day: "numeric",
                                                                  year: "numeric",
                                                              },
                                                          )
                                                        : undefined;
                                                return (
                                                    <div
                                                        key={doc.id}
                                                        className="animate-in fade-in slide-in-from-left-4"
                                                        style={{
                                                            animationDelay: `${index * 30}ms`,
                                                            animationDuration:
                                                                "300ms",
                                                            animationFillMode:
                                                                "backwards",
                                                        }}
                                                    >
                                                        <GuideListItem
                                                            title={doc.title}
                                                            url={doc.permalink}
                                                            tags={guide?.tags}
                                                            date={formattedDate}
                                                        />
                                                    </div>
                                                );
                                            })}
                                        </div>
                                    )}
                                </div>
                            </div>
                        </main>
                    </div>
                </main>
            </div>
        </HtmlClassNameProvider>
    );
}

export default function DocTagDocListPage(props: Props): ReactNode {
    const title = translate(
        {
            id: "theme.docs.tagDocListPageTitle",
            description: 'Guides tagged with "{tagName}"',
            message: 'Guides tagged with "{tagName}"',
        },
        { tagName: props.tag.label },
    );

    return (
        <>
            <DocTagDocListPageMetadata {...props} title={title} />
            <DocTagDocListPageContent {...props} />
        </>
    );
}
