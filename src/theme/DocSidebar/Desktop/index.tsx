import React from "react";
import clsx from "clsx";
import {useThemeConfig} from "@docusaurus/theme-common";
import Logo from "@theme/Logo";
import CollapseButton from "@theme/DocSidebar/Desktop/CollapseButton";
import Content from "@theme/DocSidebar/Desktop/Content";
import type {Props} from "@theme/DocSidebar/Desktop";

import styles from "./styles.module.css";
import Link from "@docusaurus/Link";
import SidebarVersionDropdown from "@site/src/components/SidebarVersionDropdown";

import {
    useActiveDocContext,
    useLatestVersion,
} from "@docusaurus/plugin-content-docs/client";
import {Icon} from "@site/src/components/Common/Icon";

function DocSidebarDesktop({path, sidebar, onCollapse, isHidden}: Props) {
    const {
        navbar: {hideOnScroll},
        docs: {
            sidebar: {hideable},
        },
    } = useThemeConfig();

    enum PathType {
        Cloud = "CLOUD",
        Guides = "GUIDES",
        Documentation = "DOCUMENTATION",
    }

    const pluginId = "default";
    const {activeVersion} = useActiveDocContext(pluginId);
    const latestVersion = useLatestVersion(pluginId);
    const versionLabel = activeVersion?.label ?? latestVersion.label;

    const pathType: PathType = (() => {
        if (path.includes("/cloud")) {
            return PathType.Cloud;
        }
        if (path.includes("/guides")) {
            return PathType.Guides;
        }
        return PathType.Documentation;
    })();

    const landingPagePath: string = (() => {
        if (pathType == PathType.Cloud) {
            return "/cloud";
        }
        if (pathType == PathType.Guides) {
            return "/guides";
        }
        return `/${versionLabel}`;
    })();

    return (
        <div
            className={clsx(
                styles.sidebar,
                hideOnScroll && styles.sidebarWithHideableNavbar,
                isHidden && styles.sidebarHidden
            )}
        >
            {hideOnScroll && (
                <Logo tabIndex={-1} className={styles.sidebarLogo} />
            )}
            <div className="menu thin-scrollbar menu_Y1UP shrink-0 !grow-0">
                <div className="menu__list-item-collapsible">
                    <Link to={landingPagePath} className="menu__link">
                        <Icon icon="home" size="xs" className="me-2" /> Start
                    </Link>
                </div>
                {pathType != PathType.Documentation && (
                    <Link to={`/${versionLabel}`} className="menu__link group">
                        <Icon icon="database" size="xs" className="me-2" />{" "}
                        RavenDB Docs
                        <small className="flex items-center ms-auto gap-1 text-[0.675rem] opacity-0 group-hover:opacity-100 !transition-all">
                            Switch <Icon icon="arrow-thin-right" size="xs" />
                        </small>
                    </Link>
                )}
                {pathType != PathType.Cloud && (
                    <Link to="/cloud" className="menu__link group">
                        <Icon icon="cloud" size="xs" className="me-2" /> RavenDB
                        Cloud Docs
                        <small className="flex items-center ms-auto gap-1 text-[0.675rem] opacity-0 group-hover:opacity-100 !transition-all">
                            Switch <Icon icon="arrow-thin-right" size="xs" />
                        </small>
                    </Link>
                )}
                {pathType != PathType.Guides && (
                    <Link to="/guides" className="menu__link group">
                        <Icon icon="guides" size="xs" className="me-2" />{" "}
                        RavenDB Guides
                        <small className="flex items-center ms-auto gap-1 text-[0.675rem] opacity-0 group-hover:opacity-100 !transition-all">
                            Switch <Icon icon="arrow-thin-right" size="xs" />
                        </small>
                    </Link>
                )}
                <Link
                    to="https://ravendb.net/community"
                    className="menu__link group"
                >
                    <Icon icon="community" size="xs" className="me-2" />{" "}
                    Community
                    <Icon
                        icon="newtab"
                        size="xs"
                        className="ms-auto opacity-0 group-hover:opacity-100 !transition-all"
                    />
                </Link>
                {pathType == PathType.Documentation && (
                    <Link
                        to={`/${versionLabel}/whats-new`}
                        className="menu__link"
                    >
                        <Icon icon="star-filled" size="xs" className="me-2" />{" "}
                        What's new
                    </Link>
                )}
            </div>
            {pathType != PathType.Guides && (
                <hr className="!my-0 !mx-3 !bg-black/10 dark:!bg-white/10" />
            )}
            {pathType == PathType.Documentation && <SidebarVersionDropdown />}
            {pathType != PathType.Guides && (
                <Content path={path} sidebar={sidebar} />
            )}
            {hideable && <CollapseButton onClick={onCollapse} />}
        </div>
    );
}

export default React.memo(DocSidebarDesktop);
