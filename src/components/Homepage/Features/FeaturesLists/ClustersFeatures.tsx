import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function ClustersFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const ClustersFeaturesList = [
    {
      title: "Sharding",
      url: `/${activeVersion.label}/sharding/overview`,
      description: "Short description",
    },
    {
      title: "Highly available tasks",
      url: `/${activeVersion.label}/server/clustering/distribution/highly-available-tasks`,
      description: "Short description",
    },
    {
      title: "Cluster-wide transactions",
      url: `/${activeVersion.label}/server/clustering/cluster-transactions`,
      description: "Short description",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Clusters
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {ClustersFeaturesList.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
