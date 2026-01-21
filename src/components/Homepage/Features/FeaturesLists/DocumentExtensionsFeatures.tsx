import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function DocumentExtensionsFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const documentExtensionsFeatures: Feature[] = [
    {
      title: "Revisions",
      icon: "revisions",
      url: `/${activeVersion.label}/document-extensions/revisions/overview`,
      description: "Capture, track, and rewind any change",
    },
    {
      title: "Time series",
      icon: "timeseries",
      url: `/${activeVersion.label}/document-extensions/timeseries/overview`,
      description: "Store, query and aggregate timestamped data natively",
      minimumSupportedVersion: "5.0"
    },
    {
      title: "Attachments",
      icon: "attachment",
      url: `/${activeVersion.label}/document-extensions/attachments/overview`,
      description:
        "Attach binary files directly to documents - scalable, searchable",
    },
    {
      title: "Documents compression",
      icon: "documents-compression",
      url: `/${activeVersion.label}/server/storage/documents-compression`,
      description:
        "Automatically compress document content for storage savings",
      minimumSupportedVersion: "5.0"
    },
    {
      title: "Counters",
      icon: "new-counter",
      url: `/${activeVersion.label}/document-extensions/counters/overview`,
      description: "Simple, scalable, and conflict-free numeric counters",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Document extensions
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {documentExtensionsFeatures
            .filter(
                feature =>
                    !feature.minimumSupportedVersion ||
                    feature.minimumSupportedVersion <= activeVersion.label
            )
            .map((props, idx) => (
                <FeatureItem key={idx} {...props} />
            ))}
      </div>
    </>
  );
}
