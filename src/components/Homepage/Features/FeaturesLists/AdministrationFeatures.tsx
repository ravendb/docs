import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";

const AdministrationFeaturesList = [
  {
    title: "Studio",
    url: "/studio/overview",
    description: "Short description",
  },
  {
    title: "RavenCLI",
    url: "/server/administration/cli",
    description: "Short description",
  },
  {
    title: "NLog",
    url: "/server/troubleshooting/logging#configuring-and-using-nlog",
    description: "Short description",
  },
];

export default function AdministrationFeaturesGrid() {
  return (
    <>
      <Heading as="h4" className="!mb-2">
        Administration
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {AdministrationFeaturesList.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
