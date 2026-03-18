import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function ClustersFeaturesGrid() {
    const pluginId = "default";
    const minimumCategorySupportedVersion = "4.0";
    const { activeVersion } = useActiveDocContext(pluginId);

    if (minimumCategorySupportedVersion > activeVersion.label) {
        return null;
    }

    const clustersFeatures: Feature[] = [
        {
            title: "Sharding",
            icon: "sharding",
            url: `/${activeVersion.label}/sharding/overview`,
            description: "Partition dozens of terabytes across nodes for extreme scale",
            minimumSupportedVersion: "6.0",
        },
        {
            title: "Cluster-wide tasks",
            icon: "cluster-wide-tasks",
            url: `/${activeVersion.label}/server/clustering/distribution/highly-available-tasks`,
            description: "Cluster-wide, auto‑failover tasks (backup, ETL, subscriptions)",
            minimumSupportedVersion: "4.0",
        },
        {
            title: "Cluster-wide transactions",
            icon: "cluster-wide-transactions",
            url: `/${activeVersion.label}/server/clustering/cluster-transactions`,
            description: "Partition-tolerant ACID writes across a cluster",
            minimumSupportedVersion: "4.1",
        },
    ];

    return (
        <>
            <Heading as="h3" className="!mb-2">
                Clusters
            </Heading>
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
                {clustersFeatures
                    .filter(
                        (feature) =>
                            !feature.minimumSupportedVersion || feature.minimumSupportedVersion <= activeVersion.label
                    )
                    .map((props, idx) => (
                        <FeatureItem key={idx} {...props} />
                    ))}
            </div>
        </>
    );
}
