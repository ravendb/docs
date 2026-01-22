import React, { type ReactNode, useState, useMemo } from "react";
import clsx from "clsx";
import Link from "@docusaurus/Link";
import {
    PageMetadata,
    HtmlClassNameProvider,
    ThemeClassNames,
} from "@docusaurus/theme-common";
import SearchMetadata from "@theme/SearchMetadata";
import Heading from "@theme/Heading";
import { Icon } from "@site/src/components/Common/Icon";
import CardWithImage from "@site/src/components/Common/CardWithImage";
import GuideListItem from "@site/src/components/Common/GuideListItem";
import LayoutSwitcher, {
    type LayoutMode,
} from "@site/src/components/Common/LayoutSwitcher";
import { usePluginData } from "@docusaurus/useGlobalData";
import type { PluginData } from "@site/src/plugins/recent-guides-plugin";
import DocSidebar from "@theme/DocSidebar";
import SidebarStyles from "@docusaurus/theme-classic/lib/theme/DocRoot/Layout/Sidebar/styles.module.css";
import DocRootStyles from "@docusaurus/theme-classic/lib/theme/DocRoot/Layout/styles.module.css";
import Layout from "@theme/Layout";

function AllGuidesPageMetadata(): ReactNode {
    return (
        <>
            <PageMetadata
                title="All Guides"
                description="Browse all available guides"
            />
            <SearchMetadata tag="all_guides" />
        </>
    );
}

function AllGuidesPageContent(): ReactNode {
    const guidesData = usePluginData("recent-guides-plugin") as PluginData;
    const [layoutMode, setLayoutMode] = useState<LayoutMode>("grid");

    const guides = guidesData?.guides || [];

    const sortedGuides = useMemo(() => {
        return [...guides]
            .filter((guide) => guide.id !== "home")
            .sort((a, b) => b.lastUpdatedAt - a.lastUpdatedAt);
    }, [guides]);

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
                            <div className="flex flex-col gap-8">
                                <header className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
                                    <div className="flex items-center gap-4">
                                        <Link
                                            to="/guides"
                                            className="!text-inherit hover:opacity-75 !transition-all"
                                            title="Back to guides"
                                        >
                                            <Icon
                                                icon="arrow-thin-left"
                                                className="w-6 h-6"
                                            />
                                        </Link>
                                        <Heading as="h1" className="!mb-0">
                                            All Guides
                                        </Heading>
                                        <span
                                            className={clsx(
                                                "flex items-center justify-center rounded-full font-bold leading-none",
                                                "w-8 h-8 text-[12px]",
                                                "bg-black/10",
                                                "dark:bg-white/20 dark:text-white",
                                            )}
                                        >
                                            {sortedGuides.length}
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
                                            {sortedGuides.map((guide, index) => {
                                                const formattedDate =
                                                    guide.lastUpdatedAt
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
                                                        key={guide.id}
                                                        title={guide.title}
                                                        description={
                                                            guide.description
                                                        }
                                                        url={guide.permalink}
                                                        imgSrc={guide.image}
                                                        imgIcon={guide.icon}
                                                        tags={guide.tags}
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
                                            {sortedGuides.map((guide, index) => {
                                                const formattedDate =
                                                    guide.lastUpdatedAt
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
                                                        key={guide.id}
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
                                                            title={guide.title}
                                                            url={guide.permalink}
                                                            tags={guide.tags}
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

export default function AllGuidesPage(): ReactNode {
    return (
        <Layout>
            <AllGuidesPageMetadata />
            <AllGuidesPageContent />
        </Layout>
    );
}

