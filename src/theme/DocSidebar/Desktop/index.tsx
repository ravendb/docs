import React from "react";
import clsx from "clsx";
import { useThemeConfig } from "@docusaurus/theme-common";
import Logo from "@theme/Logo";
import CollapseButton from "@theme/DocSidebar/Desktop/CollapseButton";
import Content from "@theme/DocSidebar/Desktop/Content";
import type { Props } from "@theme/DocSidebar/Desktop";

import styles from "./styles.module.css";
import Link from "@docusaurus/Link";
import SidebarVersionDropdown from "@site/src/components/SidebarVersionDropdown";

import { useActiveDocContext, useLatestVersion } from "@docusaurus/plugin-content-docs/client";
import { Icon } from "@site/src/components/Common/Icon";
import { SectionNavLink } from "@site/src/components/Common/SectionNavLink";
import { getPathType, getSectionNavItems, PathType } from "../../../typescript/pathUtils";

function DocSidebarDesktop({ path, sidebar, onCollapse, isHidden }: Props) {
    const {
        navbar: { hideOnScroll },
        docs: {
            sidebar: { hideable },
        },
    } = useThemeConfig();

    const pluginId = "default";
    const { activeVersion } = useActiveDocContext(pluginId);
    const latestVersion = useLatestVersion(pluginId);
    const versionLabel = activeVersion?.label ?? latestVersion.label;

    const pathType = getPathType(path);
    const sectionNavItems = getSectionNavItems(pathType, versionLabel);

    const shouldDisplayContent = pathType !== PathType.Guides && pathType !== PathType.Samples;

    return (
        <div
            className={clsx(
                styles.sidebar,
                hideOnScroll && styles.sidebarWithHideableNavbar,
                isHidden && styles.sidebarHidden
            )}
        >
            {hideOnScroll && <Logo tabIndex={-1} className={styles.sidebarLogo} />}
            <nav aria-label="Documentation sections" className="menu thin-scrollbar menu_Y1UP shrink-0 !grow-0">
                {sectionNavItems.map((item) => (
                    <SectionNavLink key={item.label} item={item} isActive={item.pathType === pathType} />
                ))}
                {pathType === PathType.Documentation && (
                    <Link to={`/${versionLabel}/whats-new`} className="menu__link">
                        <Icon icon="star-filled" size="xs" className="me-2" /> What's new
                    </Link>
                )}
            </nav>
            {shouldDisplayContent && <hr className="!my-0 !mx-3 !bg-black/10 dark:!bg-white/10" />}
            {pathType === PathType.Documentation && <SidebarVersionDropdown />}
            {shouldDisplayContent && <Content path={path} sidebar={sidebar} />}
            {hideable && <CollapseButton onClick={onCollapse} />}
        </div>
    );
}

export default React.memo(DocSidebarDesktop);
