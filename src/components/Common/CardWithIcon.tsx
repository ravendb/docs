import React, { ReactNode } from "react";
import Link from "@docusaurus/Link";
import Heading from "@theme/Heading";
import { Icon } from "@site/src/components/Common/Icon";
import { IconName } from "@site/src/typescript/iconName";
import clsx from "clsx";

export interface CardWithIconProps {
    title: string;
    icon: IconName;
    description?: ReactNode;
    url: string;
    animationDelay?: number;
}

export default function CardWithIcon({ title, icon, description, url, animationDelay = 0 }: CardWithIconProps) {
    return (
        <Link to={url} className="card-wrapper">
            <div
                className={clsx(
                    "card flex items-start gap-4",
                    "p-4 overflow-hidden rounded-2xl",
                    "border border-black/10 dark:border-white/10",
                    "!bg-black/5 dark:!bg-white/5",
                    "hover:border-black/20 dark:hover:border-white/20",
                    "hover:!bg-black/10 dark:hover:!bg-white/10",
                    "!transition-all",
                    "animate-in fade-in slide-in-from-bottom-4"
                )}
                style={{
                    animationDelay: `${animationDelay}ms`,
                    animationDuration: "400ms",
                    animationFillMode: "backwards",
                }}
            >
                <div className="flex flex-col gap-2">
                    <Icon icon={icon} />
                    <div>
                        <Heading as="h4" className="!mb-1">
                            {title}
                        </Heading>
                        <p className="!mb-0 text-sm">{description}</p>
                    </div>
                </div>
            </div>
        </Link>
    );
}
