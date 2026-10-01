import React from "react";
import clsx from "clsx";
import { useThemeConfig } from "@docusaurus/theme-common";
import Logo from "@theme/Logo";
import CollapseButton from "@theme/DocSidebar/Desktop/CollapseButton";
import Content from "@theme/DocSidebar/Desktop/Content";
import type { Props } from "@theme/DocSidebar/Desktop";

import styles from "./styles.module.css";
import SidebarVersionDropdown from "@site/src/components/SidebarVersionDropdown";
import SidebarProductSwitcher from "@site/src/components/SidebarProductSwitcher";

import { useActiveDocContext, useLatestVersion } from "@docusaurus/plugin-content-docs/client";
import { SectionNavLink } from "@site/src/components/Common/SectionNavLink";
import { PRODUCT_BRAND_COLORS } from "@site/src/components/Common/productBrandColors";
import { getPathType, getSidebarNav, PathType } from "../../../typescript/pathUtils";

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
    const nav = getSidebarNav(path, versionLabel);

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
            <nav aria-label="Documentation sections" className="menu shrink-0 !grow-0 p-2 flex flex-col gap-1">
                {nav.links.map((item) => (
                    <SectionNavLink key={item.label} item={item} isActive={item.isActive} />
                ))}
                {nav.productDocs.map((product) => (
                    <SectionNavLink
                        key={product.label}
                        item={product}
                        isActive={false}
                        iconClassName={PRODUCT_BRAND_COLORS[product.pathType]}
                    />
                ))}
            </nav>
            {nav.currentProduct && (
                <div className="flex flex-col gap-3 px-4 pt-1">
                    <SidebarProductSwitcher products={nav.products} currentProduct={nav.currentProduct} />
                    {pathType === PathType.Documentation && <SidebarVersionDropdown />}
                </div>
            )}
            {shouldDisplayContent && <Content path={path} sidebar={sidebar} />}
            <div className="menu shrink-0 !grow-0 mt-auto p-2 flex flex-col gap-1 border-t border-black/10 dark:border-white/10">
                {nav.footer.map((item) => (
                    <SectionNavLink key={item.label} item={item} isActive={item.isActive} />
                ))}
            </div>
            {hideable && <CollapseButton onClick={onCollapse} />}
        </div>
    );
}

export default React.memo(DocSidebarDesktop);
