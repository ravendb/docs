import React from "react";
import clsx from "clsx";
import Link from "@docusaurus/Link";
import { Icon } from "@site/src/components/Common/Icon";
import type { SectionNavItem } from "@site/src/typescript/pathUtils";

interface SectionNavLinkProps {
    item: SectionNavItem;
    isActive: boolean;
    onClick?: () => void;
}

export function SectionNavLink({ item, isActive, onClick }: SectionNavLinkProps) {
    return (
        <Link
            to={item.to}
            onClick={onClick}
            // Not "page": the entry points at the section landing page, which is rarely the page you are on.
            aria-current={isActive ? "true" : undefined}
            className={clsx(
                "menu__link",
                isActive && "!bg-black/5 !font-semibold hover:!bg-black/10 dark:!bg-white/5 dark:hover:!bg-white/10"
            )}
        >
            <Icon icon={item.icon} size="xs" className="me-2" />
            {item.label}
            {item.external && <Icon icon="newtab" size="xs" className="ms-auto opacity-60" />}
        </Link>
    );
}
