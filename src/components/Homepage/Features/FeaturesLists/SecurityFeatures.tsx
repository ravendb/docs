import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";

const SecurityFeaturesList = [
  {
    title: "Certificates",
    url: "/server/security/overview",
    description: "Short description",
  },
  {
    title: "Encryption",
    url: "/server/security/encryption/encryption-at-rest",
    description: "Short description",
  },
];

export default function SecurityFeaturesGrid() {
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
