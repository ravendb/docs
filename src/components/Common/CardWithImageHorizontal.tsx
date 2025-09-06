import React, { ReactNode } from "react";
import Heading from "@theme/Heading";
import Button from "@site/src/components/Common/Button";
import { IconName } from "@site/src/typescript/iconName";
import Badge from "@site/src/components/Common/Badge";
import isInternalUrl from "@docusaurus/isInternalUrl";

export interface CardWithImageHorizontalProps {
  title: string;
  description: ReactNode;
  imgSrc: string;
  imgAlt?: string;
  url?: string;
  buttonVariant?: "default" | "outline" | "ghost" | "destructive" | "secondary";
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
  iconName,
}: CardWithImageHorizontalProps) {
  return (
    <div className="card group flex !flex-row items-center gap-4 overflow-hidden rounded-2xl border border-black/10 dark:border-white/10 bg-muted/40 p-4 transition-colors">
      <div className="basis-1/3 flex-shrink-0 overflow-hidden rounded-xl relative flex items-center">
        <img
          src={imgSrc}
          alt={imgAlt}
          className="pointer-events-none w-full h-auto object-contain transition-transform origin-bottom duration-500 group-hover:scale-105"
        />
        {url && !isInternalUrl(url) && (
          <Badge className="absolute top-2 right-2" variant="default" size="sm">
            External
          </Badge>
        )}
      </div>
      <div className="flex flex-col flex-1 min-w-0 justify-center items-start gap-1">
        <Heading as="h4" className="!mb-1">
          {title}
        </Heading>
        <p className="!mb-3 text-sm">{description}</p>
        {url && (
          <Button variant={buttonVariant} size="sm" url={url} className="self-start">
            {ctaLabel}
          </Button>
        )}
      </div>
    </div>
  );
}
