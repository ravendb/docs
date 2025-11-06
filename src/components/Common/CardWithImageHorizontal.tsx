import React, { ReactNode } from "react";
import Heading from "@theme/Heading";
import Button, { type ButtonVariant } from "@site/src/components/Common/Button";
import { IconName } from "@site/src/typescript/iconName";
import Badge from "@site/src/components/Common/Badge";
import CardImage from "@site/src/components/Common/CardImage";
import isInternalUrl from "@docusaurus/isInternalUrl";

export interface CardWithImageHorizontalProps {
    title: string;
    description: ReactNode;
    imgSrc: string | { light: string; dark: string };
    imgAlt?: string;
    url?: string;
    buttonVariant?: ButtonVariant;
    ctaLabel?: string;
    iconName?: IconName;
}

export default function CardWithImageHorizontal({
    title,
    description,
    imgSrc,
    imgAlt = "",
    url,
    buttonVariant = "secondary",
    ctaLabel = "Read now",
}: CardWithImageHorizontalProps) {
    return (
        <div className="card group !grid grid-cols-1 xl:grid-cols-[minmax(10rem,_230px)_1fr] 2xl:grid-cols-[minmax(10rem,_270px)_1fr] items-center gap-4 overflow-hidden rounded-2xl border border-black/10 dark:border-white/10 bg-muted/40 p-4 transition-colors">
            <div className="aspect-[537/281] overflow-hidden rounded-xl relative flex items-center">
                <CardImage
                    imgSrc={imgSrc}
                    imgAlt={imgAlt}
                    className="pointer-events-none w-full h-auto object-contain transition-transform origin-bottom duration-500 group-hover:scale-105"
                />
                {url && !isInternalUrl(url) && (
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
                <Heading as="h4" className="!mb-1">
                    {title}
                </Heading>
                <p className="!mb-3 text-sm">{description}</p>
                {url && (
                    <Button variant={buttonVariant} url={url}>
                        {ctaLabel}
                    </Button>
                )}
            </div>
        </div>
    );
}
