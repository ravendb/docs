import React from "react";
import Link from "@docusaurus/Link";
import { Icon } from "@site/src/components/Common/Icon";
import { getIconName, parsePath } from "./utils";
import clsx from "clsx";
import { SeeAlsoItemType } from "./types";

interface SeeAlsoItemProps {
    item: SeeAlsoItemType;
    versionedLink: string;
}

export function SeeAlsoItem({ item, versionedLink }: SeeAlsoItemProps) {
    const iconName = getIconName(item.source);
    const { mainCategory, restOfPath, fullPath } = parsePath(item.path);

    return (
        <Link
            href={versionedLink}
            className={clsx(
                "flex items-center py-3 px-2 gap-4",
                "border-b border-black/10 dark:border-white/10 !text-inherit",
                "hover:!no-underline",
                "group relative",
                "!transition-all",
                "hover:bg-black/5 dark:hover:bg-white/5",
            )}
        >
            <div className="flex items-center gap-2 flex-1 min-w-0">
                <Icon icon={iconName} size="xs" />
                <span
                    className={clsx(
                        "!mb-0 text-base font-semibold leading-5",
                        "overflow-hidden truncate whitespace-nowrap",
                    )}
                    title={item.title}
                >
                    {item.title}
                </span>
            </div>
            <div
                className="text-xs text-right truncate relative text-black/60 dark:text-white/60"
                title={fullPath}
            >
                <span className="hidden sm:inline-flex truncate">
                    {mainCategory}
                </span>
                {restOfPath && (
                    <span className="hidden md:inline-flex path-collapse text-black/40 dark:text-white/40">
                        {" > "}
                        {restOfPath}
                    </span>
                )}
            </div>
        </Link>
    );
}
