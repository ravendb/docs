using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.Queue;
namespace Raven.Documentation.Samples.Server.OngoingTasks.ETL.Queue
{
    public class AmazonSqsEtl
    {
        public void AddConnectionString()
        {
            string SqsAccessKey = "SqsAccessKey";
            string SqsSecretKey = "SqsSecretKey";
            string SqsRegionName = "SqsRegionName";

            using (var store = new DocumentStore())
            {
                #region add_sqs_connection_string
                // Prepare the connection string:
                // ==============================
                var conStr = new QueueConnectionString
                {
                    // Provide a name for this connection string
                    Name = "mySqsConStr", 
                    
                    // Set the broker type
                    BrokerType = QueueBrokerType.AmazonSqs,

                    AmazonSqsConnectionSettings = new AmazonSqsConnectionSettings()
                    {
                        // Define whether to use a password or not.
                        // Set to `true` to authorize a dedicated machine that requires no password.
                        // You can only use this option in self-hosted mode.
                        Passwordless = false,

                        // Sqs destination authorization parameters 
                        Basic =
                            {
                                AccessKey = SqsAccessKey,
                                SecretKey = SqsSecretKey,
                                RegionName = SqsRegionName
                            },
                    }
                };
                
                // Deploy (send) the connection string to the server via the PutConnectionStringOperation:
                // =======================================================================================
                var res = store.Maintenance.Send(
                    new PutConnectionStringOperation<QueueConnectionString>(conStr));
                #endregion
            }
        }

        public void AddAmazonSqsEtlTask()
        {
            using (var store = new DocumentStore())
            {
                #region add_amazon_sqs_etl_task
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

                               // Load the object to the 'OrdersQueue' ququq on the SQS destination
                               // ================================================================= 
                               loadToOrdersQueue(orderData, {
                                   Id: id(this),
                                   Type: 'com.example.promotions',
                                   Source: '/promotion-campaigns/summer-sale'
                               });"
                };

                // Define the SQS ETL task
                // =======================
                var etlTask = new QueueEtlConfiguration()
                {
                    BrokerType = QueueBrokerType.AmazonSqs,
                    
                    Name = "myAmazonSqsEtlTaskName",
                    ConnectionStringName = "myAmazonSqsConStr",
                    
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

        public void AmazonSqsUseFifo()
        {
            using (var store = new DocumentStore())
            {
                Transformation transformation = new Transformation
                {
                    // Input collections
                    Collections = { "Orders" },

                    ApplyToAllDocuments = false,

                    // Transformation script
                    Name = "scriptName",
                    
                    #region sqs_fifo                    
                    Script = @"// Create an orderData object
                               // ==========================
                               var orderData = {
                                   Id: id(this), // property with RavenDB document ID
                                   OrderLinesCount: this.Lines.length,
                                   TotalCost: 0
                               };

                               for (var i = 0; i < this.Lines.length; i++) {
                                   var line = this.Lines[i];
                                   var cost = (line.Quantity * line.PricePerUnit) * ( 1 - line.Discount);
                                   orderData.TotalCost += cost;
                               }

                               // Load the object to the FIFO 'Orders' ququq on the SQS destination
                               // ================================================================= 
                               loadTo('orders.fifo', orderData, {
                                   Id: id(this),
                                   Type: 'com.github.users',
                                   Source: '/registrations/direct-signup'
                               });"
                    #endregion

                };
            }
        }
        

        public void DeleteProcessedDocuments()
        {
            using (var store = new DocumentStore())
            {
                Transformation transformation = new Transformation(); // Defined here only for compilation purposes
                
                #region sqs_delete_documents
                var etlTask = new QueueEtlConfiguration()
                {
                    BrokerType = QueueBrokerType.AmazonSqs,
                    
                    Name = "myAmazonSqsEtlTaskName",
                    ConnectionStringName = "myAmazonSqsConStr",
                    
                    Transforms = { transformation },

                    // Define whether to delete documents from RavenDB after they are sent to the target queue
                    Queues = new List<EtlQueue>()
                    {
                        new()
                        {
                            // The name of the SQS queue  
                            Name = "OrdersQueue",

                            // When set to 'true',
                            // documents that were processed by the transformation script will be deleted
                            // from RavenDB after the message is loaded to the target queue
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
                AzureQueueStorage,
                AmazonSqs
            }
            #endregion
        }
        
        private class Definition
        {
            #region queue_connection_string
            public class QueueConnectionString : ConnectionString
            {
                // Set to QueueBrokerType.AmazonSqs for an SQS connection string
                public QueueBrokerType BrokerType { get; set; }
                
                // Configure this when setting a connection string for Kafka
                public KafkaConnectionSettings KafkaConnectionSettings { get; set; }
                
                // Configure this when setting a connection string for RabbitMQ
                public RabbitMqConnectionSettings RabbitMqConnectionSettings { get; set; }

                // Configure this when setting a connection string for Azure Queue Storage
                public AzureQueueStorageConnectionSettings AzureQueueStorageConnectionSettings { get; set; }

                // Configure this when setting a connection string for Amazon SQS
                public AmazonSqsConnectionSettings AmazonSqsConnectionSettings { get; set; }
            }
            #endregion

            public abstract class ConnectionString
            {
                public string Name { get; set; }
            }

            #region amazon_sqs_con_str_settings
            public class AmazonSqsConnectionSettings
            {
                public Basic Basic { get; set; }
                public bool Passwordless { get; set; }
            }
            public class Basic
            {
                public string AccessKey { get; set; }
                public string SecretKey { get; set; }
                public string RegionName { get; set; }
            }
            #endregion

            #region etl_configuration
            public class QueueEtlConfiguration
            {
                // Set to QueueBrokerType.AmazonSqs to define an SQS Queue ETL task
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
                // The SQS queue name
                public string Name { get; set; }
                // Delete processed documents when set to 'true'
                public bool DeleteProcessedDocuments { get; set; }
            }
            #endregion

        }
    }
}
