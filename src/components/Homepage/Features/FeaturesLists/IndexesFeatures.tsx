import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function IndexesFeaturesGrid() {
    const pluginId = "default";
    const minimumCategorySupportedVersion = "3.0";
    const { activeVersion } = useActiveDocContext(pluginId);

    if (minimumCategorySupportedVersion > activeVersion.label) {
        return null;
    }

    const indexesFeatures: Feature[] = [
        {
            title: "Static indexes",
            icon: "index",
            url: `/${activeVersion.label}/indexes/creating-and-deploying`,
            description: "Complete control over precomputing & performance",
            minimumSupportedVersion: "3.0",
        },
        {
            title: "Auto indexes",
            icon: "auto-indexes",
            url: `/${activeVersion.label}/indexes/creating-and-deploying#auto-indexes`,
            description: "Self‑optimizing database with zero manual effort",
            minimumSupportedVersion: "3.0",
        },
    ];

    return (
        <>
            <Heading as="h3" className="!mb-2">
                Indexes
            </Heading>
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
                {indexesFeatures
                    .filter(
                        (feature) =>
                            !feature.minimumSupportedVersion || feature.minimumSupportedVersion <= activeVersion.label
                    )
                    .map((props, idx) => (
                        <FeatureItem key={idx} {...props} />
                    ))}
            </div>
        </>
    );
}
