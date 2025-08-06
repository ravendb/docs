import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function SecurityFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const securityFeatures: Feature[] = [
    {
      title: "Certificates",
      icon: "certificate",
      url: `/${activeVersion.label}/server/security/overview`,
      description:
        "Secure your server with X.509 certificates and fine-grained access control",
    },
    {
      title: "Encryption",
      icon: "encryption",
      url: `/${activeVersion.label}/server/security/encryption/encryption-at-rest`,
      description:
        "Transparent at-rest & in-transit, data security without code changes",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Security
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {securityFeatures.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
