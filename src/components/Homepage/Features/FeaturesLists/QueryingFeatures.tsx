import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";

const QueryingFeaturesList = [
  {
    title: "Raven Query Language",
    url: "/client-api/session/querying/what-is-rql",
    description: "Short description",
  },
  {
    title: "Full-text search",
    url: "/client-api/session/querying/text-search/full-text-search",
    description: "Short description",
  },
  {
    title: "Patching",
    url: "/client-api/operations/patching/single-document",
    description: "Short description",
  },
  {
    title: "Facets",
    url: "/indexes/querying/faceted-search",
    description: "Short description",
  },
  {
    title: "MoreLikeThis",
    url: "/indexes/querying/morelikethis",
    description: "Short description",
  },
  {
    title: "Spatial",
    url: "/indexes/querying/spatial",
    description: "Short description",
  },
];

export default function QueryingFeaturesGrid() {
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
