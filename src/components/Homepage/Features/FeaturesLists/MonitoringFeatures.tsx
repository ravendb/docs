import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function MonitoringFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const MonitoringFeaturesList = [
    {
      title: "Cluster dashboard",
      url: `${activeVersion.label}/studio/cluster/cluster-dashboard/cluster-dashboard-overview`,
      description: "Short description",
    },
    {
      title: "SNMP monitoring",
      url: `${activeVersion.label}/server/administration/snmp/snmp-overview`,
      description: "Short description",
    },
    {
      title: "Telegraf and Grafana",
      url: `${activeVersion.label}/server/administration/monitoring/telegraf`,
      description: "Short description",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Monitoring
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {MonitoringFeaturesList.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
