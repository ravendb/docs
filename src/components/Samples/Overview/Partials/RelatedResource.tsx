import React from "react";
import clsx from "clsx";
import { Icon } from "@site/src/components/Common/Icon";
import { IconName } from "@site/src/typescript/iconName";
import Link from "@docusaurus/Link";
import { useLatestVersion } from "@site/src/hooks/useLatestVersion";

type ResourceType = "guide" | "documentation";

type DocumentationType = "docs" | "cloud";

export interface RelatedResourceProps {
    className?: string;
    type: ResourceType;
    documentationType?: DocumentationType;
    subtitle: string;
    articleKey: string;
}

const TYPE_CONFIG: Record<ResourceType, { title: string; icon: IconName }> = {
    guide: {
        title: "Guide",
        icon: "guides",
    },
    documentation: {
        title: "Documentation",
        icon: "database",
    },
};

export default function RelatedResource({
    className,
    type,
    documentationType = "docs",
    subtitle,
    articleKey,
}: RelatedResourceProps) {
    const latestVersion = useLatestVersion() as string;
    const config = TYPE_CONFIG[type];

    const url = React.useMemo(() => {
        if (type === "guide") {
            return `/guides/${articleKey}`;
        }

        const basePath = documentationType === "cloud" ? "/cloud" : `/${latestVersion}`;
        return `${basePath}/${articleKey}`;
    }, [type, documentationType, articleKey, latestVersion]);

    const icon = type === "documentation" && documentationType === "cloud" ? "cloud" : config.icon;

    return (
        <Link
            to={url}
            className={clsx(
                "flex items-center gap-2.5",
                "px-2.5 py-1.5 rounded-lg",
                "hover:bg-black/5 dark:hover:bg-white/5",
                "!transition-colors",
                "!no-underline hover:!no-underline",
                className
            )}
        >
            <div
                className={clsx(
                    "flex items-center justify-center",
                    "p-1.5 rounded",
                    "border border-black/10 dark:border-white/10",
                    "shrink-0 size-7"
                )}
            >
                <Icon icon={icon} size="2xs" className="text-black dark:text-white" />
            </div>
            <div className="flex flex-col flex-1 min-w-0">
                <p className="text-sm leading-5 text-black dark:text-white !mb-0 truncate">{config.title}</p>
                <p className="text-xs leading-4 text-black/60 dark:text-white/60 !mb-0 truncate">{subtitle}</p>
            </div>
        </Link>
    );
}
