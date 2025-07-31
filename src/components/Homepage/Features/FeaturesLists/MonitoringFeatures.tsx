import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";

const MonitoringFeaturesList = [
  {
    title: "Cluster dashboard",
    url: "/studio/cluster/cluster-dashboard/cluster-dashboard-overview",
    description: "Short description",
  },
  {
    title: "SNMP monitoring",
    url: "/server/administration/SNMP/snmp",
    description: "Short description",
  },
  {
    title: "Telegraf and Grafana",
    url: "/server/administration/monitoring",
    description: "Short description",
  },
];

export default function MonitoringFeaturesGrid() {
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
