import React, { type ReactNode } from "react";
import clsx from "clsx";
import { useNavbarSecondaryMenu } from "@docusaurus/theme-common/internal";
import { ThemeClassNames } from "@docusaurus/theme-common";
import type { Props } from "@theme/Navbar/MobileSidebar/Layout";

// React 19 accepts a boolean `inert` prop natively (React 18 and earlier
// required passing an empty string). We pin react ^19 in package.json,
// so the legacy string-coercion workaround is dead code and has been
// removed. See https://github.com/facebook/react/issues/17157.
function NavbarMobileSidebarPanel({ children, inert }: { children: ReactNode; inert: boolean }) {
    return (
        <div
            className={clsx(ThemeClassNames.layout.navbar.mobileSidebar.panel, "navbar-sidebar__item menu")}
            inert={inert}
        >
            {children}
        </div>
    );
}

export default function NavbarMobileSidebarLayout({ header, primaryMenu, secondaryMenu }: Props): ReactNode {
    const { shown: secondaryMenuShown } = useNavbarSecondaryMenu();
    return (
        <div className={clsx(ThemeClassNames.layout.navbar.mobileSidebar.container, "navbar-sidebar")}>
            {header}
            <div
                className={clsx("navbar-sidebar__items", {
                    "navbar-sidebar__items--show-secondary": secondaryMenuShown,
                })}
            >
                <NavbarMobileSidebarPanel inert={secondaryMenuShown}>{primaryMenu}</NavbarMobileSidebarPanel>
                <NavbarMobileSidebarPanel inert={!secondaryMenuShown}>{secondaryMenu}</NavbarMobileSidebarPanel>
            </div>
        </div>
    );
}
