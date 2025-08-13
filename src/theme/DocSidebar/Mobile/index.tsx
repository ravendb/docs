import React from "react";
import clsx from "clsx";
import {
  NavbarSecondaryMenuFiller,
  ThemeClassNames,
} from "@docusaurus/theme-common";
import { useNavbarMobileSidebar } from "@docusaurus/theme-common/internal";
import DocSidebarItems from "@theme/DocSidebarItems";
import type { Props } from "@theme/DocSidebar/Mobile";
import Link from "@docusaurus/Link";
import {
  useActiveDocContext,
  useLatestVersion,
} from "@docusaurus/plugin-content-docs/client";
import { Icon } from "@site/src/components/Common/Icon";
import type { Props as DocSidebarProps } from "@theme/DocSidebar";
import SidebarVersionDropdown from "@site/src/components/SidebarVersionDropdown";

function DocSidebarMobileSecondaryMenu({ sidebar, path }: DocSidebarProps) {
  const mobileSidebar = useNavbarMobileSidebar();

  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);
  const latestVersion = useLatestVersion(pluginId);
  const versionLabel = activeVersion?.label ?? latestVersion.label;

  const isCloudPath = path.includes("/cloud");

  return (
    <ul className={clsx(ThemeClassNames.docs.docSidebarMenu, "menu__list")}>
      <li className="menu__list-item">
        <div className="menu__list-item-collapsible">
          <Link
            to={isCloudPath ? "/cloud" : `/${versionLabel}`}
            className="menu__link"
            onClick={() => mobileSidebar.toggle()}
          >
            <Icon icon="home" size="xs" className="me-2" /> Start
          </Link>
        </div>
      </li>
      <li className="menu__list-item">
        <Link
          to={isCloudPath ? `/${versionLabel}` : "/cloud"}
          className="menu__link group"
          onClick={() => mobileSidebar.toggle()}
        >
          <Icon
            icon={isCloudPath ? "database" : "cloud"}
            size="xs"
            className="me-2"
          />{" "}
          RavenDB {!isCloudPath && "Cloud"} Docs
          <small className="flex items-center ms-auto gap-1 text-[0.675rem]">
            Switch <Icon icon="arrow-thin-right" size="xs" />
          </small>
        </Link>
      </li>
      <li className="menu__list-item">
        <Link
          to="https://ravendb.net/community"
          className="menu__link group"
          onClick={() => mobileSidebar.toggle()}
        >
          <Icon icon="community" size="xs" className="me-2" /> Community
          <Icon icon="newtab" size="xs" className="ms-auto" />
        </Link>
      </li>
      {!isCloudPath && (
        <li className="menu__list-item">
          <Link
            to={`/${versionLabel}/whats-new`}
            className="menu__link"
            onClick={() => mobileSidebar.toggle()}
          >
            <Icon icon="star-filled" size="xs" className="me-2" /> What's new
          </Link>
        </li>
      )}
      <li className="menu__list-item !my-3">
        <hr className="!my-0 !mx-3 !bg-black/10 dark:!bg-white/10" />
      </li>
      {!isCloudPath && <SidebarVersionDropdown />}
      <DocSidebarItems
        items={sidebar}
        activePath={path}
        onItemClick={(item) => {
          // Mobile sidebar should only be closed if the category has a link
          if (item.type === "category" && item.href) {
            mobileSidebar.toggle();
          }
          if (item.type === "link") {
            mobileSidebar.toggle();
          }
        }}
        level={1}
      />
    </ul>
  );
}

function DocSidebarMobile(props: Props) {
  return (
    <NavbarSecondaryMenuFiller
      component={DocSidebarMobileSecondaryMenu}
      props={props}
    />
  );
}

export default React.memo(DocSidebarMobile);
