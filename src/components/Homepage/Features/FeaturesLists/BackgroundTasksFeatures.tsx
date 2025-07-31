import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";

const BackgroundTasksFeaturesList = [
  {
    title: "Expiration",
    url: "/server/extensions/expiration",
    description: "Short description",
  },
  {
    title: "Refresh",
    url: "/server/extensions/refresh",
    description: "Short description",
  },
  {
    title: "Archival",
    url: "/server/extensions/archival",
    description: "Short description",
  },
];

export default function BackgroundTasksFeaturesGrid() {
  return (
    <>
      <Heading as="h4" className="!mb-2">
        Background tasks
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {BackgroundTasksFeaturesList.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
