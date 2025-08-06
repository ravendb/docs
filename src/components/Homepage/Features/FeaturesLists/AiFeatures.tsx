import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import Heading from "@theme/Heading";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function AiFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const aiFeatures: Feature[] = [
    {
      title: "Vector search",
      icon: "vector-search",
      url: `/${activeVersion.label}/ai-integration/vector-search/ravendb-as-vector-database`,
      description: "Find contextually relevant data",
    },
    {
      title: "GenAI",
      icon: "genai",
      url: `/${activeVersion.label}/ai-integration/gen-ai-integration/gen-ai-overview`,
      description: "Empower your application using intelligent task",
    },
    {
      title: "Embeddings generation",
      icon: "ai-etl",
      url: `/${activeVersion.label}/ai-integration/generating-embeddings/overview`,
      description: "Automatically turn your data into AI-ready vectors",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        AI
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {aiFeatures.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
