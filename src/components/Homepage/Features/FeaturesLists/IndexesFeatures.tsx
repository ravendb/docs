import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { IconName } from "@site/src/typescript/iconName";
import { Feature } from "@site/src/typescript/feature";

export default function IndexesFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const IndexesFeatures: Feature[] = [
    {
      title: "Static indexes",
      icon: "index",
      url: `/${activeVersion.label}/indexes/creating-and-deploying`,
      description: "Complete control over precomputing & performance",
    },
    {
      title: "Auto indexes",
      icon: "auto-indexes",
      url: `/${activeVersion.label}/indexes/creating-and-deploying#auto-indexes`,
      description: "Self‑optimizing database with zero manual effort",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Indexes
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {IndexesFeatures.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
