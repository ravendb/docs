import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { Feature } from "@site/src/typescript/feature";

export default function OperationsFeaturesGrid() {
  const operationsFeatures: Feature[] = [
    {
      title: "Backup & restore",
      icon: "backup-history",
      url: "/cloud/cloud-backup-and-restore",
      description: "Keep your data safe, configure one-click restores",
    },
    {
      title: "Migration",
      icon: "import",
      url: "/cloud/cloud-migration",
      description:
        "Import live servers or export files into Cloud with a certificate swap",
    },
    {
      title: "Scaling",
      icon: "autoscaling",
      url: "/cloud/cloud-scaling",
      description:
        "Auto or manual scale-up/down without service interruption - add nodes, swap disks & more",
    },
    {
      title: "Security",
      icon: "lock",
      url: "/cloud/cloud-security",
      description: "TLS 1.2+, X.509 mutual auth, full at-rest encryption",
    },
    {
      title: "API",
      icon: "api-keys",
      url: "/cloud/cloud-api",
      description:
        "OpenAPI/Swagger endpoints plus SDKs for complete automation",
    },
    {
      title: "Product features",
      icon: "features",
      url: "/cloud/cloud-features",
      description: "Enable RavenDB capabilities on demand",
    },
    {
      title: "Settings",
      icon: "settings",
      url: "/cloud/cloud-settings",
      description: "Fine-tune your Cloud instance",
    },
    {
      title: "Support",
      icon: "support",
      url: "/cloud/cloud-support",
      description: "Pick your plan & learn about support form",
    },
    {
      title: "Maintenance & Troubleshooting",
      icon: "studio-config",
      url: "/cloud/cloud-maintenance-troubleshooting",
      description:
        "Advanced monitoring system tracking critical performance metrics",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Operations
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {operationsFeatures.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
