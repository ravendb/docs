import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

export default function IntegrationFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const IntegrationFeaturesList = [
    {
      title: "RavenDB ETL",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/basics`,
      description: "Short description",
    },
    {
      title: "OLAP ETL",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/olap`,
      description: "Short description",
    },
    {
      title: "Elasticsearch ETL",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/elasticsearch`,
      description: "Short description",
    },
    {
      title: "SQL ETL",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/sql`,
      description: "Short description",
    },
    {
      title: "Kafka ETL",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/queue-etl/kafka`,
      description: "Short description",
    },
    {
      title: "RabbitMQ ETL",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/queue-etl/rabbit-mq`,
      description: "Short description",
    },
    {
      title: "Snowflake ETL",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/snowflake`,
      description: "Short description",
    },
    {
      title: "Amazon SQS ETL",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/queue-etl/amazon-sqs`,
      description: "Short description",
    },
    {
      title: "Azure Queue Storage ETL",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/queue-etl/azure-queue`,
      description: "Short description",
    },
    {
      title: "RabbitMQ Sink",
      url: `/${activeVersion.label}/server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink`,
      description: "Short description",
    },
    {
      title: "Kafka Sink",
      url: `/${activeVersion.label}/server/ongoing-tasks/queue-sink/kafka-queue-sink`,
      description: "Short description",
    },
    {
      title: "PowerBI",
      url: `/${activeVersion.label}/integrations/postgresql-protocol/power-bi`,
      description: "Short description",
    },
    {
      title: "Akka.NET",
      url: `/${activeVersion.label}/integrations/akka.net-persistence/integrating-with-akka-persistence`,
      description: "Short description",
    },
    {
      title: "Grafana",
      url: `/${activeVersion.label}/server/troubleshooting/logging`,
      description: "Short description",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Integration
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {IntegrationFeaturesList.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
