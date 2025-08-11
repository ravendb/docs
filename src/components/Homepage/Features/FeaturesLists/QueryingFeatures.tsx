import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function QueryingFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const queryingFeatures: Feature[] = [
    {
      title: "Raven Query Language",
      icon: "rql",
      url: `/${activeVersion.label}/client-api/session/querying/what-is-rql`,
      description: "Simple yet powerful SQL-style queries",
    },
    {
      title: "Full-text search",
      icon: "full-text-search",
      url: `/${activeVersion.label}/client-api/session/querying/text-search/full-text-search`,
      description: "Cutting‑edge integrated search engine",
    },
    {
      title: "Patching",
      icon: "patch",
      url: `/${activeVersion.label}/client-api/operations/patching/single-document`,
      description: "Transform documents at scale with a script",
    },
    {
      title: "Facets",
      icon: "facets",
      url: `/${activeVersion.label}/indexes/querying/faceted-search`,
      description: "Slice and navigate through a large dataset",
    },
    {
      title: "MoreLikeThis",
      icon: "morelikethis",
      url: `/${activeVersion.label}/indexes/querying/morelikethis`,
      description: "Get similar documents based on content",
    },
    {
      title: "Spatial",
      icon: "global",
      url: `/${activeVersion.label}/indexes/querying/spatial`,
      description: "Search and sort by geographic location effortlessly",
      minimumSupportedVersion: "5.2"
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Querying
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {queryingFeatures
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
