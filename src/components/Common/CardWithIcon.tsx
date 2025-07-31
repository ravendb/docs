import React, { ReactNode } from "react";
import Link from "@docusaurus/Link";
import Heading from "@theme/Heading";

export interface CardWithIconProps {
  title: string;
  description: ReactNode;
  url?: string;
}

export default function CardWithIcon({
  title,
  description,
  url,
}: CardWithIconProps) {
  const content = (
    <div className="card flex items-start gap-4 rounded-2xl border border-black/10 dark:border-white/10 bg-muted/40 p-4 hover:border-black/20 dark:hover:border-white/20 transition-colors">
      <div className="flex flex-col gap-2">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="16"
          viewBox="0 0 831 840"
          fill="currentColor"
          className="shrink-0"
        >
          <path d="M725.4 461.601L829.4 541.601L819.8 557.601L735 703.201L727 717.601L605.4 669.601C581.4 687.201 559 700.001 535 711.201L517.4 839.201C517.4 839.201 511 839.201 498.2 839.201H331.8C320.6 839.201 312.6 839.201 312.6 839.201L295 711.201C269.4 700.001 247 687.201 224.6 669.601L103 717.601L94.9996 703.201L10.1996 557.601L0.599609 541.601L104.6 461.601C103 445.601 103 432.801 103 420.001C103 407.201 103 394.401 104.6 378.401L0.599609 298.401L10.1996 282.401L94.9996 136.801L103 122.401L224.6 170.401C248.6 152.801 271 140.001 295 128.801L314.2 0.800781H519L536.6 128.801C562.2 140.001 584.6 152.801 607 170.401L728.6 122.401L831 298.401L727 378.401C730.2 394.401 730.2 407.201 730.2 420.001C730.2 432.801 728.6 445.601 725.4 461.601ZM312.6 524.001C341.4 552.801 375 567.201 415 567.201C455 567.201 488.6 552.801 517.4 524.001C546.2 495.201 560.6 460.001 560.6 420.001C560.6 380.001 546.2 344.801 517.4 316.001C488.6 287.201 455 272.801 415 272.801C375 272.801 341.4 287.201 312.6 316.001C283.8 344.801 269.4 380.001 269.4 420.001C269.4 460.001 283.8 495.201 312.6 524.001Z" />
        </svg>
        <div>
          <Heading as="h4" className="!mb-1">
            {title}
          </Heading>
          <p className="!mb-0 text-sm">{description}</p>
        </div>
      </div>
    </div>
  );

  return url ? (
    <Link to={url} className="card-wrapper">
      {content}
    </Link>
  ) : (
    content
  );
}
