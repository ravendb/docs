import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function AdministrationFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const AdministrationFeaturesList = [
    {
      title: "Studio",
      url: `/${activeVersion.label}/studio/overview`,
      description: "Short description",
    },
    {
      title: "RavenCLI",
      url: `/${activeVersion.label}/server/administration/cli`,
      description: "Short description",
    },
    {
      title: "NLog",
      url: `/${activeVersion.label}/server/troubleshooting/logging#configuring-and-using-nlog`,
      description: "Short description",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Administration
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {AdministrationFeaturesList.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
