using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.Queue;
namespace Raven.Documentation.Samples.Server.OngoingTasks.ETL.Queue
{
    public class RabbitMqEtl
    {
        void AddConnectionString()
        {
            using (var store = new DocumentStore())
            {
                #region add_rabbitMq_connection_string
                // Prepare the connection string:
                // ==============================
                var conStr = new QueueConnectionString
                {
                    // Provide a name for this connection string
                    Name = "myRabbitMqConStr",
                    
                    // Set the broker type
                    BrokerType = QueueBrokerType.RabbitMq,
                    
                    // Configure the connection details
                    RabbitMqConnectionSettings = new RabbitMqConnectionSettings() 
                        { ConnectionString = "amqp://guest:guest@localhost:49154" }
                };
                
                // Deploy (send) the connection string to the server via the PutConnectionStringOperation:
                // =======================================================================================
                var res = store.Maintenance.Send(
                    new PutConnectionStringOperation<QueueConnectionString>(conStr));
                #endregion
            }
        }

        public void AddRabbitmqEtlTask()
        {
            using (var store = new DocumentStore())
            {
                #region add_rabbitMq_etl_task
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

                               // Load the object to the 'OrdersExchange' in RabbitMQ
                               // =================================================== 
                               loadToOrdersExchange(orderData, `routingKey`, {  
                                   Id: id(this),
                                   Type: 'com.example.promotions',
                                   Source: '/promotion-campaigns/summer-sale'
                               });"
                };

                // Define the RabbitMQ ETL task:
                // =============================
                var etlTask = new QueueEtlConfiguration()
                {
                    BrokerType = QueueBrokerType.RabbitMq,
                    
                    Name = "myRabbitMqEtlTaskName",
                    ConnectionStringName = "myRabbitMqConStr",
                    
                    Transforms = { transformation },

                    // Set to false to have the RabbitMQ client library declare the queue if does not exist
                    SkipAutomaticQueueDeclaration = false,
                    
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
                Transformation transformation = new Transformation(); // Defined here only for compilation purposes
                
                #region rabbitMq_delete_documents
                var etlTask = new QueueEtlConfiguration()
                {
                    BrokerType = QueueBrokerType.RabbitMq,
                    
                    Name = "myRabbitMqEtlTaskName",
                    ConnectionStringName = "myRabbitMqConStr",
                    
                    Transforms = { transformation },

                    // Define whether to delete documents from RavenDB after they are sent to RabbitMQ
                    Queues = new List<EtlQueue>()
                    {
                        new()
                        {
                            // The name of the target queue
                            Name = "OrdersQueue",

                            // When set to 'true',
                            // documents that were processed by the transformation script will be deleted
                            // from RavenDB after the message is loaded to the "OrdersQueue" in RabbitMQ.
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
                // Set the broker type to QueueBrokerType.RabbitMq for a RabbitMQ connection string
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
            
            #region rabbitMq_con_str_settings
            public sealed class RabbitMqConnectionSettings
            {
                // A single string that specifies the RabbitMQ exchange connection details
                public string ConnectionString { get; set; }
            }
            #endregion

            #region etl_configuration
            public class QueueEtlConfiguration
            {
                // Set to QueueBrokerType.RabbitMq to define a RabbitMQ ETL task
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
                
                // Set to 'false' to have the RabbitMQ client library declare the queue if does not exist.
                // Set to 'true' to skip automatic queue declaration, 
                // use this option when you prefer to define Exchanges, Queues & Bindings manually.
                public bool SkipAutomaticQueueDeclaration { get; set; }
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
                // The RabbitMQ target queue name
                public string Name { get; set; }
                // Delete processed documents when set to 'true'
                public bool DeleteProcessedDocuments { get; set; }
            }
            #endregion
        }
    }
}
