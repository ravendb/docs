import React from "react";
import clsx from "clsx";
import Link from "@docusaurus/Link";
import { Icon } from "@site/src/components/Common/Icon";
import type { SectionNavItem } from "@site/src/typescript/pathUtils";

interface SectionNavLinkProps {
    item: SectionNavItem;
    isActive: boolean;
    iconClassName?: string;
    onClick?: () => void;
}

export function SectionNavLink({ item, isActive, iconClassName, onClick }: SectionNavLinkProps) {
    return (
        <Link
            to={item.to}
            onClick={onClick}
            aria-current={isActive ? "page" : undefined}
            className={clsx("menu__link", isActive && "menu__link--active")}
        >
            <Icon icon={item.icon} size="xs" className={clsx("me-2", iconClassName)} />
            {item.label}
            {item.external && <Icon icon="newtab" size="xs" className="ms-auto opacity-60" />}
        </Link>
    );
}
