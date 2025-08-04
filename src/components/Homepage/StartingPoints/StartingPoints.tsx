import type { ReactNode } from "react";
import StartingPoint from "@site/src/components/Homepage/StartingPoints/StartingPointItem";
import Heading from "@theme/Heading";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function StartingPoints(): ReactNode {
    const pluginId = "default";
    const { activeVersion } = useActiveDocContext(pluginId);

    const StartingPointsList = [
        {
            title: "Developer",
            url: `/${activeVersion.label}/start/getting-started#documentstore`,
            description: (
                <>
                    Learn how to create a client, connect to the server, handle documents
                    and more
                </>
            ),
        },
        {
            title: "DevOps",
            url: `/${activeVersion.label}/start/getting-started`,
            description: (
                <>
                    Learn how to install RavenDB, set up a cluster, maintain the database
                    and more
                </>
            ),
        },
    ];

    return (
        <section className="mb-8">
            <Heading as="h3">Starting points</Heading>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                {StartingPointsList.map((props, idx) => (
                    <StartingPoint key={idx} {...props} />
                ))}
            </div>
        </section>
    );
}
