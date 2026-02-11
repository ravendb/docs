import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function BackgroundTasksFeaturesGrid() {
    const pluginId = "default";
    const { activeVersion } = useActiveDocContext(pluginId);

    const backgroundTasksFeatures: Feature[] = [
        {
            title: "Expiration",
            icon: "document-expiration",
            url: `/${activeVersion.label}/server/extensions/expiration`,
            description: "Automatically scheduled documents cleanup",
        },
        {
            title: "Refresh",
            icon: "document-refresh",
            url: `/${activeVersion.label}/server/extensions/refresh`,
            description: "Automatically re-trigger your documents",
        },
        {
            title: "Archival",
            icon: "data-archival",
            url: `/${activeVersion.label}/data-archival/overview`,
            description: "Retain old documents access while boosting performance",
            minimumSupportedVersion: "6.0",
        },
    ];

    return (
        <>
            <Heading as="h4" className="!mb-2">
                Background tasks
            </Heading>
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
                {backgroundTasksFeatures
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
