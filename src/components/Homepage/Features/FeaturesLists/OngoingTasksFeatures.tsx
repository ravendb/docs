import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";

const OngoingTasksFeaturesList = [
  {
    title: "Replication",
    url: "/server/clustering/replication/replication",
    description: "Short description",
  },
  {
    title: "Subscriptions",
    url: "/client-api/data-subscriptions/what-are-data-subscriptions",
    description: "Short description",
  },
  {
    title: "Periodic backups",
    url: "/server/ongoing-tasks/backup-overview",
    description: "Short description",
  },
];

export default function OngoingTasksFeaturesGrid() {
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
