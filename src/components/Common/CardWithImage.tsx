import React, { ReactNode } from "react";
import Heading from "@theme/Heading";
import ThemedImage from "@theme/ThemedImage";
import Button, { type ButtonVariant } from "@site/src/components/Common/Button";
import { Icon } from "@site/src/components/Common/Icon";
import { IconName } from "@site/src/typescript/iconName";
import Badge from "@site/src/components/Common/Badge";
import isInternalUrl from "@docusaurus/isInternalUrl";

export interface CardWithImageProps {
  title: string;
  description: ReactNode;
  imgSrc: string | { light: string; dark: string };
  imgAlt?: string;
  url?: string;
  buttonVariant?: ButtonVariant;
  ctaLabel?: string;
  iconName?: IconName;
  imgIcon?: IconName;
}

export default function CardWithImage({
  title,
  description,
  imgSrc,
  imgAlt = "",
  url,
  buttonVariant = "secondary",
  ctaLabel = "Read now",
  iconName,
  imgIcon,
}: CardWithImageProps) {
  const hasImage = Boolean(imgSrc);
  const isThemedImage = typeof imgSrc === "object" && imgSrc !== null;
  
  return (
    <div className="card group flex flex-col overflow-hidden rounded-2xl border border-black/10 dark:border-white/10 bg-muted/40 p-4 transition-colors">
      <div className={`flex items-center justify-center rounded-xl mb-4 overflow-hidden relative aspect-[79/24] ${hasImage ? "bg-black/40" : "bg-gradient-to-b from-[#204879] to-[#0F1425] to-[70%]"}`}> 
        {hasImage ? (
          isThemedImage ? (
            <ThemedImage
              alt={imgAlt}
              sources={{
                light: imgSrc.light,
                dark: imgSrc.dark,
              }}
              className="pointer-events-none w-full h-full object-cover object-center transition-transform origin-bottom duration-500 group-hover:scale-105 group-hover:translate-y-1"
            />
          ) : (
            <img
              src={imgSrc as string}
              alt={imgAlt}
              className="pointer-events-none w-full h-full object-cover object-center transition-transform origin-bottom duration-500 group-hover:scale-105 group-hover:translate-y-1"
            />
          )
        ) : (
          <Icon icon={imgIcon ?? "default"} size="xl" className="filter brightness-0 invert" />
        )}
        {url && !isInternalUrl(url) && (
          <Badge className="absolute top-2 right-2" variant="default" size="sm">
            External
          </Badge>
        )}
      </div>
      <Heading as="h4" className="!mb-2">
        {title}
      </Heading>
      <p className="!mb-3 text-sm">{description}</p>
      {url && (
        <Button variant={buttonVariant} url={url} className="mt-auto">
          {ctaLabel}
        </Button>
      )}
    </div>
  );
}
