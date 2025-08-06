import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { Feature } from "@site/src/typescript/feature";

export default function MarketplaceEcosystemFeaturesGrid() {
  const marketplaceEcosystemFeatures: Feature[] = [
    {
      title: "Microsoft Azure Marketplace",
      icon: "azure",
      url: "/cloud/cloud-microsoft-azure-marketplace",
      description: "Subscribe via Azure billing",
    },
    {
      title: "AWS Marketplace",
      icon: "aws",
      url: "/cloud/cloud-aws-marketplace",
      description: "Subscribe via AWS billing",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Marketplace & ecosystem
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {marketplaceEcosystemFeatures.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
