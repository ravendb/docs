import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function BackgroundTasksFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const BackgroundTasksFeaturesList = [
    {
      title: "Expiration",
      url: `${activeVersion.label}/server/extensions/expiration`,
      description: "Short description",
    },
    {
      title: "Refresh",
      url: `${activeVersion.label}/server/extensions/refresh`,
      description: "Short description",
    },
    {
      title: "Archival",
      url: `${activeVersion.label}/server/extensions/archival`,
      description: "Short description",
    },
  ];

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
