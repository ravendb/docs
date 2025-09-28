import React, { ReactNode } from "react";
import Heading from "@theme/Heading";
import Button, { type ButtonVariant } from "@site/src/components/Common/Button";
import { Icon } from "@site/src/components/Common/Icon";
import { IconName } from "@site/src/typescript/iconName";
import Badge from "@site/src/components/Common/Badge";
import isInternalUrl from "@docusaurus/isInternalUrl";

export interface CardProps {
  title: string;
  description: ReactNode;
  url?: string;
  buttonVariant?: ButtonVariant;
  ctaLabel?: string;
  iconName?: IconName; // optional icon at top
}

export default function Card({
  title,
  description,
  url,
  buttonVariant = "secondary",
  ctaLabel = "Read now",
  iconName,
}: CardProps) {
  return (
    <div className="card group flex flex-col overflow-hidden rounded-2xl border border-black/10 dark:border-white/10 bg-muted/40 p-4 transition-colors">
      {iconName && (
        <div className="flex items-center justify-center rounded-xl mb-4 bg-gradient-to-b from-[#204879] to-[#0F1425] to-[70%] aspect-[79/24]">
          <Icon icon={iconName} size="xl" className="filter brightness-0 invert" />
          {url && !isInternalUrl(url) && (
            <Badge className="absolute top-2 right-2" variant="default" size="sm">
              External
            </Badge>
          )}
        </div>
      )}
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
