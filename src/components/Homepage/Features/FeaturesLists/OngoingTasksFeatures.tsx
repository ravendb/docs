import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function OngoingTasksFeaturesGrid() {
    const pluginId = "default";
    const minimumCategorySupportedVersion = "3.0";
    const { activeVersion } = useActiveDocContext(pluginId);

    if (minimumCategorySupportedVersion > activeVersion.label) {
        return null;
    }

    const ongoingTasksFeatures: Feature[] = [
        {
            title: "Replication",
            icon: "external-replication",
            url: `/${activeVersion.label}/server/clustering/replication/replication-overview`,
            description: "Keeps your clusters in sync for high availability",
            minimumSupportedVersion: "4.0",
        },
        {
            title: "Subscriptions",
            icon: "subscriptions",
            url: `/${activeVersion.label}/client-api/data-subscriptions/what-are-data-subscriptions`,
            description: "Subscribe to defined documents, trigger your worker routines on field updates",
            minimumSupportedVersion: "3.0",
        },
        {
            title: "Periodic backups",
            icon: "periodic-backup",
            url: `/${activeVersion.label}/server/ongoing-tasks/backup-overview`,
            description: "Scheduled full & incremental backups",
            minimumSupportedVersion: "4.0",
        },
    ];

    return (
        <>
            <Heading as="h3" className="!mb-2">
                Ongoing tasks
            </Heading>
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
                {ongoingTasksFeatures
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
