import React, { ReactNode } from "react";
import Link from "@docusaurus/Link";
import Heading from "@theme/Heading";
import { IconName } from "@site/src/typescript/iconName";
import Badge from "@site/src/components/Common/Badge";
import LazyImage from "@site/src/components/Common/LazyImage";
import isInternalUrl from "@docusaurus/isInternalUrl";
import { clsx } from "clsx";

export interface CardWithImageHorizontalProps {
    title: string;
    description: ReactNode;
    imgSrc: string | { light: string; dark: string };
    imgAlt?: string;
    url: string;
    iconName?: IconName;
}

export default function CardWithImageHorizontal({
    title,
    description,
    imgSrc,
    imgAlt = "",
    url,
}: CardWithImageHorizontalProps) {
    return (
        <Link to={url} className="card-wrapper">
            <div
                className={clsx(
                    "card group !grid grid-cols-1 xl:grid-cols-[120px_1fr]",
                    "items-center gap-4 p-4",
                    "overflow-hidden rounded-2xl",
                    "border border-black/10 dark:border-white/10",
                    "!transition-all",
                    "!bg-black/5 dark:!bg-white/5",
                    "hover:border-black/20 dark:hover:border-white/20",
                    "hover:!bg-black/10 dark:hover:!bg-white/10",
                )}
            >
                <div
                    className={clsx(
                        "aspect-[537/281] xl:max-w-[120px]",
                        "overflow-hidden rounded-xl",
                        "relative flex items-center",
                    )}
                >
                    <LazyImage
                        imgSrc={imgSrc}
                        alt={imgAlt}
                        className={clsx(
                            "pointer-events-none",
                            "w-full h-auto object-contain",
                            "!transition-transform origin-bottom",
                            "group-hover:scale-105",
                        )}
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
                <div className="flex flex-col min-w-0 justify-center gap-1">
                    <Heading as="h4" className="!mb-0">
                        {title}
                    </Heading>
                    <p className="!mb-0 text-sm">{description}</p>
                </div>
            </div>
        </Link>
    );
}
