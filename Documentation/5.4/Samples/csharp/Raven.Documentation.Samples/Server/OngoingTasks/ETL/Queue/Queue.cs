using System;
using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Backups;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.ElasticSearch;
using Raven.Client.Documents.Operations.ETL.OLAP;
using Raven.Client.Documents.Operations.ETL.Queue;
using Raven.Client.Documents.Operations.ETL.SQL;
namespace Raven.Documentation.Samples.Server.OngoingTasks.ETL.Queue
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

        public ConnectionStrings()
        {
            using (var store = new DocumentStore())
            {
                #region add_rabbitMQ_connection-string
                var res = store.Maintenance.Send(
                    new PutConnectionStringOperation<QueueConnectionString>(
                        new QueueConnectionString
                        {
                            Name = "RabbitMqConStr",
                            BrokerType = QueueBrokerType.RabbitMq,
                            RabbitMqConnectionSettings = new RabbitMqConnectionSettings() 
                                { ConnectionString = "amqp://guest:guest@localhost:49154" }
                        }));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region add_kafka_connection-string
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
            }
        }

        // Add Kafka ETL Task
        public void AddKafkaEtlTask()
        {
            using (var store = new DocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    #region add_kafka_etl-task
                    // use PutConnectionStringOperation to add connection string
                    var res = store.Maintenance.Send(
                        new PutConnectionStringOperation<QueueConnectionString>(
                            new QueueConnectionString
                            {
                                Name = "KafkaConStr",
                                BrokerType = QueueBrokerType.Kafka,
                                KafkaConnectionSettings = new KafkaConnectionSettings() { BootstrapServers = "localhost:9092" }
                            }));

                    // create transformation script
                    Transformation transformation = new Transformation
                    {
                        Name = "scriptName",
                        Collections = { "Orders" },
                        Script = @"var orderData = {
                                    Id: id(this),
                                    OrderLinesCount: this.Lines.length,
                                    TotalCost: 0
                                };

                                for (var i = 0; i < this.Lines.length; i++) {
                                    var line = this.Lines[i];
                                    var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
                                    orderData.TotalCost += cost;
                                }

                                loadToOrders(orderData, {
                                    Id: id(this),
                                    PartitionKey: id(this),
                                    Type: 'special-promotion',
                                    Source: '/promotion-campaigns/summer-sale'
                                });",
                        ApplyToAllDocuments = false
                    };

                    // use AddEtlOperation to add ETL task 
                    AddEtlOperation<QueueConnectionString> operation = new AddEtlOperation<QueueConnectionString>(
                    new QueueEtlConfiguration()
                    {
                        Name = "KafkaEtlTaskName",
                        ConnectionStringName = "KafkaConStr",
                        Transforms =
                            {
                                transformation
                            },
                        Queues = { new EtlQueue() { Name = "Orders" } },
                        BrokerType = QueueBrokerType.Kafka,

                        // Do not prevent a failover to another node
                        PinToMentorNode = false
                    });
                    store.Maintenance.Send(operation);
                    
                    #endregion
                }
            }
        }


        // Add Kafka ETL Task (delete processed docs from RavenDB)
        public void AddKafkaEtlTaskDeleteProcessedDocuments()
        {
            using (var store = new DocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    // use PutConnectionStringOperation to add connection string
                    var res = store.Maintenance.Send(
                        new PutConnectionStringOperation<QueueConnectionString>(
                            new QueueConnectionString
                            {
                                Name = "KafkaConStr",
                                BrokerType = QueueBrokerType.Kafka,
                                KafkaConnectionSettings = new KafkaConnectionSettings() { BootstrapServers = "localhost:9092" }
                            }));

                    // create transformation script
                    Transformation transformation = new Transformation
                    {
                        Name = "scriptName",
                        Collections = { "Orders" },
                        Script = @"var orderData = {
                                    Id: id(this),
                                    OrderLinesCount: this.Lines.length,
                                    TotalCost: 0
                                };

                                for (var i = 0; i < this.Lines.length; i++) {
                                    var line = this.Lines[i];
                                    var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
                                    orderData.TotalCost += cost;
                                }

                                loadToOrders(orderData, {
                                    Id: id(this),
                                    PartitionKey: id(this),
                                    Type: 'special-promotion',
                                    Source: '/promotion-campaigns/summer-sale'
                                });",
                        ApplyToAllDocuments = false
                    };

                    // use AddEtlOperation to add ETL task 
                    AddEtlOperation<QueueConnectionString> operation = new AddEtlOperation<QueueConnectionString>(
                    #region kafka_EtlQueue
                    new QueueEtlConfiguration()
                    {
                        Name = "KafkaEtlTaskName",
                        ConnectionStringName = "KafkaConStr",
                        Transforms =
                            {
                                transformation
                            },
                        // Only define if you want to delete documents from RavenDB after they are processed.
                        Queues =  
                            new List<EtlQueue>() 
                            {
                                new()
                                {
                                    // Documents that were processed by the transformation script will be
                                    // deleted from RavenDB after the message is loaded to the Orders queue.
                                    Name = "Orders",
                                    DeleteProcessedDocuments = true
                                }
                            },
                        BrokerType = QueueBrokerType.Kafka
                    });
                     #endregion
                    store.Maintenance.Send(operation);

                }
            }
        }

        // Add RabbitMq ETL task
        public void AddRabbitmqEtlTask()
        {
            using (var store = new DocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    #region add_rabbitmq_etl-task
                    // use PutConnectionStringOperation to add connection string
                    var res = store.Maintenance.Send(
                        new PutConnectionStringOperation<QueueConnectionString>(
                            new QueueConnectionString
                            {
                                Name = "RabbitMqConStr",
                                BrokerType = QueueBrokerType.RabbitMq,
                                RabbitMqConnectionSettings = new RabbitMqConnectionSettings() { ConnectionString = "amqp://guest:guest@localhost:49154" }
                            }));

                    // create transformation script
                    Transformation transformation = new Transformation
                    {
                        Name = "scriptName",
                        Collections = { "Orders" },
                        Script = @"var orderData = {
                                    Id: id(this), 
                                    OrderLinesCount: this.Lines.length,
                                    TotalCost: 0
                                };

                                for (var i = 0; i < this.Lines.length; i++) {
                                    var line = this.Lines[i];
                                    var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
                                    orderData.TotalCost += cost;
                                }

                                loadToOrders(orderData, `routingKey`, {  
                                    Id: id(this),
                                    Type: 'special-promotion',
                                    Source: '/promotion-campaigns/summer-sale'
                                });",
                        ApplyToAllDocuments = false
                    };

                    // use AddEtlOperation to add ETL task 
                    AddEtlOperation<QueueConnectionString> operation = new AddEtlOperation<QueueConnectionString>(
                    new QueueEtlConfiguration()
                    {
                        Name = "RabbitMqEtlTaskName",
                        ConnectionStringName = "RabbitMqConStr",
                        Transforms =
                            {
                                transformation
                            },
                        Queues = { new EtlQueue() { Name = "Orders" } },
                        BrokerType = QueueBrokerType.RabbitMq,
                        SkipAutomaticQueueDeclaration = false,

                        // Do not prevent a failover to another node
                        PinToMentorNode = false
                    });
                    store.Maintenance.Send(operation);

                    #endregion
                }
            }
        }


        // Add RabbitMQ ETL Task (delete processed docs from RavenDB)
        public void AddRabbitmqEtlTaskDeleteProcessedDocuments()
        {
            using (var store = new DocumentStore())
            {
                // Create a document
                using (var session = store.OpenSession())
                {
                    // use PutConnectionStringOperation to add connection string
                    var res = store.Maintenance.Send(
                        new PutConnectionStringOperation<QueueConnectionString>(
                            new QueueConnectionString
                            {
                                Name = "RabbitMqConStr",
                                BrokerType = QueueBrokerType.RabbitMq,
                                RabbitMqConnectionSettings = new RabbitMqConnectionSettings() { ConnectionString = "amqp://guest:guest@localhost:49154" }
                            }));

                    // create transformation script
                    Transformation transformation = new Transformation
                    {
                        Name = "scriptName",
                        Collections = { "Orders" },
                        Script = @"var orderData = {
                                    Id: id(this), 
                                    OrderLinesCount: this.Lines.length,
                                    TotalCost: 0
                                };

                                for (var i = 0; i < this.Lines.length; i++) {
                                    var line = this.Lines[i];
                                    var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
                                    orderData.TotalCost += cost;
                                }

                                loadToOrders(orderData, `routingKey`, {  
                                    Id: id(this),
                                    Type: 'special-promotion',
                                    Source: '/promotion-campaigns/summer-sale'
                                });",
                        ApplyToAllDocuments = false
                    };

                    // use AddEtlOperation to add ETL task 
                    AddEtlOperation<QueueConnectionString> operation = new AddEtlOperation<QueueConnectionString>(
                    #region rabbitmq_EtlQueue
                    new QueueEtlConfiguration()
                    {
                        Name = "RabbitMqEtlTaskName",
                        ConnectionStringName = "RabbitMqConStr",
                        Transforms =
                            {
                                transformation
                            },
                        // Only define if you want to delete documents from RavenDB after they are processed.
                        Queues =
                            new List<EtlQueue>()
                            {
                                new()
                                {
                                    // Documents that were processed by the transformation script will be
                                    // deleted from RavenDB after the message is loaded to the Orders queue.
                                    Name = "Orders",
                                    DeleteProcessedDocuments = true
                                }
                            },
                        BrokerType = QueueBrokerType.RabbitMq,
                        SkipAutomaticQueueDeclaration = false
                    });
                    #endregion
                    store.Maintenance.Send(operation);
                }
            }
        }


        private class Definition
        {
            #region QueueConnectionString
            public class QueueConnectionString : ConnectionString
            {
                public QueueBrokerType BrokerType { get; set; }
                // Configure if a Kafka connection string is needed
                public KafkaConnectionSettings KafkaConnectionSettings { get; set; }
                // Configure if a RabbitMq connection string is needed
                public RabbitMqConnectionSettings RabbitMqConnectionSettings { get; set; }
            }
            #endregion

            public  abstract class ConnectionString
            {
                public string Name { get; set; }
            }

            #region EtlQueueDefinition
            public class EtlQueue
            {
                public string Name { get; set; }
                public bool DeleteProcessedDocuments { get; set; }
            }
            #endregion

            #region CloudEventAttributes
            public class CloudEventAttributes
            {
                public string Id { get; set; }
                public string Type { get; set; }
                public string Source { get; set; }
                public string PartitionKey { get; set; }
            }
            #endregion
        }
    }
}
