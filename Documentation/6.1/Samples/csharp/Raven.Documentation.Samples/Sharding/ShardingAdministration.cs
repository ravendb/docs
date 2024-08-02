using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide.Sharding;

namespace Raven.Documentation.Samples.Sharding
{
    class Program1
    {
        static void Main(string[] args)
        {
        }

        static async Task MainInternal()
        {
            using (var store = new DocumentStore()
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products",
            }.Initialize())
            {
                using (var session = store.OpenSession())
                {
                    var order = new Order
                    {
                    };
                    session.Store(order);
                    var invoice = new Invoice
                    {
                        OrderId = order.Id
                    };
                    #region storeInvoiceInOrderBucketExplicitNaming
                    // The invoice will be stored with the order ID as a suffix
                    session.Store(invoice, invoice.Id + "$" + order.Id);
                    session.SaveChanges();
                    #endregion
                }
            }

            #region storeInvoiceInOrderBucketNamingConvention
            // Store an invoice document in the same bucket as its order document

            // Define a naming convention for invoices
            // When an invoice is stored, the $ symbol and an order ID will be added to the invoice ID
            var conventions = new DocumentConventions();
            conventions.RegisterAsyncIdConvention<Invoice>(async (dbName, r) =>
            {
                var id = await conventions.AsyncDocumentIdGenerator(dbName, r);
                return $"{id}${r.OrderId}";
            });

            // Deploy the naming convention defined above
            using (var store = new DocumentStore()
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products",
                Conventions = conventions
            }.Initialize())
            {
                using (var session = store.OpenSession())
                {
                    var order = new Order();
                    session.Store(order);
                    
                    // The invoice will be stored with the order ID as a suffix
                    var invoice = new Invoice { OrderId = order.Id };
                    session.Store(invoice);
                    
                    session.SaveChanges();
                } 
            }
            #endregion
        }

        public void createShardedDatabase()
        {
            using (var store = new DocumentStore())
            {
                #region createShardedDatabase
                DatabaseRecord dbRecord = new DatabaseRecord("sampleDB");

                dbRecord.Sharding = new ShardingConfiguration
                {
                    Shards = new Dictionary<int, DatabaseTopology>()
                    {
                        { 0, new DatabaseTopology() }, // Shard #0 database topology
                        { 1, new DatabaseTopology() }, // Shard #1 database topology
                        { 2, new DatabaseTopology() }  // Shard #2 database topology
                    }
                };

                store.Maintenance.Server.Send(new CreateDatabaseOperation(dbRecord));
                #endregion
            }
        }

        public class Invoice
        {
            public string Id;
            public string OrderId;
        }

        public class Order
        {
            public string Id;
        }

        public class Foo
        {
            #region ShardingConfiguration_definition
            public class ShardingConfiguration
            {
                // Orchestrator configuration
                public OrchestratorConfiguration Orchestrator;

                // A database topology per shard dictionary
                public Dictionary<int, DatabaseTopology> Shards;

                // Buckets distribution between the shards (filled by RavenDB)
                public List<ShardBucketRange> BucketRanges = new List<ShardBucketRange>();

                // Buckets that are currently being resharded (filled by RavenDB)
                public Dictionary<int, ShardBucketMigration> BucketMigrations;
            }
            #endregion

                public class AddNodeToOrchestratorTopologyOperation
            {
                #region AddNodeToOrchestratorTopologyOperation_Definition
                public AddNodeToOrchestratorTopologyOperation(string databaseName, string node = null)
                #endregion
                {
                }
            }

            public class RemoveNodeFromOrchestratorTopologyOperation
            {
                #region RemoveNodeFromOrchestratorTopologyOperation_Definition
                public RemoveNodeFromOrchestratorTopologyOperation(string databaseName, string node)
                #endregion
                {
                }
            }

            #region ModifyOrchestratorTopologyResult
            public class ModifyOrchestratorTopologyResult
            {
                public string Name; // Database Name
                public OrchestratorTopology OrchestratorTopology; // Database Topology
                public long RaftCommandIndex;
            }
            #endregion

            public class AddDatabaseNodeOperation
            {
                #region AddDatabaseNodeOperation_Definition
                public AddDatabaseNodeOperation(string databaseName, int shardNumber, string node = null)
                #endregion
                {
                }
            }

            #region DatabasePutResult
            public class DatabasePutResult
            {
                public long RaftCommandIndex { get; set; }

                public string Name { get; set; }
                public DatabaseTopology Topology { get; set; }
                public List<string> NodesAddedTo { get; set; }

                public bool ShardsDefined { get; set; }
            }
            #endregion

            public class DeleteDatabasesOperation
            {
                #region DeleteDatabasesOperation_Definition
                public DeleteDatabasesOperation(
                    string databaseName, 
                    int shardNumber, 
                    bool hardDelete, 
                    string fromNode, 
                    TimeSpan? timeToWaitForConfirmation = null)
                #endregion
                {
                }
            }

            #region DeleteDatabaseResult
            public class DeleteDatabaseResult
            {
                public long RaftCommandIndex { get; set; }
                public string[] PendingDeletes { get; set; }
            }
            #endregion
            
            internal class AddDatabaseShardOperation
            {
                #region AddDatabaseShardOperation_Overload-1
                public AddDatabaseShardOperation(string databaseName, int? shardNumber = null)
                #endregion
                {
                }

                #region AddDatabaseShardOperation_Overload-2
                public AddDatabaseShardOperation(string databaseName, string[] nodes, int? shardNumber = null)
                #endregion
                {
                }

                #region AddDatabaseShardOperation_Overload-3
                public AddDatabaseShardOperation(string databaseName, int? replicationFactor, int? shardNumber = null)
                #endregion
                {
                }
            }

            #region AddDatabaseShardResult
            public class AddDatabaseShardResult
            {
                public string Name { get; set; }
                public int ShardNumber { get; set; }
                public DatabaseTopology ShardTopology { get; set; }
                public long RaftCommandIndex { get; set; }
            }
            #endregion

            public class PromoteDatabaseNodeOperation
            {
                #region PromoteDatabaseNodeOperation_Definition
                public PromoteDatabaseNodeOperation(string databaseName, int shardNumber, string node)
                #endregion
                {
                }
            }
        }
    }
}
