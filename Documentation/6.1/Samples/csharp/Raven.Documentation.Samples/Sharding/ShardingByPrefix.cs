using System.Collections.Generic;
using Raven.Client.Documents;
using System.Threading.Tasks;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide.Sharding;

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
                        // Assign a the target shard for the prefix
                        Shards = [0]
                    },
                    new PrefixedShardingSetting
                    {
                        Prefix = "users/asia/",
                        // Can assign multiple shards for a prefix
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
                        // Assign a the target shard for the prefix
                        Shards = [0]
                    },
                    new PrefixedShardingSetting
                    {
                        Prefix = "users/asia/",
                        // Can assign multiple shards for a prefix
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
    }
}
