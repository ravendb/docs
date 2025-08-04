import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function IndexesFeaturesGrid() {
    const pluginId = "default";
    const { activeVersion } = useActiveDocContext(pluginId);

    const IndexesFeaturesList = [
        {
            title: "Static indexes",
            url: `/${activeVersion.label}/indexes/creating-and-deploying`,
            description: "Short description",
        },
        {
            title: "Auto indexes",
            url: `/${activeVersion.label}/indexes/creating-and-deploying#auto-indexes`,
            description: "Short description",
        },
    ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Indexes
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {IndexesFeaturesList.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
