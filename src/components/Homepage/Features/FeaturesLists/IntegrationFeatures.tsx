import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";

const IntegrationFeaturesList = [
  {
    title: "RavenDB ETL",
    url: "/server/ongoing-tasks/etl/basics",
    description: "Short description",
  },
  {
    title: "OLAP ETL",
    url: "/server/ongoing-tasks/etl/olap",
    description: "Short description",
  },
  {
    title: "Elasticsearch ETL",
    url: "/server/ongoing-tasks/etl/elasticsearch",
    description: "Short description",
  },
  {
    title: "SQL ETL",
    url: "/server/ongoing-tasks/etl/sql",
    description: "Short description",
  },
  {
    title: "Kafka ETL",
    url: "/server/ongoing-tasks/etl/queue-etl/kafka",
    description: "Short description",
  },
  {
    title: "RabbitMQ ETL",
    url: "/server/ongoing-tasks/etl/queue-etl/rabbit-mq",
    description: "Short description",
  },
  {
    title: "Snowflake ETL",
    url: "/server/ongoing-tasks/etl/snowflake",
    description: "Short description",
  },
  {
    title: "Amazon SQS ETL",
    url: "/server/ongoing-tasks/etl/queue-etl/amazon-sqs",
    description: "Short description",
  },
  {
    title: "Azure Queue Storage ETL",
    url: "/server/ongoing-tasks/etl/queue-etl/azure-queue",
    description: "Short description",
  },
  {
    title: "RabbitMQ Sink",
    url: "/server/ongoing-tasks/queue-sink/rabbit-mq-queue-sink",
    description: "Short description",
  },
  {
    title: "Kafka Sink",
    url: "/server/ongoing-tasks/queue-sink/kafka-queue-sink",
    description: "Short description",
  },
  {
    title: "PowerBI",
    url: "/integrations/postgresql-protocol/power-bi",
    description: "Short description",
  },
  {
    title: "Akka.NET",
    url: "/integrations/akka.net-persistence/integrating-with-akka-persistence",
    description: "Short description",
  },
  {
    title: "Grafana",
    url: "/server/troubleshooting/logging",
    description: "Short description",
  },
];

export default function IntegrationFeaturesGrid() {
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
