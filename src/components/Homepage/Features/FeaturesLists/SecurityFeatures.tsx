import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function SecurityFeaturesGrid() {
    const pluginId = "default";
    const { activeVersion } = useActiveDocContext(pluginId);

    const SecurityFeaturesList = [
        {
            title: "Certificates",
            url: `${activeVersion.label}/server/security/overview`,
            description: "Short description",
        },
        {
            title: "Encryption",
            url: `${activeVersion.label}/server/security/encryption/encryption-at-rest`,
            description: "Short description",
        },
    ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Security
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {SecurityFeaturesList.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
