import React from "react";
import Link from "@docusaurus/Link";
import Heading from "@theme/Heading";
import { Icon } from "@site/src/components/Common/Icon";
import RecentGuidesList from "./RecentGuidesList";
import clsx from "clsx";
import { usePluginData } from "@docusaurus/useGlobalData";

function getRelativeTime(timestampSeconds: number) {
    const diffInSeconds = Math.floor(Date.now() / 1000 - timestampSeconds);

    const intervals = [
        { label: "y", seconds: 31536000 },
        { label: "mo", seconds: 2592000 },
        { label: "w", seconds: 604800 },
        { label: "d", seconds: 86400 },
        { label: "h", seconds: 3600 },
        { label: "m", seconds: 60 },
    ];

    for (const interval of intervals) {
        const count = Math.floor(diffInSeconds / interval.seconds);
        if (count >= 1) {
            return `${count}${interval.label} ago`;
        }
    }
    return "Just now";
}

export default function RecentGuides() {
    const pluginData = usePluginData("recent-guides-plugin") as {
        guides: any[];
    };
    const docs = pluginData?.guides || [];

    const recentGuides = docs
        .filter((doc) => doc.id !== "home")
        .map((doc: any) => ({
            title: doc.title || doc.id,
            url: doc.permalink,
            tags: doc.tags || [],
            time: doc.lastUpdatedAt
                ? getRelativeTime(doc.lastUpdatedAt)
                : "Recently",
            lastUpdatedAt: doc.lastUpdatedAt || 0,
        }))
        .slice(0, 10);

    return (
        <div className="flex flex-col gap-6 lg:h-[0px] lg:min-h-full">
            <div className="flex flex-wrap items-center justify-between shrink-0">
                <Heading as="h2" className="!mb-0">
                    Recent guides
                </Heading>
                <Link
                    to="/guides/all"
                    className="flex items-center gap-2 text-sm font-semibold text-primary hover:underline group"
                >
                    See all
                    <Icon icon="arrow-thin-right" size="xs" />
                </Link>
            </div>
            <div
                className={clsx(
                    "border border-black/10 dark:border-white/10 rounded-2xl bg-black/5 dark:bg-white/5",
                    "overflow-y-auto flex flex-col flex-1 min-h-0",
                    "scrollbar-thin scrollbar-thumb-black/10 dark:scrollbar-thumb-white/10 scrollbar-track-transparent",
                )}
            >
                <RecentGuidesList guides={recentGuides} />
            </div>
        </div>
    );
}
