import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function AdministrationFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const administrationFeatures: Feature[] = [
    {
      title: "Studio",
      icon: "studio",
      url: `/${activeVersion.label}/studio/overview`,
      description:
        "State-of-the-art admin interface bundled in every RavenDB license",
    },
    {
      title: "RavenCLI",
      icon: "raven-cli",
      url: `/${activeVersion.label}/server/administration/cli`,
      description: "Simple yet powerful shell tool for server admin",
    },
    {
      title: "NLog",
      icon: "nlog",
      url: `/${activeVersion.label}/server/troubleshooting/logging#configuring-and-using-nlog`,
      description: "Seamless NLog integration to route RavenDB logs anywhere",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Administration
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {administrationFeatures.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
