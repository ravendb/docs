using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Analysis;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Documents.Operations.Expiration;
using Raven.Client.Documents.Operations.Refresh;
using Raven.Client.Documents.Operations.Revisions;
using Raven.Client.Documents.Operations.TimeSeries;
using Raven.Client.Documents.Queries.Sorting;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide.Sharding;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class CreateDatabase
    {
        public void CreateDatabaseUsingDatabaseRecord()
        {
            using (var store = new DocumentStore())
            {
                #region create_database_1
                // Define the create database operation, pass an instance of DatabaseRecord
                var createDatabaseOp = new CreateDatabaseOperation(new DatabaseRecord("DatabaseName"));
                
                // Execute the operation by passing it to Maintenance.Server.Send
                store.Maintenance.Server.Send(createDatabaseOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region create_database_2
                // Define the database record:
                var databaseRecord = new DatabaseRecord("ShardedDatabaseName") {
                    
                    // Configure sharding:
                    Sharding = new ShardingConfiguration()
                    {
                        // Ensure nodes "A", "B", and "C" are available in the cluster
                        // before executing the database creation.
                        Shards = new Dictionary<int, DatabaseTopology>()
                        {
                            {0, new DatabaseTopology { Members = new List<string> { "A", "B" }}},
                            {1, new DatabaseTopology { Members = new List<string> { "A", "C" }}},
                            {2, new DatabaseTopology { Members = new List<string> { "B", "C" }}}
                        }
                    },
                    
                    // Enable revisions on all collections:
                    Revisions = new RevisionsConfiguration()
                    {
                        Default = new RevisionsCollectionConfiguration()
                        {
                            Disabled = false, MinimumRevisionsToKeep = 5
                        }
                    },
                    
                    // Enable the document expiration feature:
                    Expiration = new ExpirationConfiguration()
                    {
                        Disabled = false
                    },
                    
                    // Apply some database configuration setting:
                    Settings = new Dictionary<string, string>()
                    {
                        {"Databases.QueryTimeoutInSec", "500"}
                    }
                };

                // Define the create database operation
                var createDatabaseOp = new CreateDatabaseOperation(databaseRecord);
                
                // Execute the operation by passing it to Maintenance.Server.Send
                store.Maintenance.Server.Send(createDatabaseOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region create_database_3
                var databaseName = "MyDatabaseName";
                
                try
                {
                    // Try to fetch database statistics to check if the database exists
                    store.Maintenance.ForDatabase(databaseName)
                        .Send(new GetStatisticsOperation());
                }
                catch (DatabaseDoesNotExistException)
                {
                    try
                    {
                        // The database does not exist, try to create:
                        var createDatabaseOp = new CreateDatabaseOperation(
                            new DatabaseRecord(databaseName));
                        
                        store.Maintenance.Server.Send(createDatabaseOp);
                    }
                    catch (ConcurrencyException)
                    {
                        // The database was created by another client before this call completed
                    }
                }
                #endregion
            }
        }
        
        public async Task CreateDatabaseUsingDatabaseRecordAsync()
        {
            using (var store = new DocumentStore())
            {
                #region create_database_1_async
                // Define the create database operation, pass an instance of DatabaseRecord
                var createDatabaseOp = new CreateDatabaseOperation(new DatabaseRecord("DatabaseName"));
                
                // Execute the operation by passing it to Maintenance.Server.SendAsync
                await store.Maintenance.Server.SendAsync(createDatabaseOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region create_database_2_async
                // Define the database record:
                var databaseRecord = new DatabaseRecord("ShardedDatabaseName") {
                    
                    // Configure sharding:
                    Sharding = new ShardingConfiguration()
                    {
                        // Ensure nodes "A", "B", and "C" are available in the cluster
                        // before executing the database creation.
                        Shards = new Dictionary<int, DatabaseTopology>()
                        {
                            {0, new DatabaseTopology { Members = new List<string> { "A", "B" }}},
                            {1, new DatabaseTopology { Members = new List<string> { "A", "C" }}},
                            {2, new DatabaseTopology { Members = new List<string> { "B", "C" }}}
                        }
                    },
                    
                    // Enable revisions on all collections:
                    Revisions = new RevisionsConfiguration()
                    {
                        Default = new RevisionsCollectionConfiguration()
                        {
                            Disabled = false, MinimumRevisionsToKeep = 5
                        }
                    },
                    
                    // Enable the document expiration feature:
                    Expiration = new ExpirationConfiguration()
                    {
                        Disabled = false
                    },
                    
                    // Apply some database configuration setting:
                    Settings = new Dictionary<string, string>()
                    {
                        {"Databases.QueryTimeoutInSec", "500"}
                    }
                };
                
                // Define the create database operation
                var createDatabaseOp = new CreateDatabaseOperation(databaseRecord);
                
                // Execute the operation by passing it to Maintenance.Server.SendAsync
                await store.Maintenance.Server.SendAsync(createDatabaseOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region create_database_3_async
                var databaseName = "MyDatabaseName";
                
                try
                {
                    // Try to fetch database statistics to check if the database exists:
                    await store.Maintenance.ForDatabase(databaseName)
                        .SendAsync(new GetStatisticsOperation());
                }
                catch (DatabaseDoesNotExistException)
                {
                    try
                    {
                        // The database does not exist, try to create:
                        var createDatabaseOp = new CreateDatabaseOperation(
                            new DatabaseRecord(databaseName));
                        
                        await store.Maintenance.Server.SendAsync(createDatabaseOp);
                    }
                    catch (ConcurrencyException)
                    {
                        // The database was created by another client before this call completed
                    }
                }
                #endregion
            }
        }
        
        public void CreateDatabaseUsingBuilder()
        {
            using (var store = new DocumentStore())
            {
                #region create_using_builder_1
                // Define the create database operation
                var createDatabaseOp = new CreateDatabaseOperation(builder => builder
                     // Call 'Regular' to create a non-sharded database
                    .Regular("DatabaseName"));
                
                // Execute the operation by passing it to Maintenance.Server.Send
                store.Maintenance.Server.Send(createDatabaseOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region create_using_builder_2
                // Define the create database operation
                var createDatabaseOp = new CreateDatabaseOperation(builder => builder
                        
                    // Call 'Sharded' to create a sharded database
                    .Sharded("ShardedDatabaseName", topology => topology
                        // Ensure nodes "A", "B", and "C" are available in the cluster
                        // before executing the database creation.
                        .AddShard(0, new DatabaseTopology {Members = new List<string> {"A", "B"}})
                        .AddShard(1, new DatabaseTopology {Members = new List<string> {"A", "C"}})
                        .AddShard(2, new DatabaseTopology {Members = new List<string> {"B", "C"}}))
                    // Enable revisions on all collections:
                    .ConfigureRevisions(new RevisionsConfiguration()
                    {
                        Default = new RevisionsCollectionConfiguration()
                        {
                            Disabled = false, MinimumRevisionsToKeep = 5
                        }
                    })
                    // Enable the document expiration feature:
                    .ConfigureExpiration(new ExpirationConfiguration()
                    {
                        Disabled = false
                    })
                    // Apply some database configuration setting:
                    .WithSettings(new Dictionary<string, string>()
                    {
                        { "Databases.QueryTimeoutInSec", "500" }
                    })
                );
                
                // Execute the operation by passing it to Maintenance.Server.Send
                store.Maintenance.Server.Send(createDatabaseOp);
                #endregion
            }
        }

        public async Task CreateDatabaseUsingBuilderAsync()
        {
            using (var store = new DocumentStore())
            {
                #region create_using_builder_1_async
                // Define the create database operation
                var createDatabaseOp = new CreateDatabaseOperation(builder => builder
                     // Call 'Regular' to create a non-sharded database
                    .Regular("DatabaseName"));
                
                // Execute the operation by passing it to Maintenance.Server.SendAsync
                await store.Maintenance.Server.SendAsync(createDatabaseOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region create_using_builder_2_async
                // Define the create database operation
                var createDatabaseOp = new CreateDatabaseOperation(builder => builder
                        
                    // Call 'Sharded' to create a sharded database
                    .Sharded("ShardedDatabaseName", topology => topology
                        // Ensure nodes "A", "B", and "C" are available in the cluster
                        // before executing the database creation.
                        .AddShard(0, new DatabaseTopology {Members = new List<string> {"A", "B"}})
                        .AddShard(1, new DatabaseTopology {Members = new List<string> {"A", "C"}})
                        .AddShard(2, new DatabaseTopology {Members = new List<string> {"B", "C"}}))
                    // Enable revisions on all collections:
                    .ConfigureRevisions(new RevisionsConfiguration()
                    {
                        Default = new RevisionsCollectionConfiguration()
                        {
                            Disabled = false, MinimumRevisionsToKeep = 5
                        }
                    })
                    // Enable the document expiration feature:
                    .ConfigureExpiration(new ExpirationConfiguration()
                    {
                        Disabled = false
                    })
                    // Apply some database configuration setting:
                    .WithSettings(new Dictionary<string, string>()
                    {
                        { "Databases.QueryTimeoutInSec", "500" }
                    })
                );
                
                // Execute the operation by passing it to Maintenance.Server.SendAsync
                await store.Maintenance.Server.SendAsync(createDatabaseOp);
                #endregion
            }
        }
        
        private class Foo
        {
            public class CreateDatabaseOperation
            {
                #region create_database_operation_syntax
                // CreateDatabaseOperation overloads:
                // ==================================
                public CreateDatabaseOperation(DatabaseRecord databaseRecord) {}
                public CreateDatabaseOperation(DatabaseRecord databaseRecord, int replicationFactor) {}
                public CreateDatabaseOperation(Action<IDatabaseRecordBuilderInitializer> builder) {}
                #endregion
            }
            
            #region builder_syntax
            public interface IDatabaseRecordBuilderInitializer
            {
                public IDatabaseRecordBuilder Regular(string databaseName);
                public IShardedDatabaseRecordBuilder Sharded(string databaseName, Action<IShardedTopologyConfigurationBuilder> builder);
                public DatabaseRecord ToDatabaseRecord();
            }

            public interface IShardedDatabaseRecordBuilder : IDatabaseRecordBuilderBase
            {
            }
            
            // Available configurations:
            // =========================
            
            public interface IDatabaseRecordBuilder : IDatabaseRecordBuilderBase
            {
                public IDatabaseRecordBuilderBase WithTopology(DatabaseTopology topology);
                public IDatabaseRecordBuilderBase WithTopology(Action<ITopologyConfigurationBuilder> builder);
                public IDatabaseRecordBuilderBase WithReplicationFactor(int replicationFactor);
            }

            public interface IDatabaseRecordBuilderBase
            {
                DatabaseRecord ToDatabaseRecord();

                IDatabaseRecordBuilderBase ConfigureClient(ClientConfiguration configuration);
                IDatabaseRecordBuilderBase ConfigureDocumentsCompression(DocumentsCompressionConfiguration configuration);
                IDatabaseRecordBuilderBase ConfigureExpiration(ExpirationConfiguration configuration);
                IDatabaseRecordBuilderBase ConfigureRefresh(RefreshConfiguration configuration);
                IDatabaseRecordBuilderBase ConfigureRevisions(RevisionsConfiguration configuration);
                IDatabaseRecordBuilderBase ConfigureStudio(StudioConfiguration configuration);
                IDatabaseRecordBuilderBase ConfigureTimeSeries(TimeSeriesConfiguration configuration);

                IDatabaseRecordBuilderBase Disabled();
                IDatabaseRecordBuilderBase Encrypted();

                IDatabaseRecordBuilderBase WithAnalyzers(params AnalyzerDefinition[] analyzerDefinitions);
                IDatabaseRecordBuilderBase WithConnectionStrings(Action<IConnectionStringConfigurationBuilder> builder);
                IDatabaseRecordBuilderBase WithIndexes(params IndexDefinition[] indexDefinitions);
                IDatabaseRecordBuilderBase WithIntegrations(Action<IIntegrationConfigurationBuilder> builder);
                IDatabaseRecordBuilderBase WithLockMode(DatabaseLockMode lockMode);
                IDatabaseRecordBuilderBase WithSettings(Dictionary<string, string> settings);
                IDatabaseRecordBuilderBase WithSettings(Action<Dictionary<string, string>> builder);
                IDatabaseRecordBuilderBase WithSorters(params SorterDefinition[] sorterDefinitions);
            }
            #endregion
        }
    }
}
