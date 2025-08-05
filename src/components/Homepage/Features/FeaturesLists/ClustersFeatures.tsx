import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function ClustersFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const ClustersFeatures: Feature[] = [
    {
      title: "Sharding",
      icon: "sharding",
      url: `/${activeVersion.label}/sharding/overview`,
      description:
        "Partition dozens of terabytes across nodes for extreme scale",
    },
    {
      title: "Cluster-wide tasks",
      icon: "cluster-wide-tasks",
      url: `/${activeVersion.label}/server/clustering/distribution/highly-available-tasks`,
      description:
        "Cluster-wide, auto‑failover tasks (backup, ETL, subscriptions)",
    },
    {
      title: "Cluster-wide transactions",
      icon: "cluster-wide-transactions",
      url: `/${activeVersion.label}/server/clustering/cluster-transactions`,
      description: "Partition-tolerant ACID writes across a cluster",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Clusters
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {ClustersFeatures.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
