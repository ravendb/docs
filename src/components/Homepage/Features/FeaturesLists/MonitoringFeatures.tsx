import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function MonitoringFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const monitoringFeatures: Feature[] = [
    {
      title: "Cluster dashboard",
      icon: "cluster-dashboard",
      url: `/${activeVersion.label}/studio/cluster/cluster-dashboard/cluster-dashboard-overview`,
      description:
        "Customizable live view of your cluster's health and performance",
    },
    {
      title: "SNMP monitoring",
      icon: "snmp",
      url: `/${activeVersion.label}/server/administration/snmp/snmp-overview`,
      description: "Easily expose RavenDB metrics to Zabbix/Datadog/PTRG",
    },
    {
      title: "Telegraf and Grafana",
      icon: "telegraf-and-grafana",
      url: `/${activeVersion.label}/server/administration/monitoring/telegraf`,
      description:
        "Effortlessly push RavenDB metrics into your dashboards via plugin",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Monitoring
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {monitoringFeatures.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
