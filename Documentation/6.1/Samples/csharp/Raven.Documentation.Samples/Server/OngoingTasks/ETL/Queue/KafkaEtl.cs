using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.Queue;
namespace Raven.Documentation.Samples.Server.OngoingTasks.ETL.Queue
{
    public class KafkaEtl
    {
        public void AddConnectionString()
        {
            using (var store = new DocumentStore())
            {
                #region add_kafka_connection_string
                // Prepare the connection string:
                // ==============================
                var conStr = new QueueConnectionString
                {
                    // Provide a name for this connection string
                    Name = "myKafkaConStr",
                    
                    // Set the broker type
                    BrokerType = QueueBrokerType.Kafka,
                    
                    // Configure the connection details
                    KafkaConnectionSettings = new KafkaConnectionSettings() 
                        { BootstrapServers = "localhost:9092" }
                };
                
                // Deploy (send) the connection string to the server via the PutConnectionStringOperation:
                // =======================================================================================
                var res = store.Maintenance.Send(
                    new PutConnectionStringOperation<QueueConnectionString>(conStr));
                #endregion
            }
        }

        public void AddKafkaEtlTask()
        {
            using (var store = new DocumentStore())
            {
                #region add_kafka_etl_task
                // Define a transformation script for the task: 
                // ============================================
                Transformation transformation = new Transformation
                {
                    // Define the input collections
                    Collections = { "Orders" },
                    ApplyToAllDocuments = false,
                    
                    // The transformation script
                    Name = "scriptName",
                    Script = @"// Create an orderData object
                               // ==========================
                               var orderData = {
                                   Id: id(this),
                                   OrderLinesCount: this.Lines.length,
                                   TotalCost: 0
                               };

                               // Update the orderData's TotalCost field
                               // ======================================
                               for (var i = 0; i < this.Lines.length; i++) {
                                   var line = this.Lines[i];
                                   var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
                                   orderData.TotalCost += cost;
                               }

                               // Load the object to the 'OrdersTopic' in Kafka
                               // ============================================= 
                               loadToOrdersTopic(orderData, {
                                   Id: id(this),
                                   PartitionKey: id(this),
                                   Type: 'com.example.promotions',
                                   Source: '/promotion-campaigns/summer-sale'
                               });"
                };

                // Define the Kafka ETL task:
                // ==========================
                var etlTask = new QueueEtlConfiguration()
                {
                    BrokerType = QueueBrokerType.Kafka,
                    
                    Name = "myKafkaEtlTaskName",
                    ConnectionStringName = "myKafkaConStr",
                    
                    Transforms = { transformation },

                    // Set to false to allow task failover to another node if current one is down
                    PinToMentorNode = false
                }; 
                
                // Deploy (send) the task to the server via the AddEtlOperation:
                // =============================================================
                store.Maintenance.Send(new AddEtlOperation<QueueConnectionString>(etlTask));
                #endregion
            }
        }

        public void DeleteProcessedDocuments()
        {
            using (var store = new DocumentStore())
            {
                Transformation transformation = new Transformation();  // Defined here only for compilation purposes
                
                #region kafka_delete_documents
                var etlTask = new QueueEtlConfiguration()
                {
                    BrokerType = QueueBrokerType.Kafka,
                    
                    Name = "myKafkaEtlTaskName",
                    ConnectionStringName = "myKafkaConStr",
                    
                    Transforms = { transformation },

                    // Define whether to delete documents from RavenDB after they are sent to the target topic
                    Queues = new List<EtlQueue>()
                    {
                        new()
                        {
                            // The name of the Kafka topic
                            Name = "OrdersTopic",
                                
                            // When set to 'true',
                            // documents that were processed by the transformation script will be deleted
                            // from RavenDB after the message is loaded to the "OrdersTopic" in Kafka.
                            DeleteProcessedDocuments = true
                        }
                    }
                }; 
                
                store.Maintenance.Send(new AddEtlOperation<QueueConnectionString>(etlTask));
                #endregion
            }
        }

        private interface IFoo
        {
            #region queue_broker_type
            public enum QueueBrokerType
            {
                None,
                Kafka,
                RabbitMq,
                AzureQueueStorage
            }
            #endregion
        }
        
        private class Definition
        {
            #region queue_connection_string
            public class QueueConnectionString : ConnectionString
            {
                // Set the broker type to QueueBrokerType.Kafka for a Kafka connection string
                public QueueBrokerType BrokerType { get; set; }
                
                // Configure this when setting a connection string for Kafka
                public KafkaConnectionSettings KafkaConnectionSettings { get; set; }
                
                // Configure this when setting a connection string for RabbitMQ
                public RabbitMqConnectionSettings RabbitMqConnectionSettings { get; set; }
                
                // Configure this when setting a connection string for Azure Queue Storage
                public AzureQueueStorageConnectionSettings AzureQueueStorageConnectionSettings { get; set; }
            }
            #endregion

            public abstract class ConnectionString
            {
                public string Name { get; set; }
            }
            
            #region kafka_con_str_settings
            public class KafkaConnectionSettings
            {
                // A string containing comma-separated keys of "host:port" URLs to Kafka brokers
                public string BootstrapServers { get; set; }
                
                // Various configuration options
                public Dictionary<string, string> ConnectionOptions { get; set; }
                
                public bool UseRavenCertificate { get; set; }
            }
            #endregion

            #region etl_configuration
            public class QueueEtlConfiguration
            {
                // Set to QueueBrokerType.Kafka to define a Kafka ETL task
                public QueueBrokerType BrokerType { get; set; }
                // The ETL task name
                public string Name { get; set; }
                // The registered connection string name
                public string ConnectionStringName { get; set; }
                // List of transformation scripts
                public List<Transformation> Transforms { get; set; }
                // Optional configuration per queue
                public List<EtlQueue> Queues { get; set; }
                // Set to 'false' to allow task failover to another node if current one is down
                public bool PinToMentorNode { get; set; }
            }
            
            public class Transformation
            {
                // The script name
                public string Name { get; set; }
                // The source RavenDB collections that serve as the input for the script
                public List<string> Collections { get; set; }
                // Set whether to apply the script on all collections
                public bool ApplyToAllDocuments { get; set; }
                // The script itself
                public string Script { get; set; }
            }
            
            public class EtlQueue
            {
                // The Kafka topic name
                public string Name { get; set; }
                // Delete processed documents when set to 'true'
                public bool DeleteProcessedDocuments { get; set; }
            }
            #endregion
        }
    }
}
