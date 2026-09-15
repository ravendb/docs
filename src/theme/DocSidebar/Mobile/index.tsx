import React from "react";
import clsx from "clsx";
import { NavbarSecondaryMenuFiller, ThemeClassNames } from "@docusaurus/theme-common";
import { useNavbarMobileSidebar } from "@docusaurus/theme-common/internal";
import DocSidebarItems from "@theme/DocSidebarItems";
import type { Props } from "@theme/DocSidebar/Mobile";
import Link from "@docusaurus/Link";
import { useActiveDocContext, useLatestVersion } from "@docusaurus/plugin-content-docs/client";
import { Icon } from "@site/src/components/Common/Icon";
import { SectionNavLink } from "@site/src/components/Common/SectionNavLink";
import type { Props as DocSidebarProps } from "@theme/DocSidebar";
import SidebarVersionDropdown from "@site/src/components/SidebarVersionDropdown";
import { getPathType, getSectionNavItems, PathType } from "../../../typescript/pathUtils";

function DocSidebarMobileSecondaryMenu({ sidebar, path }: DocSidebarProps) {
    const mobileSidebar = useNavbarMobileSidebar();

    const pluginId = "default";
    const { activeVersion } = useActiveDocContext(pluginId);
    const latestVersion = useLatestVersion(pluginId);
    const versionLabel = activeVersion?.label ?? latestVersion.label;

    const pathType = getPathType(path);
    const sectionNavItems = getSectionNavItems(pathType, versionLabel);

    const shouldDisplayContent = pathType !== PathType.Guides && pathType !== PathType.Samples;

    return (
        <ul className={clsx(ThemeClassNames.docs.docSidebarMenu, "menu__list")}>
            {sectionNavItems.map((item) => (
                <li key={item.label} className="menu__list-item">
                    <SectionNavLink
                        item={item}
                        isActive={item.pathType === pathType}
                        onClick={() => mobileSidebar.toggle()}
                    />
                </li>
            ))}
            {pathType === PathType.Documentation && (
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
            {shouldDisplayContent && (
                <li className="menu__list-item !my-3">
                    <hr className="!my-0 !mx-3 !bg-black/10 dark:!bg-white/10" />
                </li>
            )}
            {pathType === PathType.Documentation && <SidebarVersionDropdown />}
            {shouldDisplayContent && (
                <DocSidebarItems
                    items={sidebar}
                    activePath={path}
                    onItemClick={(item) => {
                        if (item.type === "category" && item.href) {
                            mobileSidebar.toggle();
                        }
                        if (item.type === "link") {
                            mobileSidebar.toggle();
                        }
                    }}
                    level={1}
                />
            )}
        </ul>
    );
}

function DocSidebarMobile(props: Props) {
    return <NavbarSecondaryMenuFiller component={DocSidebarMobileSecondaryMenu} props={props} />;
}

export default React.memo(DocSidebarMobile);
