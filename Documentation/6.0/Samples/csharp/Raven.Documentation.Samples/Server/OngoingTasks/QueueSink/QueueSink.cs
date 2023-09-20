

using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.Queue;
using Raven.Client.Documents.Operations.QueueSink;
using static Raven.Client.Constants;

namespace Raven.Documentation.Samples.Server.OngoingTasks.ETL.QueueSink
{
    public class ConnectionStrings
    {
        private interface IFoo
        {
            #region QueueBrokerType
            public enum QueueBrokerType
            {
                None,
                Kafka,
                RabbitMq
            }
            #endregion

        }

        // Add Kafka Sink Task - a single region
        public void AddKafkaSinkTask()
        {
            using (var store = new DocumentStore())
            {
                // Create a Kafka Sink Task
                using (var session = store.OpenSession())
                {
                    #region add_kafka_sink-task
                    // Add Kafka connection string
                    var res = store.Maintenance.Send(
                        new PutConnectionStringOperation<QueueConnectionString>(
                            new QueueConnectionString
                            {
                                Name = "KafkaConStr",
                                BrokerType = QueueBrokerType.Kafka,
                                KafkaConnectionSettings = new KafkaConnectionSettings() 
                                        { BootstrapServers = "localhost:9092" }
                            }));

                    // Define a Sink script
                    QueueSinkScript queueSinkScript = new QueueSinkScript
                    {
                        // Script name
                        Name = "orders",
                        // Queues list
                        Queues = new List<string>() { "orders" },
                        // Apply this script
                        Script = @"this['@metadata']['@collection'] = 'Orders'; 
                                   put(this.Id, this)"
                    };

                    // Define a Kafka configuration
                    var config = new QueueSinkConfiguration()
                    {
                        // Sink name
                        Name = "KafkaSinkTaskName",
                        // The connection string to connect the broker with
                        ConnectionStringName = "KafkaConStr",
                        // What queue broker is this task using
                        BrokerType = QueueBrokerType.Kafka,
                        // The list of scripts to run
                        Scripts = { queueSinkScript }
                    };

                    AddQueueSinkOperationResult addQueueSinkOperationResult = 
                        store.Maintenance.Send(new AddQueueSinkOperation<QueueConnectionString>(config));
                    #endregion

                }
            }
        }

        // Add Kafka Sink Task - several regions
        public void AddKafkaSinkTaskParts()
        {
            using (var store = new DocumentStore())
            {
                // Create a Kafka Sink Task
                using (var session = store.OpenSession())
                {
                    #region add_kafka_connection-string
                    // Add Kafka connection string
                    var res = store.Maintenance.Send(
                        new PutConnectionStringOperation<QueueConnectionString>(
                            new QueueConnectionString
                            {
                                Name = "KafkaConStr",
                                BrokerType = QueueBrokerType.Kafka,
                                KafkaConnectionSettings = new KafkaConnectionSettings()
                                        { BootstrapServers = "localhost:9092" }
                            }));
                    #endregion

                    #region define-kafka-sink-script
                    // Define a Sink script
                    QueueSinkScript queueSinkScript = new QueueSinkScript
                    {
                        // Script name
                        Name = "orders",
                        // Queues list
                        Queues = new List<string>() { "orders" },
                        // Apply this script
                        Script = @"this['@metadata']['@collection'] = 'Orders'; 
                                   put(this.Id, this)"
                    };
                    #endregion

                    // Define a Kafka configuration
                    var config = new QueueSinkConfiguration()
                    {
                        // Sink name
                        Name = "KafkaSinkTaskName",
                        // The connection string to connect the broker with
                        ConnectionStringName = "KafkaConStr",
                        // What queue broker is this task using
                        BrokerType = QueueBrokerType.Kafka,
                        // The list of scripts to run
                        Scripts = { queueSinkScript }
                    };

                    AddQueueSinkOperationResult addQueueSinkOperationResult =
                        store.Maintenance.Send(new AddQueueSinkOperation<QueueConnectionString>(config));
                }
            }
        }


        // Creat a RabbitMq sink task - single region
        public void AddRabbitMqSinkTask()
        {
            using (var store = new DocumentStore())
            {
                // Create a RabbitMq Sink Task
                using (var session = store.OpenSession())
                {
                    #region add_RabbitMq_sink-task

                    // Add RabbitMq connection string
                    var res = store.Maintenance.Send(
                        new PutConnectionStringOperation<QueueConnectionString>(
                            new QueueConnectionString
                            {
                                Name = "RabbitMqConStr",
                                BrokerType = QueueBrokerType.RabbitMq,
                                RabbitMqConnectionSettings = new RabbitMqConnectionSettings()
                                        { ConnectionString = "amqp://guest:guest@localhost:5672/" }
                            }));

                    // Define a Sink script
                    QueueSinkScript queueSinkScript = new QueueSinkScript
                    {
                        // Script name
                        Name = "orders",
                        // Queues list
                        Queues = new List<string>() { "orders" },
                        // Apply this script
                        Script = @"this['@metadata']['@collection'] = 'Orders'; 
                                   put(this.Id, this)"
                    };

                    // Define a RabbitMq onfiguration
                    var config = new QueueSinkConfiguration()
                    {
                        // Sink name
                        Name = "RabbitMqSinkTaskName",
                        // The connection string to connect the broker with
                        ConnectionStringName = "RabbitMqConStr",
                        // What queue broker is this task using
                        BrokerType = QueueBrokerType.RabbitMq,
                        // The list of scripts to run
                        Scripts = { queueSinkScript }
                    };

                    AddQueueSinkOperationResult addQueueSinkOperationResult =
                        store.Maintenance.Send(new AddQueueSinkOperation<QueueConnectionString>(config));
                    #endregion

                }
            }
        }

        // Creat a RabbitMq sink task - several regions
        public void AddRabbitMqSinkTaskParts()
        {
            using (var store = new DocumentStore())
            {
                // Create a RabbitMq Sink Task
                using (var session = store.OpenSession())
                {
                    #region add_RabbitMq_connection-string
                    // Add RabbitMq connection string
                    var res = store.Maintenance.Send(
                        new PutConnectionStringOperation<QueueConnectionString>(
                            new QueueConnectionString
                            {
                                Name = "RabbitMqConStr",
                                BrokerType = QueueBrokerType.RabbitMq,
                                RabbitMqConnectionSettings = new RabbitMqConnectionSettings()
                                        { ConnectionString = "amqp://guest:guest@localhost:5672/" }
                            }));
                    #endregion

                    #region define-rabbitmq-sink-script
                    // Define a Sink script
                    QueueSinkScript queueSinkScript = new QueueSinkScript
                    {
                        // Script name
                        Name = "orders",
                        // Queues list
                        Queues = new List<string>() { "orders" },
                        // Apply this script
                        Script = @"this['@metadata']['@collection'] = 'Orders'; 
                                   put(this.Id, this)"
                    };
                    #endregion

                    #region define-rabbit-mq-configuration
                    // Define a RabbitMq onfiguration
                    var config = new QueueSinkConfiguration()
                    {
                        // Sink name
                        Name = "RabbitMqSinkTaskName",
                        // The connection string to connect the broker with
                        ConnectionStringName = "RabbitMqConStr",
                        // What queue broker is this task using
                        BrokerType = QueueBrokerType.RabbitMq,
                        // The list of scripts to run
                        Scripts = { queueSinkScript }
                    };

                    AddQueueSinkOperationResult addQueueSinkOperationResult =
                        store.Maintenance.Send(new AddQueueSinkOperation<QueueConnectionString>(config));
                    #endregion

                }
            }
        }
    }
}
