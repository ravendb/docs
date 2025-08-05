import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";
import { Feature } from "@site/src/typescript/feature";

export default function IntegrationFeaturesGrid() {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  const IntegrationFeatures: Feature[] = [
    {
      title: "RavenDB ETL",
      icon: "ravendb-etl",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/basics`,
      description: "Move and transform data between RavenDB databases",
    },
    {
      title: "OLAP ETL",
      icon: "olap-etl",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/olap`,
      description: "Short description",
    },
    {
      title: "Elasticsearch ETL",
      icon: "elastic-search-etl",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/elasticsearch`,
      description: "Stream RavenDB data into Elasticsearch indices",
    },
    {
      title: "SQL ETL",
      icon: "sql-etl",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/sql`,
      description: "Send documents to SQL databases with schema transformation",
    },
    {
      title: "Kafka ETL",
      icon: "kafka-etl",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/queue-etl/kafka`,
      description:
        "Publish documents into Kafka topics for real-time event pipelines",
    },
    {
      title: "RabbitMQ ETL",
      icon: "rabbitmq-etl",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/queue-etl/rabbit-mq`,
      description:
        "Send transformed RavenDB data directly to RabbitMQ exchanges",
    },
    {
      title: "Snowflake ETL",
      icon: "snowflake-etl",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/snowflake`,
      description:
        "Load and transform data directly into Snowflake for analytics",
    },
    {
      title: "Amazon SQS ETL",
      icon: "amazon-sqs-etl",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/queue-etl/amazon-sqs`,
      description: "Send CloudEvents messages to Amazon SQS",
    },
    {
      title: "Azure Queue Storage ETL",
      icon: "azure-queue-storage-etl",
      url: `/${activeVersion.label}/server/ongoing-tasks/etl/queue-etl/azure-queue`,
      description: "Send CloudEvents messages to Azure Queue Storage",
    },
    {
      title: "RabbitMQ Sink",
      icon: "rabbitmq-sink",
      url: `/${activeVersion.label}/server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink`,
      description:
        "Ingest messages from RabbitMQ directly into RavenDB documents",
    },
    {
      title: "Kafka Sink",
      icon: "kafka-sink",
      url: `/${activeVersion.label}/server/ongoing-tasks/queue-sink/kafka-queue-sink`,
      description:
        "Receive data into RavenDB from Kafka streams as source event",
    },
    {
      title: "PowerBI",
      icon: "powerbi",
      url: `/${activeVersion.label}/integrations/postgresql-protocol/power-bi`,
      description:
        "Connect RavenDB to Power BI for live business intelligence reporting",
    },
    {
      title: "Akka.NET",
      icon: "akka-net",
      url: `/${activeVersion.label}/integrations/akka.net-persistence/integrating-with-akka-persistence`,
      description: "Use RavenDB as Akka.Persistence storage.",
    },
    {
      title: "Grafana",
      icon: "grafana",
      url: `/${activeVersion.label}/server/troubleshooting/logging`,
      description: "Visualize RavenDB metrics in real-time Grafana dashboards",
    },
  ];

  return (
    <>
      <Heading as="h4" className="!mb-2">
        Integration
      </Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        {IntegrationFeatures.map((props, idx) => (
          <FeatureItem key={idx} {...props} />
        ))}
      </div>
    </>
  );
}
