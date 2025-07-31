import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";

const DocumentExtensionsFeaturesList = [
  {
    title: "Revisions",
    url: "/document-extensions/revisions/overview",
    description: "Short description",
  },
  {
    title: "Time series",
    url: "/document-extensions/timeseries/overview",
    description: "Short description",
  },
  {
    title: "Attachments",
    url: "/document-extensions/attachments/what-are-attachments",
    description: "Short description",
  },
  {
    title: "Documents compression",
    url: "/server/storage/documents-compression",
    description: "Short description",
  },
  {
    title: "Counters",
    url: "/document-extensions/counters/overview",
    description: "Short description",
  },
];

export default function DocumentExtensionsFeaturesGrid() {
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
