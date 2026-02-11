import React from "react";
import Link from "@docusaurus/Link";
import clsx from "clsx";
import { ThemeClassNames } from "@docusaurus/theme-common";

export interface BreadcrumbItem {
    label: string;
    href?: string;
}

export interface BreadcrumbsProps {
    items: BreadcrumbItem[];
    className?: string;
}

export default function CustomBreadcrumbs({ items, className }: BreadcrumbsProps) {
    if (!items || items.length === 0) {
        return null;
    }

    return (
        <nav
            className={clsx(
                "breadcrumbsContainer",
                ThemeClassNames.docs.docBreadcrumbs,
                "theme-doc-breadcrumbs",
                className
            )}
            aria-label="Breadcrumbs"
        >
            <ul className="breadcrumbs" itemScope itemType="https://schema.org/BreadcrumbList">
                {items.map((item, index) => {
                    const isLast = index === items.length - 1;

                    return (
                        <li
                            key={index}
                            className={clsx("breadcrumbs__item", {
                                "breadcrumbs__item--active": isLast,
                            })}
                            itemProp="itemListElement"
                            itemScope
                            itemType="https://schema.org/ListItem"
                        >
                            {!isLast && item.href ? (
                                <Link className="breadcrumbs__link" itemProp="item" to={item.href}>
                                    <span itemProp="name">{item.label}</span>
                                </Link>
                            ) : (
                                <span className="breadcrumbs__link breadcrumbs__link--active" itemProp="name">
                                    {item.label}
                                </span>
                            )}
                            <meta itemProp="position" content={String(index + 1)} />
                        </li>
                    );
                })}
            </ul>
        </nav>
    );
}
