import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function OngoingTasksFeaturesGrid() {
    const pluginId = "default";
    const { activeVersion } = useActiveDocContext(pluginId);

    const ongoingTasksFeatures: Feature[] = [
        {
            title: "Replication",
            icon: "external-replication",
            url: `/${activeVersion.label}/server/clustering/replication/replication-overview`,
            description: "Keeps your clusters in sync for high availability",
        },
        {
            title: "Subscriptions",
            icon: "subscriptions",
            url: `/${activeVersion.label}/client-api/data-subscriptions/what-are-data-subscriptions`,
            description: "Subscribe to defined documents, trigger your worker routines on field updates",
        },
        {
            title: "Periodic backups",
            icon: "periodic-backup",
            url: `/${activeVersion.label}/server/ongoing-tasks/backup-overview`,
            description: "Scheduled full & incremental backups",
        },
    ];

    return (
        <>
            <Heading as="h4" className="!mb-2">
                Ongoing tasks
            </Heading>
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
                {ongoingTasksFeatures.map((props, idx) => (
                    <FeatureItem key={idx} {...props} />
                ))}
            </div>
        </>
    );
}
