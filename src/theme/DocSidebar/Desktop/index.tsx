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

import {
  useActiveDocContext,
  useLatestVersion,
} from "@docusaurus/plugin-content-docs/client";

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

  const isCloudPath = path.includes("/cloud");

  return (
    <div
      className={clsx(
        styles.sidebar,
        hideOnScroll && styles.sidebarWithHideableNavbar,
        isHidden && styles.sidebarHidden
      )}
    >
      {hideOnScroll && <Logo tabIndex={-1} className={styles.sidebarLogo} />}
      <div className="menu thin-scrollbar menu_Y1UP shrink-0 !grow-0">
        <div className="menu__list-item-collapsible">
          <Link
            to={isCloudPath ? "/cloud" : `/${versionLabel}`}
            className="menu__link"
          >
            Start
          </Link>
        </div>
        <Link
          to={isCloudPath ? `/${versionLabel}` : "/cloud"}
          className="menu__link"
        >
          RavenDB {!isCloudPath && "Cloud"} Docs
        </Link>
        <Link to="https://ravendb.net/community" className="menu__link">
          Community
        </Link>
        {!isCloudPath && (
          <Link to={`/${versionLabel}/whats-new`} className="menu__link">
            What's new
          </Link>
        )}
      </div>
      <hr className="!my-0 !mx-3 !bg-black/10 dark:!bg-white/10" />
      {!isCloudPath && <SidebarVersionDropdown />}
      <Content path={path} sidebar={sidebar} />
      {hideable && <CollapseButton onClick={onCollapse} />}
    </div>
  );
}

export default React.memo(DocSidebarDesktop);
