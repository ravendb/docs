using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using System.Threading.Tasks;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide.Sharding;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Sharding
{
    public class ShardingByPrefix
    {
        public async Task AddPrefixesWhenCreatingDatabase()
        {
            using (var store = new DocumentStore())
            {
                #region prefix_1
                // Define the database record:
                // ===========================
                var databaseRecord = new DatabaseRecord
                {
                    // Provide a name for the new database
                    DatabaseName = "SampleDB",

                    // Set the sharding topology configuration
                    // Here each shard will have a replication factor of 2 nodes
                    Sharding = new ShardingConfiguration
                    {
                        Shards = new Dictionary<int, DatabaseTopology>
                        {
                            [0] = new() { Members = new List<string> { "A", "B" } },
                            [1] = new() { Members = new List<string> { "A", "C" } },
                            [2] = new() { Members = new List<string> { "C", "B" } }
                        }
                    }
                };

                // Define prefixes and their target shard/s:
                // =========================================
                databaseRecord.Sharding.Prefixed =
                [
                    new PrefixedShardingSetting
                    {
                        Prefix = "users/us/",
                        // Assign a SINGLE shard for the prefix
                        Shards = [0]
                    },
                    new PrefixedShardingSetting
                    {
                        Prefix = "users/asia/",
                        // Can assign MULTIPLE shards for a prefix
                        Shards = [1, 2]
                    }
                ];

                // Deploy the new database to the server: 
                // ======================================
                store.Maintenance.Server.Send(new CreateDatabaseOperation(databaseRecord));

                // You can verify the sharding configuration that has been created:
                // ================================================================
                var record = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));
                
                var shardingConfiguration = record.Sharding;
                var numberOfShards = shardingConfiguration.Shards.Count;     // 3
                var numberOfPrefixes = shardingConfiguration.Prefixed.Count; // 2
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region prefix_1_async
                // Define the database record:
                // ===========================
                var databaseRecord = new DatabaseRecord
                {
                    // Provide a name for the new database
                    DatabaseName = "SampleDB",

                    // Set the sharding topology configuration
                    // Here each shard will have a replication factor of 2 nodes
                    Sharding = new ShardingConfiguration
                    {
                        Shards = new Dictionary<int, DatabaseTopology>
                        {
                            [0] = new() { Members = new List<string> { "A", "B" } },
                            [1] = new() { Members = new List<string> { "A", "C" } },
                            [2] = new() { Members = new List<string> { "C", "B" } }
                        }
                    }
                };

                // Define prefixes and their target shard/s:
                // =========================================
                databaseRecord.Sharding.Prefixed =
                [
                    new PrefixedShardingSetting
                    {
                        Prefix = "users/us/",
                        // Assign a SINGLE shard for the prefix
                        Shards = [0]
                    },
                    new PrefixedShardingSetting
                    {
                        Prefix = "users/asia/",
                        // Can assign MULTIPLE shards for a prefix
                        Shards = [1, 2]
                    }
                ];

                // Deploy the new database to the server: 
                // ======================================
                await store.Maintenance.Server.SendAsync(new CreateDatabaseOperation(databaseRecord));

                // You can verify the sharding configuration that has been created:
                // ================================================================
                var record = await store.Maintenance.Server.SendAsync(new GetDatabaseRecordOperation(store.Database));
                
                var shardingConfiguration = record.Sharding;
                var numberOfShards = shardingConfiguration.Shards.Count;     // 3
                var numberOfPrefixes = shardingConfiguration.Prefixed.Count; // 2
                #endregion
            }
        }
        
        public async Task AddPrefixesUsingOperation()
        {
            using (var store = new DocumentStore())
            {
                #region prefix_2
                // Define the prefix to add and its target shard/s
                var shardingSetting = new PrefixedShardingSetting
                {
                    Prefix = "users/eu/",
                    Shards = [2]
                    // Can assign multiple shards, e.g.: Shards = [2, 3]
                };

                // Define the add operation:
                var addPrefixOp = new AddPrefixedShardingSettingOperation(shardingSetting);
                
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(addPrefixOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region prefix_2_async
                // Define the prefix to add and its target shard/s
                var shardingSetting = new PrefixedShardingSetting
                {
                    Prefix = "users/eu/",
                    Shards = [2]
                    // Can assign multiple shards, e.g.: Shards = [2, 3]
                };

                // Define the add operation:
                var addPrefixOp = new AddPrefixedShardingSettingOperation(shardingSetting);
                
                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(addPrefixOp);
                #endregion
            }
        }
        
        public async Task DeletePrefixesUsingOperation()
        {
            using (var store = new DocumentStore())
            {
                #region prefix_3
                // Define the delete prefix operation,
                // Pass the prefix string
                var deletePrefixOp = new DeletePrefixedShardingSettingOperation("users/eu/");
                
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(deletePrefixOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region prefix_3_async
                // Define the delete prefix operation,
                // Pass the prefix string
                var deletePrefixOp = new DeletePrefixedShardingSettingOperation("users/eu/");
                
                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(deletePrefixOp);
                #endregion
            }
        }
        
        public async Task UpdatePrefixesUsingOperation()
        {
            using (var store = new DocumentStore())
            {
                #region prefix_4
                // Define the shards configuration you wish to update for the specified prefix
                var shardingSetting = new PrefixedShardingSetting
                {
                    Prefix = "users/eu/",
                    // Adding shard #0 to the previous prefix configuration
                    Shards = [0, 2]
                };

                // Define the update operation:
                var updatePrefixOp = new UpdatePrefixedShardingSettingOperation(shardingSetting);
                
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(updatePrefixOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region prefix_4_async
                // Define the shards configuration you wish to update for the specified prefix
                var shardingSetting = new PrefixedShardingSetting
                {
                    Prefix = "users/eu/",
                    // Adding shard #0 to the previous prefix configuration
                    Shards = [0, 2]
                };

                // Define the update operation:
                var updatePrefixOp = new UpdatePrefixedShardingSettingOperation(shardingSetting);
                
                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(updatePrefixOp);
                #endregion
            }
        }

        public async Task QueryShardsByPrefix()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query_1
                    // Query for 'Company' documents from shard/s assigned to a specific prefix:
                    // =========================================================================
                    var companyDocs = session.Query<Company>()
                         // Call 'ShardContext' to select which shard/s to query
                         // RavenDB will query only the shard/s assigned to prefix 'users/us/'
                        .Customize(x => x.ShardContext(s => s.ByPrefix("users/us/")))
                         // The query predicate
                        .Where(x => x.Address.Country == "US")
                        .ToList();
                    
                    // Variable 'companyDocs' will include all documents of type 'Company'
                    // that match the query predicate and reside on the shard/s assigned to prefix 'users/us/'.
                    
                    // Query for ALL documents from shard/s assigned to a specific prefix:
                    // ===================================================================
                    var allDocs = session.Query<object>() // query with <object>
                        .Customize(x => x.ShardContext(s => s.ByPrefix("users/us/")))
                        .ToList();
                    
                    // Variable 'allDocs' will include ALL documents that reside on
                    // the shard/s assigned to prefix 'users/us/'.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_1_async
                    // Query for 'Company' documents from shard/s assigned to a specific prefix:
                    // =========================================================================
                    var companyDocs = await asyncSession.Query<Company>()
                        .Customize(x => x.ShardContext(s => s.ByPrefix("users/us/")))
                        .Where(x => x.Address.Country == "US")
                        .ToListAsync();
                    
                    // Query for ALL documents from shard/s assigned to a specific prefix:
                    // ===================================================================
                    var allDocs = await asyncSession.Query<object>()
                        .Customize(x => x.ShardContext(s => s.ByPrefix("users/us/")))
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_2
                    // Query for 'Company' documents from shard/s assigned to a specific prefix:
                    // =========================================================================
                    var companyDocs = session.Advanced.DocumentQuery<Company>()
                        .ShardContext(s => s.ByPrefix("users/us/"))
                        .WhereEquals(x => x.Address.Country, "US")
                        .ToList();
                    
                    // Query for ALL documents from shard/s assigned to a specific prefix:
                    // ===================================================================
                    var allDocs = session.Advanced.DocumentQuery<Company>()
                        .ShardContext(s => s.ByPrefix("users/us/"))
                        .ToList();
                    #endregion
                }
            }
        }
        
        public async Task QueryShardsByPrefixes()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query_3
                    // Query for 'Company' documents from shard/s assigned to the specified prefixes:
                    // ==============================================================================
                    var companyDocs = session.Query<Company>()
                         // Call 'ShardContext' to select which shard/s to query
                         // RavenDB will query only the shard/s assigned to prefixes 'users/us/' or 'users/asia/'
                        .Customize(x => x.ShardContext(s => s.ByPrefixes(["users/us/", "users/asia/"])))
                         // The query predicate
                        .Where(x => x.Address.Country == "US")
                        .ToList();
                    
                    // Variable 'companyDocs' will include all documents of type 'Company'
                    // that match the query predicate and reside on the shard/s
                    // assigned to prefix 'users/us/' or prefix 'users/asia/'.
                    
                    // Query for ALL documents from shard/s assigned to the specified prefixes:
                    // ========================================================================
                    var allDocs = session.Query<object>() // query with <object>
                        .Customize(x => x.ShardContext(s => s.ByPrefixes(["users/us/", "users/asia/"])))
                        .ToList();
                    
                    // Variable 'allDocs' will include all documents reside on the shard/s
                    // assigned to prefix 'users/us/' or prefix 'users/asia/'.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_3_async
                    // Query for 'Company' documents from shard/s assigned to the specified prefixes:
                    // ==============================================================================
                    var companyDocs = await asyncSession.Query<Company>()
                        .Customize(x => x.ShardContext(s => s.ByPrefixes(["users/us/", "users/asia/"])))
                        .Where(x => x.Address.Country == "US")
                        .ToListAsync();
                    
                    // Query for ALL documents from shard/s assigned to the specified prefixes:
                    // ========================================================================
                    var allDocs = await asyncSession.Query<object>()
                        .Customize(x => x.ShardContext(s => s.ByPrefixes(["users/us/", "users/asia/"])))
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_4
                    // Query for 'Company' documents from shard/s assigned to the specified prefixes:
                    // ==============================================================================
                    var companyDocs = session.Advanced.DocumentQuery<Company>()
                        .ShardContext(s => s.ByPrefixes(["users/us/", "users/asia/"]))
                        .WhereEquals(x => x.Address.Country, "US")
                        .ToList();
                    
                    // Query for ALL documents from shard/s assigned to the specified prefixes:
                    // ========================================================================
                    var allDocs = session.Advanced.DocumentQuery<Company>()
                        .ShardContext(s => s.ByPrefixes(["users/us/", "users/asia/"]))
                        .ToList();
                    #endregion
                }
            }
        }
        
        public async Task QueryShardsByPrefixAndByDocumentId()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query_5
                    // Query for 'Company' documents from shard/s assigned to a prefix & by document ID:
                    // =================================================================================
                    var companyDocs = session.Query<Company>()
                        .Customize(x => x.ShardContext(s => 
                            s.ByPrefix("users/us/").ByDocumentId("companies/1")))
                        .Where(x => x.Address.Country == "US")
                        .ToList();
                    
                    // Variable 'companyDocs' will include all documents of type 'Company'
                    // that match the query predicate and reside on:
                    // * the shard/s assigned to prefix 'users/us/'
                    // * or the shard containing document 'companies/1'.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_5_async
                    // Query for 'Company' documents from shard/s assigned to a prefix & by document ID:
                    // =================================================================================
                    var companyDocs = await asyncSession.Query<Company>()
                        .Customize(x => x.ShardContext(s => 
                            s.ByPrefix("users/us/").ByDocumentId("companies/1")))
                        .Where(x => x.Address.Country == "US")
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_6
                    // Query for 'Company' documents from shard/s assigned to a prefix & by document ID:
                    // =================================================================================
                    var companyDocs = session.Advanced.DocumentQuery<Company>()
                        .ShardContext(s => 
                            s.ByPrefix("users/us/").ByDocumentId("companies/1"))
                        .WhereEquals(x => x.Address.Country, "US")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
