import React, { memo, type ReactNode } from "react";
import clsx from "clsx";
import Link from "@docusaurus/Link";
import { translate } from "@docusaurus/Translate";
import { useVisibleBlogSidebarItems, BlogSidebarItemList } from "@docusaurus/plugin-content-blog/client";
import BlogSidebarContent from "@theme/BlogSidebar/Content";
import type { Props } from "@theme/BlogSidebar/Desktop";

import styles from "./styles.module.css";

const WHATS_NEW_PATH = "/cloud/whats-new";

function ListComponent({ items }: { items: Props["sidebar"]["items"] }): ReactNode {
    return (
        <BlogSidebarItemList
            items={items}
            ulClassName={clsx(styles.sidebarItemList, "clean-list")}
            liClassName={styles.sidebarItem}
            linkClassName={styles.sidebarItemLink}
            linkActiveClassName={styles.sidebarItemLinkActive}
        />
    );
}

function BlogSidebarDesktop({ sidebar }: Props): ReactNode {
    const items = useVisibleBlogSidebarItems(sidebar.items);
    return (
        <aside className="col col--3">
            <nav
                className={clsx(styles.sidebar, "thin-scrollbar")}
                aria-label={translate({
                    id: "theme.blog.sidebar.navAriaLabel",
                    message: "Blog recent posts navigation",
                    description: "The ARIA label for recent posts in the blog sidebar",
                })}
            >
                <Link to={WHATS_NEW_PATH} className={clsx(styles.sidebarItemTitle, "margin-bottom--md")}>
                    {sidebar.title}
                </Link>
                <BlogSidebarContent
                    items={items}
                    ListComponent={ListComponent}
                    yearGroupHeadingClassName={styles.yearGroupHeading}
                />
            </nav>
        </aside>
    );
}

export default memo(BlogSidebarDesktop);
