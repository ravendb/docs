import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function OngoingTasksFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const OngoingTasksFeaturesList = [
    {
      title: "Replication",
      url: `/${activeVersion.label}/server/clustering/replication/replication-overview`,
      description: "Short description",
    },
    {
      title: "Subscriptions",
      url: `/${activeVersion.label}/client-api/data-subscriptions/what-are-data-subscriptions`,
      description: "Short description",
    },
    {
      title: "Periodic backups",
      url: `/${activeVersion.label}/server/ongoing-tasks/backup-overview`,
      description: "Short description",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Ongoing tasks
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {OngoingTasksFeaturesList.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
