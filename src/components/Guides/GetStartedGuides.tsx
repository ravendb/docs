import React from "react";
import CardWithIcon from "../Common/CardWithIcon";
import Heading from "@theme/Heading";
import { IconName } from "@site/src/typescript/iconName";

interface GuideItem {
    title: string;
    description: string;
    icon: IconName;
    url: string;
}

const GUIDES: GuideItem[] = [
    {
        title: "Get started with Documents",
        description:
            "Understand the core: dive into architecture, data modeling, and fundamental concepts.",
        icon: "database",
        url: "#",
    },
    {
        title: "Get started with Querying",
        description:
            "Scale with ease: strategies for data sharding, replication, and distribution across clusters.",
        icon: "query",
        url: "#",
    },
    {
        title: "Get started with Indexing",
        description:
            "Ensure data reliability: master validation, transactions, and consistency models.",
        icon: "index",
        url: "#",
    },
    {
        title: "Get started with Attachments",
        description:
            "Unlock smart search: explore similarity, NLP, and advanced cognitive techniques.",
        icon: "attachment",
        url: "#",
    },
    {
        title: "Get started with RQL",
        description:
            "Boost performance: techniques for indexing, caching, and query optimization.",
        icon: "rql",
        url: "#",
    },
    {
        title: "Get started with Studio",
        description:
            "Bring intelligence to the edge: data sync, edge analytics, and IoT integration strategies.",
        icon: "studio",
        url: "#",
    },
    {
        title: "Get started with Compare-Exchange",
        description:
            "Build robust data solutions: schema design, performance tuning, and scalability practices.",
        icon: "cluster-wide-transactions",
        url: "#",
    },
    {
        title: "Get started with Counters",
        description:
            "Administer databases effectively: setup, monitoring, troubleshooting, and HA configuration.",
        icon: "new-counter",
        url: "#",
    },
];

export default function GetStartedGuides() {
    return (
        <div className="flex flex-col gap-6 items-start relative w-full mb-12">
            <div className="flex flex-col gap-1 items-start w-full">
                <Heading as="h2" className="!mb-0">
                    Get started with RavenDB
                </Heading>
                <p className="text-base text-black/60 dark:text-white/60 !mb-0">
                    If you’re new to RavenDB, get started by learning how it
                    works.
                </p>
            </div>
            <div className="w-full grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
                {GUIDES.map((guide, index) => (
                    <CardWithIcon
                        key={index}
                        title={guide.title}
                        icon={guide.icon}
                        description={guide.description}
                        url={guide.url}
                    />
                ))}
            </div>
        </div>
    );
}
