import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function SecurityFeaturesGrid() {
  const pluginId = "default";
  const minimumCategorySupportedVersion = "4.0";
  const { activeVersion } = useActiveDocContext(pluginId);

  if (minimumCategorySupportedVersion > activeVersion.label) {
    return null;
  }

  const securityFeatures: Feature[] = [
    {
      title: "Certificates",
      icon: "certificate",
      url: `/${activeVersion.label}/server/security/overview`,
      description:
        "Secure your server with X.509 certificates and fine-grained access control",
      minimumSupportedVersion: "4.0"
    },
    {
      title: "Encryption",
      icon: "encryption",
      url: `/${activeVersion.label}/server/security/encryption/encryption-at-rest`,
      description:
        "Transparent at-rest & in-transit, data security without code changes",
      minimumSupportedVersion: "4.0"
    },
    {
      title: "Audit Log",
      icon: "audit-logs",
      url: `/${activeVersion.label}/server/security/audit-log/audit-log-overview`,
      description:
        "Record who connected and what they did - built-in audit trail.",
      minimumSupportedVersion: "5.4"
    }
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Security
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {securityFeatures
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
