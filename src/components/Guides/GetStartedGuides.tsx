import React from "react";
import CardWithIcon from "../Common/CardWithIcon";
import Heading from "@theme/Heading";
import { IconName } from "@site/src/typescript/iconName";
import { useLatestVersion } from "@site/src/hooks/useLatestVersion";

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
            "Learn how to handle documents efficiently: explore data modeling, document structure, and CRUD operations.",
        icon: "database",
        url: "/studio/database/documents/documents-and-collections/",
    },
    {
        title: "Get started with Querying",
        description:
            "Master the fundamentals of querying: understand filtering, projections, and advanced query techniques.",
        icon: "query",
        url: "/client-api/session/querying/how-to-query",
    },
    {
        title: "Get started with Indexing",
        description:
            "Dive into indexing: learn how to optimize searches, improve performance, and manage indexes effectively.",
        icon: "index",
        url: "/indexes/what-are-indexes",
    },
    {
        title: "Get started with Attachments",
        description:
            "Work with attachments: store, manage, and retrieve files associated with your documents seamlessly.",
        icon: "attachment",
        url: "/document-extensions/attachments/overview",
    },
    {
        title: "Get started with RQL",
        description:
            "Unlock the full power of Raven Query Language (RQL): write expressive, efficient, and precise queries.",
        icon: "rql",
        url: "/client-api/session/querying/what-is-rql",
    },
    {
        title: "Get started with Studio",
        description:
            "Navigate RavenDB Studio: manage your data, analyze performance metrics, and simplify database operations using the visual interface.",
        icon: "studio",
        url: "/studio/overview",
    },
    {
        title: "Get started with Compare-Exchange",
        description:
            "Leverage Compare-Exchange to achieve atomic operations across distributed environments and ensure data integrity.",
        icon: "cluster-wide-transactions",
        url: "/compare-exchange/start",
    },
    {
        title: "Get started with Counters",
        description:
            "Learn how to use counters: track numeric values, such as statistics or metrics, in your application efficiently.",
        icon: "new-counter",
        url: "/document-extensions/counters/overview",
    },
];

export default function GetStartedGuides() {
    const latestVersion = useLatestVersion();
    return (
        <div className="flex flex-col gap-6 items-start relative w-full mb-8">
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
                        url={latestVersion + guide.url}
                    />
                ))}
            </div>
        </div>
    );
}
