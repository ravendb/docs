import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function QueryingFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const QueryingFeaturesList = [
    {
      title: "Raven Query Language",
      url: `${activeVersion.label}/client-api/session/querying/what-is-rql`,
      description: "Short description",
    },
    {
      title: "Full-text search",
      url: `${activeVersion.label}/client-api/session/querying/text-search/full-text-search`,
      description: "Short description",
    },
    {
      title: "Patching",
      url: `${activeVersion.label}/client-api/operations/patching/single-document`,
      description: "Short description",
    },
    {
      title: "Facets",
      url: `${activeVersion.label}/indexes/querying/faceted-search`,
      description: "Short description",
    },
    {
      title: "MoreLikeThis",
      url: `${activeVersion.label}/indexes/querying/morelikethis`,
      description: "Short description",
    },
    {
      title: "Spatial",
      url: `${activeVersion.label}/indexes/querying/spatial`,
      description: "Short description",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Querying
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {QueryingFeaturesList.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
