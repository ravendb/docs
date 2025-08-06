import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { Feature } from "@site/src/typescript/feature";

export default function CoreServiceFeaturesGrid() {
  const coreServiceFeatures: Feature[] = [
    {
      title: "Tiers and Instances",
      icon: "dbgroup",
      url: "/cloud/cloud-instances",
      description:
        "Pick Free, Development, or Production-graded products, with premium or standard storage options",
    },
    {
      title: "Pricing, Payment and Billing",
      icon: "price-tag",
      url: "/cloud/cloud-pricing-payment-billing",
      description:
        "Learn more about on-demand & yearly pricing, billing, and payment configuration",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Core service
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {coreServiceFeatures.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
