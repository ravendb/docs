import React, { ReactNode } from "react";
import Link from "@docusaurus/Link";
import Heading from "@theme/Heading";
import { Icon } from "@site/src/components/Common/Icon";
import { IconName } from "@site/src/typescript/iconName";
import Badge from "@site/src/components/Common/Badge";
import isInternalUrl from "@docusaurus/isInternalUrl";
import clsx from "clsx";

export interface CardProps {
    title: string;
    description: ReactNode;
    url: string;
    iconName?: IconName;
}

export default function Card({ title, description, url, iconName }: CardProps) {
    return (
        <Link to={url} className="card-wrapper">
            <div
                className={clsx(
                    "card group flex h-full flex-col",
                    "p-4 overflow-hidden rounded-2xl",
                    "border border-black/10 dark:border-white/10",
                    "bg-black/5 dark:bg-white/5",
                    "hover:border-black/20 dark:hover:border-white/20",
                    "hover:bg-black/10 dark:hover:bg-white/10",
                    "!transition-all",
                )}
            >
                {iconName && (
                    <div className="flex items-center justify-center rounded-xl mb-4 bg-gradient-to-b from-[#204879] to-[#0F1425] to-[70%] aspect-[79/24] relative">
                        <Icon
                            icon={iconName}
                            size="xl"
                            className="filter brightness-0 invert"
                        />
                        {!isInternalUrl(url) && (
                            <Badge
                                className="absolute top-2 right-2"
                                variant="default"
                                size="sm"
                            >
                                External
                            </Badge>
                        )}
                    </div>
                )}
                <Heading as="h4" className="!mb-2">
                    {title}
                </Heading>
                <p className="!mb-0 text-sm">{description}</p>
            </div>
        </Link>
    );
}
