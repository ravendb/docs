import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function DocumentExtensionsFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const DocumentExtensionsFeaturesList = [
    {
      title: "Revisions",
      url: `/${activeVersion.label}/document-extensions/revisions/overview`,
      description: "Short description",
    },
    {
      title: "Time series",
      url: `/${activeVersion.label}/document-extensions/timeseries/overview`,
      description: "Short description",
    },
    {
      title: "Attachments",
      url: `/${activeVersion.label}/document-extensions/attachments/what-are-attachments`,
      description: "Short description",
    },
    {
      title: "Documents compression",
      url: `/${activeVersion.label}/server/storage/documents-compression`,
      description: "Short description",
    },
    {
      title: "Counters",
      url: `${activeVersion.label}/document-extensions/counters/overview`,
      description: "Short description",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Document extensions
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {DocumentExtensionsFeaturesList.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
