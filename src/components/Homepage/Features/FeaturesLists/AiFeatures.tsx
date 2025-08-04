import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import Heading from "@theme/Heading";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function AiFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const AiFeaturesList = [
    {
      title: "Vector search",
      url: `/${activeVersion.label}/ai-integration/vector-search/ravendb-as-vector-database`,
      description: "Short description",
    },
    {
      title: "GenAI",
      url: `/${activeVersion.label}/ai-integration/gen-ai-integration/gen-ai-overview`,
      description: "Short description",
    },
    {
      title: "Embeddings generation",
      url: `/${activeVersion.label}/ai-integration/generating-embeddings/overview`,
      description: "Short description",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        AI
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {AiFeaturesList.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
