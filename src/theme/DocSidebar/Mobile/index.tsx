import React from "react";
import clsx from "clsx";
import { NavbarSecondaryMenuFiller, ThemeClassNames } from "@docusaurus/theme-common";
import { useNavbarMobileSidebar } from "@docusaurus/theme-common/internal";
import DocSidebarItems from "@theme/DocSidebarItems";
import type { Props } from "@theme/DocSidebar/Mobile";
import { useActiveDocContext, useLatestVersion } from "@docusaurus/plugin-content-docs/client";
import { SectionNavLink } from "@site/src/components/Common/SectionNavLink";
import { PRODUCT_BRAND_COLORS } from "@site/src/components/Common/productBrandColors";
import type { Props as DocSidebarProps } from "@theme/DocSidebar";
import SidebarVersionDropdown from "@site/src/components/SidebarVersionDropdown";
import SidebarProductSwitcher from "@site/src/components/SidebarProductSwitcher";
import { getPathType, getSidebarNav, PathType } from "../../../typescript/pathUtils";

function DocSidebarMobileSecondaryMenu({ sidebar, path }: DocSidebarProps) {
    const mobileSidebar = useNavbarMobileSidebar();

    const pluginId = "default";
    const { activeVersion } = useActiveDocContext(pluginId);
    const latestVersion = useLatestVersion(pluginId);
    const versionLabel = activeVersion?.label ?? latestVersion.label;

    const pathType = getPathType(path);
    const nav = getSidebarNav(path, versionLabel);

    const shouldDisplayContent = pathType !== PathType.Guides && pathType !== PathType.Samples;

    return (
        <ul className={clsx(ThemeClassNames.docs.docSidebarMenu, "menu__list")}>
            {nav.links.map((item) => (
                <li key={item.label} className="menu__list-item">
                    <SectionNavLink item={item} isActive={item.isActive} onClick={() => mobileSidebar.toggle()} />
                </li>
            ))}
            {nav.productDocs.map((product) => (
                <li key={product.label} className="menu__list-item">
                    <SectionNavLink
                        item={product}
                        isActive={false}
                        iconClassName={PRODUCT_BRAND_COLORS[product.pathType]}
                        onClick={() => mobileSidebar.toggle()}
                    />
                </li>
            ))}
            {nav.currentProduct && (
                <li className="menu__list-item !my-3 flex flex-col gap-3 px-2">
                    <SidebarProductSwitcher
                        products={nav.products}
                        currentProduct={nav.currentProduct}
                        onNavigate={() => mobileSidebar.toggle()}
                    />
                    {pathType === PathType.Documentation && <SidebarVersionDropdown />}
                </li>
            )}
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
            <li className="menu__list-item sticky bottom-0 !mt-3 pt-2 flex flex-col gap-1 border-t border-black/10 dark:border-white/10 bg-[var(--ifm-navbar-background-color)]">
                {nav.footer.map((item) => (
                    <SectionNavLink
                        key={item.label}
                        item={item}
                        isActive={item.isActive}
                        onClick={() => mobileSidebar.toggle()}
                    />
                ))}
            </li>
        </ul>
    );
}

function DocSidebarMobile(props: Props) {
    return <NavbarSecondaryMenuFiller component={DocSidebarMobileSecondaryMenu} props={props} />;
}

export default React.memo(DocSidebarMobile);
