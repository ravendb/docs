import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import Heading from "@theme/Heading";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function AiFeaturesGrid() {
  const pluginId = "default";
  const minimumCategorySupportedVersion = "7.0";
  const { activeVersion } = useActiveDocContext(pluginId);

  if (minimumCategorySupportedVersion > activeVersion.label) {
    return null;
  }

  const aiFeatures: Feature[] = [
    {
      title: "AI Agents",
      icon: "ai-agents",
      url: `/${activeVersion.label}/ai-integration/ai-agents/ai-agents-api`,
      description: "Database-native agents that query & act safely",
      minimumSupportedVersion: "7.1"
    },
    {
      title: "Vector search",
      icon: "vector-search",
      url: `/${activeVersion.label}/ai-integration/vector-search/ravendb-as-vector-database`,
      description: "Find contextually relevant data",
      minimumSupportedVersion: "7.0"
    },
    {
      title: "GenAI",
      icon: "genai",
      url: `/${activeVersion.label}/ai-integration/gen-ai-integration/gen-ai-overview`,
      description: "Empower your application using intelligent task",
      minimumSupportedVersion: "7.1"
    },
    {
      title: "Embeddings generation",
      icon: "ai-etl",
      url: `/${activeVersion.label}/ai-integration/generating-embeddings/overview`,
      description: "Automatically turn your data into AI-ready vectors",
      minimumSupportedVersion: "7.0"
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        AI
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {aiFeatures
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
